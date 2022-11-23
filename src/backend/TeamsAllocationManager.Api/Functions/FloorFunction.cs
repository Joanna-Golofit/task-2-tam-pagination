using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base;
using TeamsAllocationManager.Contracts.Floor.Queries;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Floor;

namespace TeamsAllocationManager.Api.Functions;

public class FloorFunction : FunctionBase
{
	public FloorFunction(IDispatcher dispatcher) : base(dispatcher)
	{
	}

	[FunctionName("FloorFunction")]
	public override async Task<IActionResult> RunAsync(
		[HttpTrigger(AuthorizationLevel.Anonymous, Route = "Floor/{*path}")]
		HttpRequest req, string? path, ILogger log)
		=> await base.RunAsync(req, path, log);

	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpGet]
	public async Task<FloorsDto> GetAllFloors()
		=> await _dispatcher.DispatchAsync<GetAllFloorsQuery, FloorsDto>(new GetAllFloorsQuery());

	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpGet("Filter")]
	public async Task<PagedFloorsDto> GetFilteredFloors([FromQuery] FloorsQueryDto query)
		=> await _dispatcher.DispatchAsync<GetFilteredFloorsQuery, PagedFloorsDto>(new GetFilteredFloorsQuery());
}