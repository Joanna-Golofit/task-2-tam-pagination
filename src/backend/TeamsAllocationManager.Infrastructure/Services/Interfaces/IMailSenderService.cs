using System.Collections.Generic;
using System.Threading.Tasks;
using TeamsAllocationManager.Dtos.Common;

namespace TeamsAllocationManager.Infrastructure.Services.Interfaces;

public interface IMailSenderService
{
	Task SendMails(IEnumerable<MailDto> mails);
	Task SendMail(MailDto mailDto);
}
