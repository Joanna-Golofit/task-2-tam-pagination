using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using TeamsAllocationManager.Dtos.Common;
using TeamsAllocationManager.Infrastructure.Options;
using TeamsAllocationManager.Infrastructure.Services.Interfaces;

namespace TeamsAllocationManager.Infrastructure.Services;

public class MailSenderService : IMailSenderService
{
	private readonly AzureAdSettings _options;
	private readonly GraphServiceClient _graphClient;

	private readonly string _mailFooter =
		"<br><br>Aplikacja Team Allocation Manager dostêpna jest pod <a href=\"{0}\">tym adresem</a>.";

	public MailSenderService(IOptions<AzureAdSettings> adOptions, GraphServiceClient graphClient, IOptions<EnvironmentSettings> envOptions)
	{
		_options = adOptions.Value;
		_graphClient = graphClient;
		_mailFooter = string.Format(_mailFooter, envOptions.Value.AppUrl);
	}

	public virtual async Task SendMail(MailDto mailDto)
	{
		await _graphClient.Users[_options.UserObjectId].SendMail(CreatMailFromMailDto(mailDto)).Request().PostAsync(CancellationToken.None);
	}

	public async Task SendMails(IEnumerable<MailDto> mails)
	{
		foreach (var mail in mails)
		{
			await SendMail(mail);
		}
	}

	private IEnumerable<Recipient> CreateRecipients(IEnumerable<string>? emails)
	{
		if (emails == null)
		{
			return new Recipient[] { };
		}

		return emails.Select(email =>
			new Recipient()
			{
				EmailAddress = new EmailAddress { Address = email }
			});
	}

	private Message CreatMailFromMailDto(MailDto mail)
	{
		return new Message
						{
				Subject = mail.Subject,
				Body = new ItemBody
				{
					ContentType = BodyType.Html,
					Content = $"{mail.Body} {_mailFooter}"
				},
				ToRecipients = CreateRecipients(mail.Recipients.Distinct()),
				CcRecipients = CreateRecipients(mail.Cc?.Distinct())
		};
	}
};
