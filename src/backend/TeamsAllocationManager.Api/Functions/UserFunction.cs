using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base;
using TeamsAllocationManager.Contracts.Employee.Queries;
using TeamsAllocationManager.Contracts.LoggedUser.Queries;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Employee;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Api.Functions;

public class UserFunction : FunctionBase
{
	public UserFunction(IDispatcher dispatcher) : base(dispatcher) { }

	[FunctionName("UserFunction")]
	public override async Task<IActionResult> RunAsync(
		[HttpTrigger(AuthorizationLevel.Anonymous, Route = "User/{*path}")]
		HttpRequest req, string? path, ILogger log)
		=> await base.RunAsync(req, path, log);

	[HttpGet("GetIsUserAdmin")]
	public async Task<bool> GetIsUserAdmin()
			=> await _dispatcher.DispatchAsync<GetIsUserAdminQuery, bool>(new GetIsUserAdminQuery(CurrentUsername));

	[HttpGet("GetAllUsersForSearcher")]
	public async Task<UsersForSearcherDto> GetAllUsersForSearcher()
			=> await _dispatcher.DispatchAsync<GetAllUsersForSearcherQuery, UsersForSearcherDto>(new GetAllUsersForSearcherQuery());

	[HttpGet("{id}")]
	public async Task<UserDetailsDto> GetUserDetails(Guid id)
			=> await _dispatcher.DispatchAsync<GetUserDetailsQuery, UserDetailsDto>(new GetUserDetailsQuery(id));

	[HttpGet("GetLoggedUserData")]
	[OnlyForRoles(RoleEntity.Admin)]
	public async Task<LoggedUserDataDto> GetLoggedUserData(string loggedUserEmail)
	{
		if (string.IsNullOrWhiteSpace(loggedUserEmail))
		{
			throw new InvalidArgumentException("loggedUserEmail is required");
		}
		return await _dispatcher.DispatchAsync<GetLoggedUserDataQuery, LoggedUserDataDto>(new GetLoggedUserDataQuery(CurrentUsername, loggedUserEmail));
	}

	[HttpGet("GetLoggedUserData")]
	public async Task<LoggedUserDataDto> GetLoggedUserData()
	{
		return await _dispatcher.DispatchAsync<GetLoggedUserDataQuery, LoggedUserDataDto>(new GetLoggedUserDataQuery(CurrentUsername));
	}
}
