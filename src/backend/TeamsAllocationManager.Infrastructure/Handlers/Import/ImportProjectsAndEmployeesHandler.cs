using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Import.Commands;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Domain.Enums;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Integrations.FutureDatabase.Clients;
using TeamsAllocationManager.Integrations.FutureDatabase.Enums;
using TeamsAllocationManager.Integrations.FutureDatabase.Models;
using TeamsAllocationManager.Infrastructure.Extensions;
using TeamsAllocationManager.Dtos.Import;
using TeamsAllocationManager.Infrastructure.Options;

namespace TeamsAllocationManager.Infrastructure.Handlers.Import;

public class ImportProjectsAndEmployeesHandler : IAsyncCommandHandler<ImportProjectsAndEmployeesCommand, ImportReportDto>
{
	private readonly ApplicationDbContext _applicationDbContext;
	private readonly IFutureDatabaseApiClient _futureDatabaseApiClient;
	private readonly ILogger<ImportProjectsAndEmployeesHandler> _logger;
	private readonly IMapper _mapper;
	private readonly ImportReportDto _reportDataToLog = new ImportReportDto();
	private readonly AutoImportSettings _autoImportSettings;

	public ImportProjectsAndEmployeesHandler(
		ApplicationDbContext applicationDbContext,
		IFutureDatabaseApiClient futureDatabaseApiClient,
		ILogger<ImportProjectsAndEmployeesHandler> logger,
		IMapper mapper,
		IOptions<AutoImportSettings> autoImportOptions)
	{
		_applicationDbContext = applicationDbContext;
		_autoImportSettings = autoImportOptions.Value;
		_futureDatabaseApiClient = futureDatabaseApiClient;
		_logger = logger;
		_mapper = mapper;
	}

	public async Task<ImportReportDto> HandleAsync(ImportProjectsAndEmployeesCommand command, CancellationToken cancellationToken = default)
	{
		if (command.IsAutoImport == false)
		{
			var isAdmin = await _applicationDbContext.UserRoles
			.Where(ur => ur.Employee.Email == command.CurrentUsername)
			.AnyAsync(x => x.Role.Name == RoleEntity.Admin);

			if (isAdmin == false)
			{
				throw new UnauthorizedAccessException("Admin role is required");
			}
		}

		var employeesFromDb = await _applicationDbContext.Employees.Include(e => e.UserRoles).ToListAsync();
		var projectsFromDb = await _applicationDbContext.Projects.ToListAsync();

		_reportDataToLog.UsersBeforeImportCount = employeesFromDb.Count();
		_reportDataToLog.ProjectsBeforeImportCount = projectsFromDb.Count();

		await ImportEmployeesAsync(employeesFromDb);
		var groups = await ImportProjectsAsync(projectsFromDb);
		await AddProjectAssignments(groups, employeesFromDb, projectsFromDb);

		await RemoveIgnoredProjects();

		await RemoveEmptyProjectsAndEmployeesWithoutProjects();

		_reportDataToLog.UsersAfterImportCount = await _applicationDbContext.Employees.CountAsync();
		_reportDataToLog.ProjectsAfterImportCount = await _applicationDbContext.Projects.CountAsync();

		await SetDivisions(groups);
		await SetAdminRoles();

		_logger.LogInformation("Import executed (IsAutoImport = " + command.IsAutoImport.ToString() + ") {LogData}", _reportDataToLog.SerializeToJson());

		return _reportDataToLog;
	}

	private async Task ImportEmployeesAsync(List<EmployeeEntity> employees)
	{
		ICollection<User>? users = await _futureDatabaseApiClient.GetUsersAsync();

		if (users == null || !users.Any())
		{
			return;
		}

		// ignore improper user data (mainly in PROD)
		users = users.Where(x => x.DomainUserLogin != null && x.DomainUserLogin.Equals(x.Email.Substring(0, x.Email.IndexOf('@')), StringComparison.OrdinalIgnoreCase)).ToList();

		foreach (User user in users)
		{
			EmployeeEntity? employee = employees
				.SingleOrDefault(x => x.ExternalId == user.Id || x.UserLogin.Equals(user.DomainUserLogin, StringComparison.OrdinalIgnoreCase));

			if (employee != null && employee.IsExternal) // the external employees exists in FDB (former FP employees are not deleted) - we need to ignore the records
			{
				continue;
			}

			if (employee != null)
			{
				_mapper.Map(user, employee);
			}
			else
			{
				employee = _mapper.Map<EmployeeEntity>(user);
				_applicationDbContext.Employees.Add(employee);
				employees.Add(employee);
			}

			employee.IsContractor = user.UserTypeId == (int)UserTypeFDB.Contractor;
		}

		await _applicationDbContext.SaveChangesAsync();
	}

