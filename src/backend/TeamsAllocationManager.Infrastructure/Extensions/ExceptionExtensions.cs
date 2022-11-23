using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using TeamsAllocationManager.Infrastructure.Exceptions;
using TeamsAllocationManager.Integrations.Exceptions;

namespace TeamsAllocationManager.Infrastructure.Extensions;

public static class ExceptionExtensions
{
	public static ObjectResult ToObjectResult(this Exception e)
		=> new ObjectResult(e.SerializeException())
		{
			StatusCode = (int)e.GetStatusCode()
		};

	private static object SerializeException(this Exception exception)
		=> exception is ExceptionBase handledException
			? ConvertToObject(handledException)
			: exception is ApiClientException apiClientException
				? ConvertToObject(new ExternalApiException("FutureDatabase", apiClientException))
				: ConvertToObject(new UnhandledException());

	private static HttpStatusCode GetStatusCode(this Exception exception)
		=> exception is ExceptionBase handledException
			? handledException.StatusCode
			: exception is UnauthorizedAccessException
				? HttpStatusCode.Unauthorized
				: HttpStatusCode.InternalServerError;

	private static object ConvertToObject(ExceptionBase exc)
		=> new 
		{
			Message = exc.Message,
			Code = exc.Code,
			Status = exc.Status.ToLower(),
			TranslationKey = exc.TranslationKey
		};
}
