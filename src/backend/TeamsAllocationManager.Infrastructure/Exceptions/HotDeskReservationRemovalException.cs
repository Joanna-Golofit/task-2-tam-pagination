namespace TeamsAllocationManager.Infrastructure.Exceptions;

public class HotDeskReservationRemovalException : ExceptionBase
{
	public HotDeskReservationRemovalException(string message) : base(message) { }

	public override int Code => 6;
	public override string Status => "hotdesk_reservation_removal_failed";
}