	private async Task<IEnumerable<Group>> ImportProjectsAsync(List<ProjectEntity> projects)
	{
		ICollection<Group>? groups = await _futureDatabaseApiClient.GetGroupsAsync();

		if (groups == null || !groups.Any())
		{
			return new List<Group>();
		}

		foreach (Group item in groups)
		{
			ProjectEntity? project = projects.SingleOrDefault(x => x.ExternalId == item.Id);

			if (project != null)
			{
				_mapper.Map(item, project);
			}
			else
			{
				project = _mapper.Map<ProjectEntity>(item);
				_applicationDbContext.Projects.Add(project);
				projects.Add(project);
			}
		}

		await _applicationDbContext.SaveChangesAsync();

		return groups;
	}

	private async Task AddProjectAssignments(IEnumerable<Group> groups, List<EmployeeEntity> employees, List<ProjectEntity> projects)
	{
		var assignments = groups.SelectMany(x => x.Assignments);

		if (assignments == null || !assignments.Any())
		{
			return;
		}

		DateTime currentDate = DateTime.Now.Date;

		_reportDataToLog.RemovedEmployeesFromFutureAssigmentsCount = assignments
									.Where(a => currentDate < a.FromDate)
									.Select(a => new { a.UserId, a.GroupId, a.RoleId })
									.GroupBy(a => new { a.UserId, a.GroupId })
									.Count();

		var activeAssignments = assignments
									.Where(a => (a.ToDate == null || currentDate <= a.ToDate) && currentDate >= a.FromDate)
									.Select(a => new { a.UserId, a.GroupId, a.RoleId })
									.GroupBy(a => new { a.UserId, a.GroupId })
									.Select(g => new
									{
										g.Key.UserId,
										g.Key.GroupId,
										IsLeader = g.Any(x => x.RoleId == (int)RoleFDB.TeamLeader
											|| x.RoleId == (int)RoleFDB.TeamManager
											|| x.RoleId == (int)RoleFDB.ProjectManager)
									})
									.ToList();

		var oldEmployeeProjects = await _applicationDbContext.EmployeeProjects
			.Where(ep => !ep.Employee.IsExternal && !ep.Project.IsExternal)
			.ToListAsync();

		_reportDataToLog.AssignmentsBeforeImportCount = oldEmployeeProjects.Count;

		_applicationDbContext.EmployeeProjects.RemoveRange(oldEmployeeProjects);

		await _applicationDbContext.SaveChangesAsync();

		var projectIds = projects
							.Join(
								activeAssignments.Select(a => a.GroupId).Distinct(), 
								p => p.ExternalId, 
								groupId => groupId, 
								(p, groupId) => new { p.Id, p.ExternalId }
				                )
							.ToList();
		var employeeIds = employees
							.Join(activeAssignments.Select(a => a.UserId).Distinct(), e => e.ExternalId, userId => userId, (e, userId) => new { e.Id, e.ExternalId })
							.ToList();

		var employeeProjectEntitiesWithExternalId = activeAssignments.Select(aa =>
			{
				return new
				{
					ProjectExternalId = aa.GroupId,
					EmployeeProjectEntity = new EmployeeProjectEntity
					{
						EmployeeId = employeeIds.SingleOrDefault(e => e.ExternalId == aa.UserId)?.Id ?? Guid.Empty,
						ProjectId = projectIds.SingleOrDefault(p => p.ExternalId == aa.GroupId)?.Id ?? Guid.Empty,
						IsTeamLeaderProjectRole = aa.IsLeader
					}
				};
			})
			.Where(x => x.EmployeeProjectEntity.EmployeeId != Guid.Empty && x.EmployeeProjectEntity.ProjectId != Guid.Empty)
			.ToList();

		// if no leader then take from parent group if exists
		var projectsWithoutLeader = employeeProjectEntitiesWithExternalId
												.GroupBy(x => x.EmployeeProjectEntity.ProjectId)
												.Select(x => new
												{
													ProjectId = x.Key,
													IsLeader = x.Any(externalEp => externalEp.EmployeeProjectEntity.IsTeamLeaderProjectRole),
													ExternalId = x.First().ProjectExternalId
												})
												.Where(y => y.IsLeader == false)
												.Join(groups, o => o.ExternalId, g => g.Id, (o, g) => new { o.ProjectId, ParentExternalId = g.ParentGroupId })
												.Where(x => x.ParentExternalId.HasValue)
												.Join(projects, o => o.ParentExternalId, p => p.ExternalId, (o, p) => new { o.ProjectId, ParentProjectId = p.Id })
												.ToList();
		var employeeProjectEntities = employeeProjectEntitiesWithExternalId.Select(x => x.EmployeeProjectEntity).ToList();

		foreach (var pwl in projectsWithoutLeader)
		{
			var firstLeaderFromParentAssignment = employeeProjectEntities
											.FirstOrDefault(ep => ep.ProjectId == pwl.ParentProjectId && ep.IsTeamLeaderProjectRole);
			if (firstLeaderFromParentAssignment == null)
			{
				continue;
			}

			var existingNotLeader = employeeProjectEntities.FirstOrDefault(ep => ep.EmployeeId == firstLeaderFromParentAssignment.EmployeeId && ep.ProjectId == pwl.ProjectId);

			if (existingNotLeader != null)
			{
				existingNotLeader.IsTeamLeaderProjectRole = true;
			}
			else
			{
				employeeProjectEntities.Add(new EmployeeProjectEntity
				{
					EmployeeId = firstLeaderFromParentAssignment.EmployeeId,
					ProjectId = pwl.ProjectId,
					IsTeamLeaderProjectRole = true
				});
			}
		}

		var existingEmployeeProjects = await _applicationDbContext.EmployeeProjects.Select(e => new { e.EmployeeId, e.ProjectId, e.IsTeamLeaderProjectRole }).ToListAsync();

		var duplicatedEmployeeProjects = employeeProjectEntities.Where(ep => existingEmployeeProjects.Any(e => e.ProjectId.Equals(ep.ProjectId) && e.EmployeeId.Equals(ep.EmployeeId) && e.IsTeamLeaderProjectRole.Equals(ep.IsTeamLeaderProjectRole))).ToList();

		duplicatedEmployeeProjects.ForEach(d => employeeProjectEntities.Remove(d));

		await _applicationDbContext.EmployeeProjects.AddRangeAsync(employeeProjectEntities);

		await _applicationDbContext.SaveChangesAsync();

		_reportDataToLog.AssignmentsAfterImportCount = employeeProjectEntities.Count;

		await AddTeamLeaderUserRolesAsync(employeeProjectEntities);
	}

