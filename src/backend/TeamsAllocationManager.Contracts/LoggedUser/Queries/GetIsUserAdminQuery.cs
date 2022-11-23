using TeamsAllocationManager.Contracts.Base.Queries;

namespace TeamsAllocationManager.Contracts.LoggedUser.Queries;

public class GetIsUserAdminQuery : IQuery<bool>
{
	public string CurrentUsername { get; }

	public GetIsUserAdminQuery(string currentUsername)
	{
		CurrentUsername = currentUsername;
	}
}
