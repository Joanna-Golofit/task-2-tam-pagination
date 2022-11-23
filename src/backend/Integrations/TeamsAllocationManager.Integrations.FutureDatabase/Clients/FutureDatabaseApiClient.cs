using Microsoft.Identity.Web;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamsAllocationManager.Integrations.Builders;
using TeamsAllocationManager.Integrations.Clients;
using TeamsAllocationManager.Integrations.FutureDatabase.Enums;
using TeamsAllocationManager.Integrations.FutureDatabase.Models;

namespace TeamsAllocationManager.Integrations.FutureDatabase.Clients;

public class FutureDatabaseApiClient : ApiClient, IFutureDatabaseApiClient
{
	public FutureDatabaseApiClient(IDownstreamWebApi downstreamWebApi)
		: base(downstreamWebApi, "FutureDatabase")
	{
	}

	public async Task<ICollection<Group>?> GetGroupsAsync()
	{
		string path = new ODataQueryBuilder("Groups")
			.Select<Group>()
			.Expand(nameof(Group.Assignments))
			.Build();

		return await GetAsync<ICollection<Group>>(path);
	}

	public async Task<ICollection<User>?> GetUsersAsync()
	{
		string expandOption = $"{nameof(User.AssignmentUsers)}($select={nameof(Assignment.RoleId)})";
		string filterOption = $"{nameof(User.UserTypeId)} in (" +
				$"{(int)UserTypeFDB.Employee}," +
				$"{(int)UserTypeFDB.Contractor})";

		string path = new ODataQueryBuilder("Users")
			.Select<User>()
			.Expand(expandOption)
			.Filter(filterOption)
			.Build();

		return await GetAsync<ICollection<User>>(path);
	}
}
