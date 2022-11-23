namespace TeamsAllocationManager.Infrastructure.Exceptions;

public class DeskException : ExceptionBase
{
	public DeskException(string message) : base(message) { }

	public override int Code => 8;
	public override string Status => "desk_exception";
}
