namespace TeamsAllocationManager.Infrastructure.Services.EmailFormatters;

public class HotDeskReservationReminderMailFormatter : IMailMessageFormatter<HotDeskReservationReminderMailFormatter>
{
	public string FormatTitle(params object[] arguments) => "Hot desk - przypomnienie";

	public string FormatBody(params object[] arguments) => 
		$"Przypominamy, ¿e biurko numer {arguments[0]} w pokoju {arguments[1]}-{arguments[2]} zosta³o zarezerwowane na dzieñ {arguments[3]}. Jeœli nie planujesz wizyty w biurze - anuluj rezerwacjê.";
}
