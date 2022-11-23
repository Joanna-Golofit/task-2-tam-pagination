using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base;
using TeamsAllocationManager.Infrastructure.Extensions;

namespace TeamsAllocationManager.Api.Functions;

[Authorize]
public class FunctionBase
{
	private const char ParameterStarter = '{';
	private readonly char[] _pathSeparators = { '/' };

	public HttpRequest Request { get; private set; } = null!;
	protected string CurrentUsername { get; private set; } = null!;
	private ILogger _logger = null!;
	protected readonly IDispatcher _dispatcher;

	public FunctionBase(IDispatcher dispatcher)
	{
		_dispatcher = dispatcher;
	}

	public virtual async Task<IActionResult> RunAsync(HttpRequest req, string? path, ILogger log)
	{
		try
		{
			return await ExecuteFunction(req, path, log);
		}
		catch(Exception e)
		{
			return e.ToObjectResult();
		}
	}

	private async Task<IActionResult> ExecuteFunction(HttpRequest req, string? path, ILogger log)
	{
		Request = req;
		path ??= string.Empty;
		_logger = log;

		using (_logger.BeginScope(new Dictionary<string, object>
			    {
				    ["AppVersion"] = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "-",
				    ["RequestUrl"] = req.Path.Value!,
				    ["HttpMethod"] = req.Method
			    }))
		{
			(string? idToken, string? accessToken) = GetAadTokens();
			CurrentUsername = GetCurrentUser(string.IsNullOrWhiteSpace(idToken) ? accessToken : idToken)!;

			if (string.IsNullOrWhiteSpace(CurrentUsername))
			{
				_logger.LogWarning($"CurrentUsername is empty. idToken {(string.IsNullOrWhiteSpace(idToken) ? "is empty" : "exists")}, accessToken {(string.IsNullOrWhiteSpace(accessToken) ? "is empty" : "exists")}");
			}

			using (_logger.BeginScope(new Dictionary<string, object>
				    {
					    ["LoggedUser"] = CurrentUsername,
				    }))
			{
				if (HttpMethods.IsOptions(req.Method))
				{
					return new OkResult(); // for now, let's return 200 for all preflight requests
				}

				MethodData[] handlers = await ApplyParamFilterAsync(GetHandlers(req, path), path);

				if (handlers.Length > 0 && handlers.Count(h => h.NotFitted == 0 && h.Parameters.Length > 0) <= 1)
				{
					var handler = handlers.First();
					await handler.ValidateAccess(_dispatcher, CurrentUsername);

					foreach (var param in handler.Params)
					{
						if (param != null && !param.IsValid(out var validationResults))
						{
							return BadRequest(validationResults);
						}
					}

					return (await HandleAsync(handler)) ?? new OkResult();
				}
				else
				{
					_logger.LogWarning("No action found");
				}

				return BadRequest(handlers.Length);
			}
		}
	}

	private async Task<MethodData[]> ApplyParamFilterAsync(MethodData[] handlers, string path)
	{
		var result = new List<MethodData>();

		foreach (MethodData handler in handlers)
		{
			bool fits = await GetCallParametersAsync(handler, path, handler.Params);
			handler.NotFitted = handler.Parameters.Count() - handler.Params.Count(p => p != null);

			if (fits)
			{
				result.Add(handler);
			}
		}

		return result.OrderBy(m => m.NotFitted).ThenByDescending(m => m.Parameters.Count()).ToArray();
	}

	private BadRequestObjectResult BadRequest(int count)
	{
		var message = $"Found {count} handlers for this request. Make sure, there is only one possible handler.";

		_logger.LogWarning(message);

		return new BadRequestObjectResult(message);
	}

	private BadRequestObjectResult BadRequest(ICollection<ValidationResult> validationResults)
	{
		var message = $"Model is invalid: {string.Join(", ", validationResults.Select(s => s.ErrorMessage!.TrimEnd('.')))}.";
		_logger.LogWarning(message);

		return new BadRequestObjectResult(message);
	}

	private MethodData[] GetHandlers(HttpRequest req, string path)
	{
		return GetType()
				.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
				.Select(method => new MethodData(method))
				.Where(methodData => (methodData.Method.Name.StartsWith(req.Method, StringComparison.OrdinalIgnoreCase) ||
									methodData.Attribute?.HttpMethods?.Any(a => a.Equals(req.Method, StringComparison.OrdinalIgnoreCase)) == true) &&
									IsMethodPathCorrect(methodData.Attribute, path, methodData.Method.GetParameters()))
				.ToArray();
	}

