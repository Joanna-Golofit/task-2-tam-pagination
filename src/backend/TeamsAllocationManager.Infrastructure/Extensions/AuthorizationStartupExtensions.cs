using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace TeamsAllocationManager.Infrastructure.Extensions;

public static class AuthorizationStartupExtensions
{
	// ARM Authentication
    private const string ArmAuthenticationExtensions = "Microsoft.Extensions.DependencyInjection.ArmAuthenticationExtensions";
    private static Type? _armAuthenticationExtensionsTypes = null;
    private static Type? ArmAuthenticationExtensionsType
    {
        get
        {
            if (_armAuthenticationExtensionsTypes is null)
            {
                _armAuthenticationExtensionsTypes = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .FirstOrDefault(x => x.IsClass && x.FullName == ArmAuthenticationExtensions);
            }
            return _armAuthenticationExtensionsTypes;
        }
    }

    public static AuthenticationBuilder AddArmToken(this AuthenticationBuilder builder)
    {
		MethodInfo? method = ArmAuthenticationExtensionsType?.GetMethods()
            .Where(x => x.Name == nameof(AddArmToken))
            .FirstOrDefault(x => x.GetParameters().Length == 1);
        return (method?.Invoke(null, new object[] { builder }) as AuthenticationBuilder)!;
    }

	// Script Authentication Level
    private const string AuthLevelExtensionsExtensions = "Microsoft.Extensions.DependencyInjection.AuthLevelExtensions";
    private static Type? _authLevelExtensionsExtensionsTypes = null;
    private static Type? AuthLevelExtensionsExtensionsType
    {
        get
        {
            if (_authLevelExtensionsExtensionsTypes is null)
            {
                _authLevelExtensionsExtensionsTypes = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .FirstOrDefault(x => x.IsClass && x.FullName == AuthLevelExtensionsExtensions);
            }
            return _authLevelExtensionsExtensionsTypes;
        }
    }

    public static AuthenticationBuilder AddScriptAuthLevel(this AuthenticationBuilder builder)
    {
		MethodInfo? method = AuthLevelExtensionsExtensionsType?.GetMethods()
            .Where(x => x.Name == nameof(AddScriptAuthLevel))
            .FirstOrDefault(x => x.GetParameters().Length == 1);
        return (method?.Invoke(null, new object[] { builder }) as AuthenticationBuilder)!;
    }
	// JWT Bearer

	private const string JwtBearerExtensions = "Microsoft.Extensions.DependencyInjection.ScriptJwtBearerExtensions";
	private static Type? _jwtBearerExtensionsTypes = null;
	private static Type? JwtBearerExtensionsType
	{
		get
		{
			if (_jwtBearerExtensionsTypes is null)
			{
				_jwtBearerExtensionsTypes = AppDomain.CurrentDomain
					.GetAssemblies()
					.SelectMany(x => x.GetTypes())
					.FirstOrDefault(x => x.IsClass && x.FullName == JwtBearerExtensions);
			}
			return _jwtBearerExtensionsTypes;
		}
	}

	public static AuthenticationBuilder AddScriptJwtBearer(this AuthenticationBuilder builder)
	{
		MethodInfo? method = JwtBearerExtensionsType?.GetMethods()
			.Where(x => x.Name == nameof(AddScriptJwtBearer))
			.FirstOrDefault(x => x.GetParameters().Length == 1);
		return (method?.Invoke(null, new object[] { builder }) as AuthenticationBuilder)!;
	}
	private const string AuthorizationOptionsExtensions = "Microsoft.Azure.WebJobs.Script.WebHost.Security.Authorization.Policies.AuthorizationOptionsExtensions";
	private static Type? _authorizationOptionsExtensionsType = null;
	private static Type? AuthorizationOptionsExtensionsType
	{
		get
		{
			if (_authorizationOptionsExtensionsType is null)
			{
				_authorizationOptionsExtensionsType = AppDomain.CurrentDomain
					.GetAssemblies()
					.SelectMany(t => t.GetTypes())
					.FirstOrDefault(t => t.IsClass && t.FullName == AuthorizationOptionsExtensions);
			}
			return _authorizationOptionsExtensionsType;
		}
	}

