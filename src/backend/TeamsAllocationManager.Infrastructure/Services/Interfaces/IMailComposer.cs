using System.Collections.Generic;
using TeamsAllocationManager.Dtos.Common;

namespace TeamsAllocationManager.Infrastructure.Services.Interfaces;

public interface IMailComposer<TFormatter>
{
	IEnumerable<MailDto> Compose(string recipient, string[]? cc, IEnumerable<object> mailArguments);
	IEnumerable<MailDto> Compose(string[] recipients, string[]? cc, IEnumerable<object> mailArguments);
}