	private async Task<IActionResult?> HandleAsync(MethodData method)
	{
		using (_logger.BeginScope(new Dictionary<string, object>
		{
			["Action"] = method.Method.Name,
			["ParameterName"] = method.Parameters.Any() ? (method.Parameters[0].Name ?? "-") : "-",
			["ParameterValue"] = method.Params.Any() ? method.Params[0].SerializeToJson() : "-"
		}))
		{
			try
			{
				string printedUser = !string.IsNullOrEmpty(CurrentUsername) ? CurrentUsername : "?";
				_logger.LogInformation($"Handling action started (action: {method.Method.Name}, user: {printedUser})");

				return (await GetResultAsync(method.Method.Invoke(this, method.Params.ToArray()))) as IActionResult;
			}
			catch (Exception ex)
			{
				LogException(_logger, ex);
				throw;
			}
		}
	}

	protected void LogException(ILogger logger, Exception e, int innerExceptionCounter = 0)
	{
		logger.LogInformation($"*** {(innerExceptionCounter == 0 ? "EXCEPTION" : $"INNER EXCEPTION, DEPTH: {innerExceptionCounter}")} ({e.GetType().Name}) | MESSAGE: {e.Message} | STACK TRACE: {e.StackTrace}");

		if (e.InnerException != null)
		{
			innerExceptionCounter++;
			LogException(logger, e.InnerException, innerExceptionCounter);
		}
	}

	private async Task<bool> GetCallParametersAsync(MethodData methodData, string path, IList<object?> paramsValues)
	{
		string[]? splittedTemplate = methodData.Attribute?.Template?.Split(_pathSeparators, StringSplitOptions.RemoveEmptyEntries);
		var pathParameters = new Dictionary<string, string>();

		GetRawParameters(path, splittedTemplate, pathParameters);

		return await GetTypedParametersAsync(methodData, pathParameters, paramsValues);
	}

	private async Task<bool> GetTypedParametersAsync(MethodData method, Dictionary<string, string> pathParameters, IList<object?> paramsValues)
	{
		ParameterInfo[] methodParameters = method.Parameters;
		string requestBody = "";

		try
		{
			Request.Body.Position = 0; // resets Streams's position to zero in case of multiple reads/tests of Request Body
			requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
		}
		catch { }

		foreach (ParameterInfo methodParam in methodParameters)
		{
			string param = pathParameters.ContainsKey(methodParam.Name!) ?
				pathParameters[methodParam.Name!] :
				requestBody;
			Type type = methodParam.ParameterType;
			object? typedParam;

			if (methodParam.CustomAttributes.Any(a => a.AttributeType == typeof(FromQueryAttribute)))
			{
				typedParam = QuerySerializer.Deserialize(type, Request.Query, parameterName: methodParam.Name);
			}
			else if ((type.IsClass || type.IsInterface) && !type.Equals(typeof(string)) && !string.IsNullOrEmpty(param))
			{
				try
				{
					typedParam = JsonConvert.DeserializeObject(requestBody, type);
				}
				catch (Exception) // the case when JSON deserializer encounters unexpected type (multiple handlers can have different input types)
				{
					return false;
				}
			}
			else if (!string.IsNullOrEmpty(param))
			{
				try
				{
					typedParam = type == typeof(Guid) ? TypeDescriptor.GetConverter(typeof(Guid)).ConvertFromInvariantString(param)
						: typedParam = Convert.ChangeType(param, type);
				}
				catch (Exception)
				{
					return false;
				}
			}
			else
			{
				if (type.IsValueType || !methodParam.IsNullable())
				{
					return false;
				}

				typedParam = type.IsValueType ? Activator.CreateInstance(type) : null;
			}

			if (typedParam == null && !methodParam.IsNullable())
			{
				return false;
			}

			paramsValues.Add(typedParam);
		}

		return true;
	}

	private void GetRawParameters(string path, string[]? splittedTemplate, Dictionary<string, string> pathParameters)
	{
		if (splittedTemplate != null && splittedTemplate.Length > 0)
		{
			string[] splittedPath = path.Split(_pathSeparators, StringSplitOptions.RemoveEmptyEntries);

			for (int i = 0; i < splittedTemplate.Length; i++)
			{
				if (splittedTemplate[i].StartsWith("{"))
				{
					pathParameters.Add(splittedTemplate[i][1..^1], splittedPath[i]);
				}
			}
		}

		foreach (KeyValuePair<string, StringValues> query in Request.Query)
		{
			pathParameters.Add(query.Key, query.Value);
		}
	}

	private async Task<object?> GetResultAsync(object? methodResult)
	{
		switch (methodResult)
		{
			case Task awaitable:
				await awaitable.ConfigureAwait(false);
				awaitable.GetAwaiter().GetResult();
				if (!typeof(Task).Equals(awaitable.GetType()))
				{
					return await GetResultAsync(methodResult.GetType().GetProperty(nameof(Task<object>.Result))?.GetValue(methodResult));
				}
				else
				{
					return new OkResult();
				}

			case IActionResult actionResult:
				return actionResult;

			default:
				return methodResult == null ? new OkResult() as IActionResult : new OkObjectResult(methodResult);
		}
	}