	private async Task AddTeamLeaderUserRolesAsync(List<EmployeeProjectEntity> employeeProjectEntities)
	{
		var teamLeaderRole = await _applicationDbContext.Roles
		.Where(r => r.Name.Equals(RoleEntity.TeamLeader))
		.SingleAsync();

		_applicationDbContext.UserRoles.RemoveRange(_applicationDbContext.UserRoles.Where(ur => ur.RoleId == teamLeaderRole.Id));

		var teamLeadersUserRoles = employeeProjectEntities.Where(ep => ep.IsTeamLeaderProjectRole)
								.GroupBy(ep => ep.EmployeeId)
								.Select(g => new UserRoleEntity
								{
									Role = teamLeaderRole,
									RoleId = teamLeaderRole.Id,
									EmployeeId = g.Key
								});
		_applicationDbContext.UserRoles.AddRange(teamLeadersUserRoles);

		await _applicationDbContext.SaveChangesAsync();
	}

	private async Task RemoveEmptyProjectsAndEmployeesWithoutProjects()
	{
		var projectsToDelete = await _applicationDbContext.Projects
											.Where(p => p.Employees.Count == 0 && p.IsExternal == false)
											.ToListAsync();
		_applicationDbContext.Projects.RemoveRange(projectsToDelete);

		var employeesToRemove = await _applicationDbContext.Employees
											.Include(e => e.EmployeeDeskReservations) // hanlde removing employees assigned to desk
												.ThenInclude(dr => dr.Desk)
											.Where(e => e.Projects.Count == 0 && e.IsExternal == false)
											.ToListAsync();

		_applicationDbContext.Employees.RemoveRange(employeesToRemove);

		var employeesToRemoveIds = employeesToRemove.Select(e => e.Id).ToList();

		var employeeEquipmentHistoryEntities = await _applicationDbContext.EmployeeEquipmentHistory
																		  .Where(eeh => eeh.EmployeeId != null && employeesToRemoveIds.Contains(eeh.EmployeeId.Value))
																		  .ToListAsync();

		_applicationDbContext.EmployeeEquipmentHistory.RemoveRange(employeeEquipmentHistoryEntities);

		await _applicationDbContext.SaveChangesAsync();

		_reportDataToLog.RemovedEmptyProjectsCount = projectsToDelete.Count;
		_reportDataToLog.RemovedEmployeesWithoutProjectsCount = employeesToRemove.Count;
	}

