using System;

namespace TeamsAllocationManager.Infrastructure.Exceptions;

public class DeskReservationException : ExceptionBase
{
	public DeskReservationException(string message) : base(message) { }

	public override int Code => 5;
	public override string Status => "desk_reservation_failed";
}
