using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Employee.Commands;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Enums;
using TeamsAllocationManager.Dtos.Common;
using TeamsAllocationManager.Infrastructure.Services.Interfaces;

namespace TeamsAllocationManager.Infrastructure.Handlers.Employee;

public class SendMailReminderWorkTypeDeskHandler : IAsyncCommandHandler<SendMailReminderWorkTypeDeskCommand>
{
	private readonly IProjectRepository _projectRepository;
	private readonly IMailSenderService _mailSenderService;

	private readonly string _employeesWithoutWorkTypeMailTemplate =
		"Poni¿ej znajduje siê lista pracowników bez podanego trybu pracy (zdalny/hybrydowy/stacjonarny).\n{0}";
	private readonly string _employeesWithoutDesksMailTemplate =
		"Poni¿ej znajduje siê lista pracowników hybrydowych oraz zdalnych bez przypisanych biurek.\n{0}";

	public SendMailReminderWorkTypeDeskHandler(IProjectRepository projectRepository, IMailSenderService mailSenderService)
	{
		_projectRepository = projectRepository;
		_mailSenderService = mailSenderService;
	}

	public async Task HandleAsync(SendMailReminderWorkTypeDeskCommand command, CancellationToken cancellationToken = default)
	{
		List<MailDto> mails = (await GetMailsAboutEmployeesWithoutDesk(command.MailAddress)).ToList();
		mails.AddRange(await GetMailsAboutEmployeesWithoutWorkType(command.MailAddress));

		await _mailSenderService.SendMails(mails);
	}

	private async Task<IEnumerable<MailDto>> GetMailsAboutEmployeesWithoutWorkType(IEnumerable<string>? mailAddress)
	{
		var mails = new List<MailDto>();

		var projectsWithEmployees = await _projectRepository.GetProjectsWithEmployeesAssigned();

		var employeesWithNoWorkType = projectsWithEmployees
			.Select(p => new
			{
				ProjectName = p.Name,
				TeamLeaders = p.Employees
				               .Where(ep => ep.IsTeamLeaderProjectRole)
				               .Select(ep => ep.Employee.Email),
				Employees = p.Employees
				             .Where(ep => ep.Employee.WorkspaceType == null)
				             .Select(ep => $"{ep.Employee.Name} {ep.Employee.Surname}")
			})
			.Where(e => e.Employees.Any());
			
		foreach (var dto in employeesWithNoWorkType)
		{
			mails.Add(new MailDto
			{
				Recipients = mailAddress ?? dto.TeamLeaders,
				Subject = $"{dto.ProjectName} - pracownicy bez ustawionego trybu pracy",
				Body = string.Format(_employeesWithoutWorkTypeMailTemplate, string.Join(",\n", dto.Employees.Select(e => $" - {e}")))
			});
		}

		return mails;
	}

	private async Task<IEnumerable<MailDto>> GetMailsAboutEmployeesWithoutDesk(IEnumerable<string>? mailAddress)
	{
		var mails = new List<MailDto>();

		var projectsWithEmployees = await _projectRepository.GetProjectsWithEmployeesAssigned();

		var employeesWithNoDesks = projectsWithEmployees
			.Select(p => new
			{
				ProjectName = p.Name,
				TeamLeaders = p.Employees
				               .Where(ep => ep.IsTeamLeaderProjectRole)
				               .Select(ep => ep.Employee.Email),
				Employees = p.Employees
				             .Where(ep =>
					             ep.Employee.WorkspaceType != WorkspaceType.Hybrid &&
					             !ep.Employee.EmployeeDeskReservations.Any(dr => dr.IsSchedule) &&
					             ep.Employee.IsExternal)
				             .Select(ep => $"{ep.Employee.Name} {ep.Employee.Surname}")
			})
			.Where(e => e.Employees.Any());

		foreach (var dto in employeesWithNoDesks)
		{
			mails.Add(new MailDto
			{
				Recipients = mailAddress ?? dto.TeamLeaders,
				Subject = $"{dto.ProjectName} - pracownicy bez ustawionego miejsca pracy",
				Body = string.Format(_employeesWithoutDesksMailTemplate, string.Join(",\n", dto.Employees.Select(e => $" - {e}")))
			});
		}

		return mails;
	}
}
