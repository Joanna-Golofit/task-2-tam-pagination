using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base;
using TeamsAllocationManager.Contracts.Employee.Commands;
using TeamsAllocationManager.Contracts.Employee.Queries;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Employee;

namespace TeamsAllocationManager.Api.Functions;

public class EmployeeFunction : FunctionBase
{
	public EmployeeFunction(IDispatcher dispatcher) : base(dispatcher)
	{
	}

	[FunctionName("EmployeeFunction")]
	public override async Task<IActionResult> RunAsync(
		[HttpTrigger(AuthorizationLevel.Anonymous, Route = "Employee/{*path}")]
		HttpRequest req, string? path, ILogger log)
		=> await base.RunAsync(req, path, log);

	[HttpGet("GetTeamLeaders")]
	public async Task<IEnumerable<TeamLeaderDto>> GetTeamLeaders()
		=> await _dispatcher.DispatchAsync<GetTeamLeadersQuery, IEnumerable<TeamLeaderDto>>(new GetTeamLeadersQuery());

	[HttpGet("GetTeamLeaderProjectsForDropdown/{teamLeaderId}")]
	public async Task<IEnumerable<TeamLeaderProjectForDropdownDto>> GetTeamLeaderProjectsForDropdown(Guid teamLeaderId)
		=> await _dispatcher
			.DispatchAsync<GetTeamLeaderProjectsForDropdownQuery, IEnumerable<TeamLeaderProjectForDropdownDto>>(
				new GetTeamLeaderProjectsForDropdownQuery(teamLeaderId));

	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpPut("UpdateEmployeesWorkspaceTypes")]
	public async Task<IActionResult> UpdateEmployeesWorkspaceTypes(
		IEnumerable<UpdateEmployeeWorkspaceTypeDto> updateEmployeeWorkspaceTypeDtos)
		=> await _dispatcher.DispatchAsync<UpdateEmployeesWorkspaceTypesCommand, bool>(
			new UpdateEmployeesWorkspaceTypesCommand(updateEmployeeWorkspaceTypeDtos))
			? new OkObjectResult(updateEmployeeWorkspaceTypeDtos)
			: new NotFoundObjectResult(updateEmployeeWorkspaceTypeDtos);

	[OnlyForRoles(RoleEntity.Admin)]
	[HttpDelete("{employeeId}")]
	public async Task DeleteExternalEmployee(Guid employeeId)
		=> await _dispatcher.DispatchAsync<DeleteExternalCompanyEmployeeCommand>(
			new DeleteExternalCompanyEmployeeCommand(employeeId));
}