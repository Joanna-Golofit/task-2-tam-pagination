using System;

namespace TeamsAllocationManager.Infrastructure.Services.EmailFormatters;

public class HotDeskRemovedMailFormatter : IMailMessageFormatter<HotDeskRemovedMailFormatter>
{
	public string FormatTitle(params object[] arguments) => "Hot desk - anulowano rezerwacjê";

	public string FormatBody(params object[] arguments) =>
		$@"Rezerwacja biurka numer {arguments[0]} w pokoju {arguments[1]}-{arguments[2]} 
			{FormatDateString((DateTime)arguments[3], (DateTime)arguments[4])} zosta³a anulowana, 
			poniewa¿ biurko przesta³o funkcjonowaæ jako Hot Desk. Mo¿esz zarezerwowaæ inny Hot Desk.";

	private string FormatDateString(DateTime startDate, DateTime endDate)
		=> startDate.Date.AddDays(1) < endDate.Date
			? $"w dniach {startDate.ToShortDateString()} - {endDate.ToShortDateString()}"
			: $"na dzieñ {startDate.ToShortDateString()}";
}