	private bool IsMethodPathCorrect(HttpMethodAttribute? attribute, string path, ParameterInfo[] parameters)
	{
		string[] splittedPath = path.Split(_pathSeparators, StringSplitOptions.RemoveEmptyEntries);
		string[] splittedTemplate = attribute?.Template?.Split(_pathSeparators, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();

		return path == attribute?.Template ||
			splittedPath.Length == splittedTemplate.Length &&
			IsTemplateCorrect(splittedPath, splittedTemplate, parameters);
	}

	private bool IsTemplateCorrect(string[] splittedPath, string[] splittedTemplate, ParameterInfo[] parameters)
	{
		for (int i = 0; i < splittedTemplate.Length; i++)
		{
			string templateItem = splittedTemplate[i];

			if (templateItem.ToLower() != splittedPath[i].ToLower() && !templateItem.StartsWith(ParameterStarter))
			{
				return false;
			}

			if (templateItem.StartsWith(ParameterStarter))
			{
				ParameterInfo? param = parameters.FirstOrDefault(p => p.Name == templateItem[1..^1]);

				try
				{
					bool testConvert = param != null && (param.ParameterType == typeof(Guid) ?
						TypeDescriptor.GetConverter(typeof(Guid)).ConvertFromInvariantString(splittedPath[i])!.ToString()?.ToLower() == splittedPath[i].ToLower()
						: Convert.ChangeType(splittedPath[i], param.ParameterType).ToString() == splittedPath[i]);
				}
				catch (Exception)
				{
					return false;
				}
			}
		}

		return true;
	}

	private string? GetCurrentUser(string? token)
	{
		const string claimTypeNameAccessToken = "upn";
		const string claimTypeNameAccessToken2 = "unique_name";
		const string claimTypeNameIdToken = "preferred_username";

		string? username = null;

		if (!string.IsNullOrWhiteSpace(token))
		{
			try
			{
				var jwtToken = new JwtSecurityToken(token);
				username = jwtToken.Claims.Where(claim => (
							claim.Type == claimTypeNameAccessToken
							|| claim.Type == claimTypeNameAccessToken2
							|| claim.Type == claimTypeNameIdToken)
							&& string.IsNullOrWhiteSpace(claim.Value) == false)
					.Select(x => x.Value)
					.FirstOrDefault();

				if (string.IsNullOrWhiteSpace(username))
				{
					_logger.LogError($"Username is not present in claims! Claims:{jwtToken.Claims.Select(x => x.ToString()).SerializeToJson()}");
				}
			}
			catch { }
		}

		return username;
	}

	private (string? idToken, string? accessToken) GetAadTokens()
	{
		const string aadHeaderIdTokenKeyName = "X-MS-TOKEN-AAD-ID-TOKEN";
		const string aadHeaderAccessTokenKeyName = "X-MS-TOKEN-AAD-ACCESS-TOKEN";

		string? idTokenFromHeader = Request?.Headers?[aadHeaderIdTokenKeyName].ToString();
		string? accessTokenFromHeader = Request?.Headers?[aadHeaderAccessTokenKeyName].ToString();

		if (string.IsNullOrWhiteSpace(idTokenFromHeader) && string.IsNullOrWhiteSpace(accessTokenFromHeader))
		{
			_logger.LogError($"Tokens are not present! Headers:{Request?.Headers?.SerializeToJson()} | Cookies {Request?.Cookies?.SerializeToJson()}");
		}

		return (idTokenFromHeader, accessTokenFromHeader);
	}

	private class MethodData
	{
		public MethodInfo Method { get; set; }
		public HttpMethodAttribute? Attribute { get; set; }
		public ParameterInfo[] Parameters { get; set; }
		public IList<object?> Params { get; set; } = new List<object?>();
		public int NotFitted { get; set; }
		public IEnumerable<Attribute> MethodAttributes { get; set; }

		public MethodData(MethodInfo method)
		{
			Method = method;
			Attribute = method.GetCustomAttribute<HttpMethodAttribute>();
			Parameters = method.GetParameters();
			MethodAttributes = method.GetCustomAttributes().Where(att => att.GetType() != typeof(HttpMethodAttribute));
		}

		public async Task ValidateAccess(IDispatcher dispatcher, string userName)
		{
			OnlyForRolesAttribute? roleAttribute = (OnlyForRolesAttribute?) MethodAttributes.FirstOrDefault(att => att.GetType() == typeof(OnlyForRolesAttribute));
			if (roleAttribute != null)
			{
				await roleAttribute.Validate(dispatcher, userName);
			}
		}
	}
}
