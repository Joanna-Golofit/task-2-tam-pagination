using TeamsAllocationManager.Contracts.Base.Commands;

namespace TeamsAllocationManager.Contracts.Employee.Commands;

public class SendMailReminderWorkTypeDeskCommand : ICommand
{
	public string[]? MailAddress { get; } = null;

	public SendMailReminderWorkTypeDeskCommand() { }

	public SendMailReminderWorkTypeDeskCommand(string mailAddress)
	{
		MailAddress = new [] { mailAddress };
	}
}
