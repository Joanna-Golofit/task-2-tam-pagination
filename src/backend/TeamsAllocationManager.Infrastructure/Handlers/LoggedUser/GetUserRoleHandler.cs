using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.LoggedUser.Queries;
using TeamsAllocationManager.Database.Repositories.Interfaces;

namespace TeamsAllocationManager.Infrastructure.Handlers.LoggedUser;

public class GetUserRoleHandler : IAsyncQueryHandler<GetUserRoleQuery, IEnumerable<string>>
{
	private readonly IEmployeesRepository _employeesRepository;

	public GetUserRoleHandler(IEmployeesRepository employeesRepository)
	{
		_employeesRepository = employeesRepository;
	}

	public async Task<IEnumerable<string>> HandleAsync(GetUserRoleQuery query, CancellationToken cancellationToken = default)
	{
		return await _employeesRepository.GetUserRoleNames(query.CurrentUsername);
	}
}
