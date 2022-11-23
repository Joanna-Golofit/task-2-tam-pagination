namespace TeamsAllocationManager.Infrastructure.Services.EmailFormatters;

public class SendMailInfoNewEmployeeAssignedToSameDeskMailFormatter : IMailMessageFormatter<SendMailInfoNewEmployeeAssignedToSameDeskMailFormatter>
{
	public string FormatBody(params object[] arguments) =>
		$"Od teraz inny pracownik bêdzie razem z Tob¹ dzieliæ biurko nr {arguments[0]} w pokoju {arguments[1]}-{arguments[2]}." +
		"<br/> <br/> Szczegó³y mo¿esz sprawdziæ w aplikacji lub u swojego lidera.";

	public string FormatTitle(params object[] arguments) =>
		$"Nowy wspó³u¿ytkownik biurka nr {arguments[0]} w {arguments[1]}-{arguments[2]}";
}
