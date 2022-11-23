using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base;
using TeamsAllocationManager.Contracts.Summary.Queries;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Summary;

namespace TeamsAllocationManager.Api.Functions;

public class SummaryFunction : FunctionBase
{
	public SummaryFunction(IDispatcher dispatcher) : base(dispatcher) { }

	[FunctionName("SummaryFunction")]
	public override async Task<IActionResult> RunAsync(
		[HttpTrigger(AuthorizationLevel.Anonymous, Route = "Summary/{*path}")]
		HttpRequest req, string? path, ILogger log)
		=> await base.RunAsync(req, path, log);

	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpGet]
	public async Task<SummaryDto> GetSummary()
			=> await _dispatcher.DispatchAsync<GetSummaryQuery, SummaryDto>(new GetSummaryQuery());
}
