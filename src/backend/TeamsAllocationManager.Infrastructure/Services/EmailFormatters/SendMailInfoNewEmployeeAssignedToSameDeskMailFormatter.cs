namespace TeamsAllocationManager.Infrastructure.Services.EmailFormatters;

public class SendMailInfoNewEmployeeAssignedToSameDeskMailFormatter : IMailMessageFormatter<SendMailInfoNewEmployeeAssignedToSameDeskMailFormatter>
{
	public string FormatBody(params object[] arguments) =>
		$"Od teraz inny pracownik b�dzie razem z Tob� dzieli� biurko nr {arguments[0]} w pokoju {arguments[1]}-{arguments[2]}." +
		"<br/> <br/> Szczeg�y mo�esz sprawdzi� w aplikacji lub u swojego lidera.";

	public string FormatTitle(params object[] arguments) =>
		$"Nowy wsp�u�ytkownik biurka nr {arguments[0]} w {arguments[1]}-{arguments[2]}";
}
