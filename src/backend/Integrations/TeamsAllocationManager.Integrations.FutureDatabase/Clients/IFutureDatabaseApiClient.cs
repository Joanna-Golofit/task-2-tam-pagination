using System.Collections.Generic;
using System.Threading.Tasks;
using TeamsAllocationManager.Integrations.FutureDatabase.Models;

namespace TeamsAllocationManager.Integrations.FutureDatabase.Clients;

public interface IFutureDatabaseApiClient
{
	Task<ICollection<Group>?> GetGroupsAsync();

	Task<ICollection<User>?> GetUsersAsync();
}
