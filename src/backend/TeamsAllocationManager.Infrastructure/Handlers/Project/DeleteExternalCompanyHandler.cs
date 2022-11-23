using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Project.Commands;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Project;

public class DeleteExternalCompanyHandler : IAsyncCommandHandler<DeleteExternalCompanyCommand>
{
	private readonly IProjectRepository _projectRepository;
	private readonly IEmployeesRepository _employeesRepository;

	public DeleteExternalCompanyHandler(
		IProjectRepository projectRepository, 
		IEmployeesRepository employeesRepository)
	{
		_projectRepository = projectRepository;
		_employeesRepository = employeesRepository;
	}

	public async Task HandleAsync(DeleteExternalCompanyCommand command, CancellationToken cancellationToken = default)
	{
		ProjectEntity? externalCompany = await _projectRepository.GetExternalProjectWithDesks(command.CompanyId);

		if (externalCompany == null)
		{
			throw new EntityNotFoundException<ProjectEntity>(command.CompanyId);
		}

		var companyEmployees = externalCompany.Employees.Select(ep => ep.Employee);
		foreach (var employee in companyEmployees)
		{
			employee.EmployeeDeskReservations?.ToList().ForEach(dr => dr.Desk.ReleaseDesk());
		}

		await _employeesRepository.RemoveRangeAsync(companyEmployees);
		await _projectRepository.RemoveAsync(externalCompany);
	}
}
