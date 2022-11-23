using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.LoggedUser.Queries;
using TeamsAllocationManager.Database.Repositories.Interfaces;

namespace TeamsAllocationManager.Infrastructure.Handlers.LoggedUser;

public class GetIsUserAdminHandler : IAsyncQueryHandler<GetIsUserAdminQuery, bool>
{
	private readonly IEmployeesRepository _employeesRepository;

	public GetIsUserAdminHandler(IEmployeesRepository employeesRepository)
	{
		_employeesRepository = employeesRepository;
	}

	public async Task<bool> HandleAsync(GetIsUserAdminQuery query, CancellationToken cancellationToken = default)
	{
		return await _employeesRepository.IsUserAdmin(query.CurrentUsername);
	}
}
