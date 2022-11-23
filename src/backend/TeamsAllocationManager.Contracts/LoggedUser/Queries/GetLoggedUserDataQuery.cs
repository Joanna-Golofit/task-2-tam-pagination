using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Dtos.Employee;

namespace TeamsAllocationManager.Contracts.LoggedUser.Queries;

public class GetLoggedUserDataQuery : IQuery<LoggedUserDataDto>
{
	public GetLoggedUserDataQuery(string currentUserName, string? loggedUserEmail = null)
	{
		LoggedUserEmail = loggedUserEmail;
		CurrentUsername = currentUserName;
	}

	public string? LoggedUserEmail { get; }
	public string CurrentUsername { get; }
}