	private async Task RemoveIgnoredProjects()
	{
		var externalIds = (await _applicationDbContext.Configs.SingleAsync(c => c.Key == DbConfigKey.IgnoredProjects))
			.GetIgnoredProjects()
			.Select(p => p.ExternalId)
			.ToList();
		var projectsToDelete = await _applicationDbContext.Projects.Where(p => p.ExternalId.HasValue && externalIds.Contains(p.ExternalId.Value)).ToListAsync();
		_applicationDbContext.Projects.RemoveRange(projectsToDelete);

		await _applicationDbContext.SaveChangesAsync();
	}

	private async Task SetDivisions(IEnumerable<Group> groups)
	{
		if (await _applicationDbContext.Projects.AnyAsync(p => p.DivisionExternalId == null))
		{
			var divsExternalIds = (await _applicationDbContext.Configs.SingleAsync(c => c.Key == DbConfigKey.Divisions))
			.GetDivisions()
			.Select(p => p.ExternalGroupId)
			.ToList();

			if (!divsExternalIds.Any())
			{
				throw new Exception("Divisions not defined!");
			}

			var projects = await _applicationDbContext.Projects
									.Where(p => p.IsExternal == false)
									.ToListAsync();

			foreach (var project in projects)
			{
				if (project.DivisionExternalId != null || !project.ExternalId.HasValue)
				{
					continue;
				}
				project.DivisionExternalId = SetDivisionFromGroupsHierachy(groups, projects, divsExternalIds, project.ExternalId.Value);
			}

			await _applicationDbContext.SaveChangesAsync();
		}
	}

	private int? SetDivisionFromGroupsHierachy(
		IEnumerable<Group> groups,
		List<ProjectEntity> allProjects,
		List<int> divsExternalIds,
		int projectExternalId,
		List<ProjectEntity>? middleProjects = null)
	{
		if (divsExternalIds.Contains(projectExternalId)) // in case of some employee was assigned directly to division group - in this case division project would exists in database
		{
			return projectExternalId;
		}

		var group = groups.Single(g => g.Id == projectExternalId);

		if (group.ParentGroupId == null)
		{
			_logger.LogError($"Group EmployeeId = {group.Id}, Name = '{group.Name}' does not have parent group!");
			return null;
		}
		else if (divsExternalIds.Contains(group.ParentGroupId.Value))
		{
			if (middleProjects != null)
			{
				middleProjects.ForEach(p => p.DivisionExternalId = group.ParentGroupId.Value);
			}

			return group.ParentGroupId.Value;
		}
		else
		{
			var middleProject = allProjects.SingleOrDefault(p => p.ExternalId == group.ParentGroupId.Value && p.DivisionExternalId == null);
			if (middleProject != null)
			{
				if (middleProjects == null)
				{
					middleProjects = new List<ProjectEntity>();
				}
				middleProjects.Add(middleProject);
			}

			return SetDivisionFromGroupsHierachy(groups, allProjects, divsExternalIds, group.ParentGroupId.Value, middleProjects);
		}
	}

	private async Task SetAdminRoles()
	{
		List<EmployeeEntity> employees= await _applicationDbContext
            .Employees
            .Include(e => e.UserRoles)
				.ThenInclude(ur => ur.Role)
            .Include(e => e.Projects)
            .Where(e => e.Projects.Any(ep => ep.Project.ExternalId.HasValue && _autoImportSettings.AutoAdminProjectExternalIds.Contains((int)ep.Project.ExternalId)))
            .ToListAsync();
			
		RoleEntity adminRole = await _applicationDbContext
			.Roles
			.Where(r => r.Name == RoleEntity.Admin)
			.FirstAsync();

		employees.ForEach(e => e.SetRoleIfNotAlreadySet(adminRole));

		await _applicationDbContext.SaveChangesAsync();
	}
}
