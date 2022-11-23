using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base;
using TeamsAllocationManager.Contracts.Employee.Commands;
using TeamsAllocationManager.Contracts.LoggedUser.Queries;
using TeamsAllocationManager.Contracts.Project.Commands;
using TeamsAllocationManager.Contracts.Project.Queries;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Project;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Api.Functions;

public class ProjectFunction : FunctionBase
{
	public ProjectFunction(IDispatcher dispatcher) : base(dispatcher)
	{
	}

	[FunctionName("ProjectFunction")]
	public override async Task<IActionResult> RunAsync(
		[HttpTrigger(AuthorizationLevel.Anonymous, Route = "Project/{*path}")]
		HttpRequest req, string? path, ILogger log)
		=> await base.RunAsync(req, path, log);

	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpGet]
	public async Task<IEnumerable<ProjectDto>> GetProjects(
		[FromQuery] ProjectsFiltersDto query)
	{
		IEnumerable<string> userRoles =
			await _dispatcher.DispatchAsync<GetUserRoleQuery, IEnumerable<string>>(
				new GetUserRoleQuery(CurrentUsername));
		if (userRoles.All(r => r != RoleEntity.Admin))
		{
			throw new InvalidUserRoleException();
		}

		return await _dispatcher.DispatchAsync<GetProjectsQuery, IEnumerable<ProjectDto>>(
			new GetProjectsQuery());
	}

	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpGet("GetAllProjectsForDropdown")]
	public async Task<IEnumerable<ProjectForDropdownDto>> GetAllProjectsForDropdown([FromQuery] string? search)
		=> await _dispatcher.DispatchAsync<GetAllProjectsForDropdownQuery, IEnumerable<ProjectForDropdownDto>>(
			new GetAllProjectsForDropdownQuery(search));

	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpGet("{projectId}")]
	public async Task<ProjectDetailsDto?> GetProjectDetails(Guid projectId)
		=> await _dispatcher.DispatchAsync<GetProjectDetailsQuery, ProjectDetailsDto?>(
			new GetProjectDetailsQuery(projectId));

	[OnlyForRoles(RoleEntity.Admin)]
	[HttpPost("NewExternalCompany")]
	public async Task<Guid> CreateExternalCompany(NewExternalCompanyDto newExternalCompany)
		=> await _dispatcher.DispatchAsync<CreateExternalCompanyCommand, Guid>(
			new CreateExternalCompanyCommand(newExternalCompany));

	[OnlyForRoles(RoleEntity.Admin)]
	[HttpPut("{companyId}/Update")]
	public async Task UpdateExternalCompany(Guid companyId, EditExternalCompanyDto editExternalCompany)
		=> await _dispatcher.DispatchAsync<EditExternalCompanyCommand>(
			new EditExternalCompanyCommand(companyId, editExternalCompany));

	[OnlyForRoles(RoleEntity.Admin)]
	[HttpPost("{companyId}/AddEmployees")]
	public async Task AddEmployeesToCompany(Guid companyId, int employeeCount)
		=> await _dispatcher.DispatchAsync<CreateExternalCompanyEmployeeCommand>(
			new CreateExternalCompanyEmployeeCommand(companyId, employeeCount));

	// TODO: This endpoint should be replaced by ReserveDesk on frontend side
	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpPut("AssignEmployeesToDesks")]
	public async Task AssignEmployeesToDesks(AssignEmployeesToDesksDto assignEmployeesToDesksDto)
		=> await _dispatcher.DispatchAsync<AssignEmployeesToDesksCommand>(
			new AssignEmployeesToDesksCommand(assignEmployeesToDesksDto));

	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpPut("RemoveEmployeesFromDesks")]
	public async Task RemoveEmployeesFromDesks(RemoveEmployeesFromDesksDto dto)
		=> await _dispatcher.DispatchAsync<RemoveEmployeesFromDesksCommand>(new RemoveEmployeesFromDesksCommand(dto));

	[OnlyForRoles(RoleEntity.Admin)]
	[HttpDelete("{companyId}")]
	public async Task DeleteExternalCompany(Guid companyId)
		=> await _dispatcher.DispatchAsync<DeleteExternalCompanyCommand>(new DeleteExternalCompanyCommand(companyId));
}