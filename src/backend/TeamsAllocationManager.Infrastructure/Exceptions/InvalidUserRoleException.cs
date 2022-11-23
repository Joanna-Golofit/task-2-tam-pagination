using System.Net;

namespace TeamsAllocationManager.Infrastructure.Exceptions;

public class InvalidUserRoleException : ExceptionBase
{
	public InvalidUserRoleException() : base("Invalid user role") { }
		
	public InvalidUserRoleException(string message) : base(message) { }

	public override int Code => 7;
	public override string Status => "invalid_user_role";
	public override HttpStatusCode StatusCode => HttpStatusCode.Forbidden;
}

