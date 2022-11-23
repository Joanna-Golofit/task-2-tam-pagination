using System;
using System.Net;

namespace TeamsAllocationManager.Infrastructure.Exceptions;

public class ExternalApiException : ExceptionBase
{
	private readonly string _api;

	public ExternalApiException(string api, Exception e) : base(e.Message)
	{
		_api = api;
	}

	public ExternalApiException(string api, string message) : base(message)
	{
		_api = api;
	}

	public override int Code => 3;
	public override string Status => "unhandled_exception";
	public override HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;
}
