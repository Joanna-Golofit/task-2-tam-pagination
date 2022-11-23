using System;

namespace TeamsAllocationManager.Infrastructure.Services.EmailFormatters;

public class HotDeskReservationConfirmationMailFormatter : IMailMessageFormatter<HotDeskReservationConfirmationMailFormatter>
{
	public string FormatTitle(params object[] arguments) => "Hot desk - potwierdzenie rezerwacji";

	public string FormatBody(params object[] arguments) => 
		$@"Biurko numer {arguments[0]} w pokoju {arguments[1]}-{arguments[2]} zosta�o zarezerwowane 
			{FormatDateString((DateTime)arguments[3], (DateTime)arguments[4])}. Je�li nie planujesz 
			wizyty w biurze - anuluj rezerwacj�.";

	private string FormatDateString(DateTime startDate, DateTime endDate)
		=> startDate.Date == endDate.Date
			? $"na dzie� {startDate.ToShortDateString()}"
			: $"w dniach {startDate.ToShortDateString()} - {endDate.ToShortDateString()}";
}
