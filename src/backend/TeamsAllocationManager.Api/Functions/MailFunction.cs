using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TeamsAllocationManager.Contracts.Base;
using TeamsAllocationManager.Contracts.Employee.Commands;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Infrastructure.Services.Interfaces;
using TeamsAllocationManager.Dtos.Common;
using TeamsAllocationManager.Infrastructure.Options;

namespace TeamsAllocationManager.Api.Functions;

public class MailFunction : FunctionBase
{
	private readonly IMailSenderService _mail;
	private readonly EnvironmentSettings _environmentSettings;

	public MailFunction(IDispatcher dispatcher, IMailSenderService mail, IOptions<EnvironmentSettings> options) : base(dispatcher)
	{
		_mail = mail;
		_environmentSettings = options.Value;
	}

	[FunctionName("MailFunction")]
	public override async Task<IActionResult> RunAsync(
		[HttpTrigger(AuthorizationLevel.Anonymous, Route = "Mail/{*path}")]
		HttpRequest req, string? path, ILogger log)
		=> await base.RunAsync(req, path, log);

	[HttpPost("SendMailsToSelf")]
	public async Task SendMailsToSelf(MailDto[] mails)
	{
		mails.ToList().ForEach(m => m.Recipients = new[] { CurrentUsername });
		await _mail.SendMails(mails);
	}

	[HttpPost("SendFunctionMailsToSelf")]
	[OnlyForRoles(RoleEntity.Admin, RoleEntity.TeamLeader)]
	public async Task SendFunctionMailsToSelf()
		=> await _dispatcher.DispatchAsync(new SendMailReminderWorkTypeDeskCommand(CurrentUsername));

	[FunctionName("NewUsersFromLastMonthFunction")]
	public async Task NewUsersFromLastMonthFunction([TimerTrigger("0 0 0 1 * *")] TimerInfo timer, ILogger logger)
    {
	    using (logger.BeginScope(new Dictionary<string, object>
	            {
		            ["AppVersion"] = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "-",
		            ["Action"] = "NewUsersFromLastMonthFunction",
	            }))
	    {
		    try
		    {
			    logger.LogInformation("Sending mails about new Employees from last month started");

				if (_environmentSettings.EnvironmentType == EnvironmentType.Production)
				{
					await _dispatcher.DispatchAsync(new SendMailReminderWorkTypeDeskCommand());
				}
				else
				{
					logger.LogInformation($"Sending aborted due to environment being different than {nameof(EnvironmentType.Production)}");
				}
			}
			catch (Exception ex)
		    {
			    LogException(logger, ex);
			    throw;
		    }
	    }
    }
}
