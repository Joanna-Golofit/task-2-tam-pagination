using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Queries;

namespace TeamsAllocationManager.Contracts.LoggedUser.Queries;

public class GetUserRoleQuery : IQuery<IEnumerable<string>>
{
	public string CurrentUsername { get; }

	public GetUserRoleQuery(string currentUsername)
	{
		CurrentUsername = currentUsername;
	}
}
