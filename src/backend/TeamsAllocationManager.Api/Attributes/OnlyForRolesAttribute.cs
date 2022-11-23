using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base;
using TeamsAllocationManager.Contracts.LoggedUser.Queries;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Api;

public class OnlyForRolesAttribute : Attribute
{
	private readonly IEnumerable<string> _roleGuard;

	public OnlyForRolesAttribute(params string[] roles)
	{
		_roleGuard = roles;
	}

	public async Task Validate(IDispatcher dispatcher, string userName)
	{
		IEnumerable<string> userRoles = await dispatcher.DispatchAsync<GetUserRoleQuery, IEnumerable<string>>(new GetUserRoleQuery(userName));
		if (!userRoles.Any(r => _roleGuard.Contains(r) || r == RoleEntity.Admin))
		{
			throw new InvalidUserRoleException();
		}
	}
}
