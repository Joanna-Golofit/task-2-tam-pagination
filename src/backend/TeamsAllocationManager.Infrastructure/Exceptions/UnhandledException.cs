using System.Net;

namespace TeamsAllocationManager.Infrastructure.Exceptions;

public class UnhandledException : ExceptionBase
{
	public UnhandledException() : base("Unhandled exception") { }
	public UnhandledException(string message) : base(message) { }

	public override int Code => -1;
	public override string Status => "unhandled_exception";
	public override HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;
}
