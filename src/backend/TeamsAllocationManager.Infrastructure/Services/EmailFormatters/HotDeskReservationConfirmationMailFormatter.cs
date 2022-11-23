using System;

namespace TeamsAllocationManager.Infrastructure.Services.EmailFormatters;

public class HotDeskReservationConfirmationMailFormatter : IMailMessageFormatter<HotDeskReservationConfirmationMailFormatter>
{
	public string FormatTitle(params object[] arguments) => "Hot desk - potwierdzenie rezerwacji";

	public string FormatBody(params object[] arguments) => 
		$@"Biurko numer {arguments[0]} w pokoju {arguments[1]}-{arguments[2]} zosta³o zarezerwowane 
			{FormatDateString((DateTime)arguments[3], (DateTime)arguments[4])}. Jeœli nie planujesz 
			wizyty w biurze - anuluj rezerwacjê.";

	private string FormatDateString(DateTime startDate, DateTime endDate)
		=> startDate.Date == endDate.Date
			? $"na dzieñ {startDate.ToShortDateString()}"
			: $"w dniach {startDate.ToShortDateString()} - {endDate.ToShortDateString()}";
}
