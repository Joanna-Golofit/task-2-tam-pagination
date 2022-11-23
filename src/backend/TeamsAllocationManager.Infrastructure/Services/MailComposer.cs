using System.Collections.Generic;
using System.Linq;
using TeamsAllocationManager.Dtos.Common;
using TeamsAllocationManager.Infrastructure.Services.EmailFormatters;
using TeamsAllocationManager.Infrastructure.Services.Interfaces;

namespace TeamsAllocationManager.Infrastructure.Services;

public class MailComposer<TFormatter> : IMailComposer<TFormatter>
{
	private readonly IMailMessageFormatter<TFormatter> _formatter;

	public MailComposer(IMailMessageFormatter<TFormatter> formatter)
	{
		_formatter = formatter;
	}

	public IEnumerable<MailDto> Compose(string recipient, string[]? cc, IEnumerable<object> mailArguments) =>
		new[] 
		{ 
			new MailDto {
				Recipients = new string [] {recipient},
				Cc = cc,
				Subject = _formatter.FormatTitle(mailArguments.ToArray()),
				Body = _formatter.FormatBody(mailArguments.ToArray()),
			}
		};
	public IEnumerable<MailDto> Compose(string[] recipients, string[]? cc, IEnumerable<object> mailArguments) =>
		new[]
		{
			new MailDto {
				Recipients = recipients,
				Cc = cc,
				Subject = _formatter.FormatTitle(mailArguments.ToArray()),
				Body = _formatter.FormatBody(mailArguments.ToArray()),
			}
		};
}
