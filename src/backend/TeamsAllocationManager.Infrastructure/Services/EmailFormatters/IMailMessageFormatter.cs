namespace TeamsAllocationManager.Infrastructure.Services.EmailFormatters;

public interface IMailMessageFormatter<TFormatter>
{
	string FormatTitle(params object[] arguments);

	string FormatBody(params object[] arguments);
}
