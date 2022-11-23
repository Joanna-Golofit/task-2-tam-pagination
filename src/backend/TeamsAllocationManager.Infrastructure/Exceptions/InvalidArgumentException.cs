using System.Net;

namespace TeamsAllocationManager.Infrastructure.Exceptions;

public class InvalidArgumentException : ExceptionBase
{
	public InvalidArgumentException(string message) : base(message) { }

	public override int Code => 4;
	public override string Status => "invalid_argument_exception";
	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
