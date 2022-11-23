using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Employee.Commands;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Employee;

public class CreateExternalCompanyEmployeeHandler : IAsyncCommandHandler<CreateExternalCompanyEmployeeCommand>
{
	private readonly IProjectRepository _projectRepository;

	public CreateExternalCompanyEmployeeHandler(IProjectRepository projectRepository)
	{
		_projectRepository = projectRepository;
	}

	public async Task HandleAsync(CreateExternalCompanyEmployeeCommand command, CancellationToken cancellationToken = default)
	{
		var externalCompany = await _projectRepository.GetExternalProject(command.CompanyId);

		if (externalCompany == null)
		{
			throw new EntityNotFoundException<ProjectEntity>(command.CompanyId);
		}

		int maxSurname = externalCompany.Employees.Select(e => {
			bool isParsed = int.TryParse(e.Employee.Surname ?? "-1", out int number);
			return number ;
		}).DefaultIfEmpty(0).Max();

		maxSurname = maxSurname >= 0 ? maxSurname + 1 : 1;

		for (int i = 0; i < command.EmployeeCount; i++)
		{
			externalCompany.Employees.Add(EmployeeProjectEntity.AddNewExternalEmployeeToProject(externalCompany, maxSurname++, false));
		}

		await _projectRepository.UpdateAsync(externalCompany);
	}
}
