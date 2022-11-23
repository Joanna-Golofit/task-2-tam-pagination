namespace TeamsAllocationManager.Infrastructure.Exceptions
{
	public class HotDeskGetReservationException : ExceptionBase
	{
		public HotDeskGetReservationException(string message) : base(message) { }

		public override int Code => 9;
		public override string Status => "get_desk_reservation_failed";
	}
}
