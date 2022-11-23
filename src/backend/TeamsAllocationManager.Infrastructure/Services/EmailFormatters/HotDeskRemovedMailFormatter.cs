using System;

namespace TeamsAllocationManager.Infrastructure.Services.EmailFormatters;

public class HotDeskRemovedMailFormatter : IMailMessageFormatter<HotDeskRemovedMailFormatter>
{
	public string FormatTitle(params object[] arguments) => "Hot desk - anulowano rezerwacj�";

	public string FormatBody(params object[] arguments) =>
		$@"Rezerwacja biurka numer {arguments[0]} w pokoju {arguments[1]}-{arguments[2]} 
			{FormatDateString((DateTime)arguments[3], (DateTime)arguments[4])} zosta�a anulowana, 
			poniewa� biurko przesta�o funkcjonowa� jako Hot Desk. Mo�esz zarezerwowa� inny Hot Desk.";

	private string FormatDateString(DateTime startDate, DateTime endDate)
		=> startDate.Date.AddDays(1) < endDate.Date
			? $"w dniach {startDate.ToShortDateString()} - {endDate.ToShortDateString()}"
			: $"na dzie� {startDate.ToShortDateString()}";
}