	public static void AddScriptPolicies(this AuthorizationOptions options)
	{
		MethodInfo? method = AuthorizationOptionsExtensionsType?.GetMethods()
			.Where(x => x.Name == nameof(AddScriptPolicies))
			.FirstOrDefault(x => x.GetParameters().Length == 1);
		method?.Invoke(null, new object[] { options });
	}

	private const string AuthLevelAuthorizationHandler = "Microsoft.Azure.WebJobs.Script.WebHost.Security.Authorization.AuthLevelAuthorizationHandler";
	private static Type? _authLevelAuthorizationHandlerType = null;
	private static Type? AuthLevelAuthorizationHandlerType
	{
		get
		{
			if (_authLevelAuthorizationHandlerType is null)
			{
				_authLevelAuthorizationHandlerType = AppDomain.CurrentDomain
					.GetAssemblies()
					.SelectMany(t => t.GetTypes())
					.FirstOrDefault(t => t.IsClass && t.FullName == AuthLevelAuthorizationHandler);
			}
			return _authLevelAuthorizationHandlerType;
		}
	}

	public static IServiceCollection AddAuthLevelAuthorizationHandler(this IServiceCollection services)
	{
		MethodInfo? method = typeof(ServiceCollectionServiceExtensions).GetMethods()
			.Where(x => x.Name == nameof(ServiceCollectionServiceExtensions.AddSingleton))
			.FirstOrDefault(x => x.GetParameters().Length == 1);
		MethodInfo? genericMethod = method?.MakeGenericMethod(typeof(IAuthorizationHandler), AuthLevelAuthorizationHandlerType!);
		return (genericMethod?.Invoke(null, new object[] { services }) as IServiceCollection)!;
	}

	private const string NamedAuthLevelAuthorizationHandler = "Microsoft.Azure.WebJobs.Script.WebHost.Security.Authorization.NamedAuthLevelAuthorizationHandler";
	private static Type? _namedAuthLevelAuthorizationHandlerType = null;
	private static Type? NamedAuthLevelAuthorizationHandlerType
	{
		get
		{
			if (_namedAuthLevelAuthorizationHandlerType is null)
			{
				_namedAuthLevelAuthorizationHandlerType = AppDomain.CurrentDomain
					.GetAssemblies()
					.SelectMany(t => t.GetTypes())
					.FirstOrDefault(t => t.IsClass && t.FullName == NamedAuthLevelAuthorizationHandler);
			}
			return _namedAuthLevelAuthorizationHandlerType;
		}
	}

	public static IServiceCollection AddNamedAuthLevelAuthorizationHandler(this IServiceCollection services)
	{
		MethodInfo? method = typeof(ServiceCollectionServiceExtensions).GetMethods()
			.Where(x => x.Name == nameof(ServiceCollectionServiceExtensions.AddSingleton))
			.FirstOrDefault(x => x.GetParameters().Length == 1);
		MethodInfo? genericMethod = method?.MakeGenericMethod(typeof(IAuthorizationHandler), NamedAuthLevelAuthorizationHandlerType!);
		return (genericMethod?.Invoke(null, new object[] { services }) as IServiceCollection)!;
	}

	private const string FunctionAuthorizationHandler = "Microsoft.Azure.WebJobs.Script.WebHost.Security.Authorization.FunctionAuthorizationHandler";
	private static Type? _functionAuthorizationHandlerType = null;
	private static Type? FunctionAuthorizationHandlerType
	{
		get
		{
			if (_functionAuthorizationHandlerType is null)
			{
				_functionAuthorizationHandlerType = AppDomain.CurrentDomain
					.GetAssemblies()
					.SelectMany(t => t.GetTypes())
					.FirstOrDefault(t => t.IsClass && t.FullName == FunctionAuthorizationHandler);
			}
			return _functionAuthorizationHandlerType;
		}
	}

	public static IServiceCollection AddFunctionAuthorizationHandler(this IServiceCollection services)
	{
		MethodInfo? method = typeof(ServiceCollectionServiceExtensions).GetMethods()
			.Where(x => x.Name == nameof(ServiceCollectionServiceExtensions.AddSingleton))
			.FirstOrDefault(x => x.GetParameters().Length == 1);
		MethodInfo genericMethod = method!.MakeGenericMethod(typeof(IAuthorizationHandler), FunctionAuthorizationHandlerType!);
		return (genericMethod.Invoke(null, new object[] { services }) as IServiceCollection)!;
	}
}
