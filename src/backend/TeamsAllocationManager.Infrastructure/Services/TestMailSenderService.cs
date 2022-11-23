using Microsoft.Extensions.Options;
using Microsoft.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamsAllocationManager.Dtos.Common;
using TeamsAllocationManager.Infrastructure.Options;

namespace TeamsAllocationManager.Infrastructure.Services;

public class TestMailSenderService : MailSenderService
{
	private readonly EnvironmentSettings _environmentSettings;

	public TestMailSenderService(
		IOptions<AzureAdSettings> options,
		GraphServiceClient graphClient,
		IOptions<EnvironmentSettings> envOptions)
		: base(options, graphClient, envOptions)
	{
		_environmentSettings = envOptions.Value;
	}

	public override async Task SendMail(MailDto mailDto)
	{
		mailDto.Body += $" <br /> Osoby do których powinnien zostaæ wys³any mail: {string.Join(", ", mailDto.Recipients)}.";
		mailDto.Body += mailDto.Cc != null ? $" <br /> Oraz CC do: {string.Join(", ", mailDto.Cc)}." : string.Empty;

		mailDto.Recipients = new List<string> { _environmentSettings.TestMailAddress };
		mailDto.Cc = mailDto.Cc != null ? new string[] { _environmentSettings.TestMailAddress } : mailDto.Cc;

		await base.SendMail(mailDto);
	}
}

