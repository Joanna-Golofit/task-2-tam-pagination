using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.Employee.Queries;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Dtos.Employee;

namespace TeamsAllocationManager.Infrastructure.Handlers.Employee;

public class GetAllUsersForSearcherHandler : IAsyncQueryHandler<GetAllUsersForSearcherQuery, UsersForSearcherDto>
{
	private readonly IEmployeesRepository _employeesRepository;

	public GetAllUsersForSearcherHandler(IEmployeesRepository employeesRepository)
	{
		_employeesRepository = employeesRepository;
	}

	public async Task<UsersForSearcherDto> HandleAsync(GetAllUsersForSearcherQuery query, CancellationToken cancellationToken = default)
	{
		var employees = await _employeesRepository.GetEmployeesForSearcher();

		return new UsersForSearcherDto { Users = employees.Select(e =>
				new UserForSearcherDto
				{
					Id = e.Id,
					Email = e.Email!,
					DisplayName = $"{e.Name} {e.Surname}",
					WorkspaceType = (int?)e.WorkspaceType,
					ProjectsNames = e.Projects.Select(p => p.Project.Name)
				}).ToList()
		};
	}
}
