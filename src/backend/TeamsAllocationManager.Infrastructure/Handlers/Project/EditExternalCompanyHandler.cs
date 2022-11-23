using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Project.Commands;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Project;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Project;

public class EditExternalCompanyHandler : IAsyncCommandHandler<EditExternalCompanyCommand>
{
	private readonly IProjectRepository _projectRepository;

	public EditExternalCompanyHandler(IProjectRepository projectRepository)
	{
		_projectRepository = projectRepository;
	}

	public async Task HandleAsync(EditExternalCompanyCommand command, CancellationToken cancellationToken = default)
	{
		ProjectEntity? externalCompany = await _projectRepository.GetProject(command.CompanyId);

		if (externalCompany == null)
		{
			throw new EntityNotFoundException<ProjectEntity>(command.CompanyId);
		}

		externalCompany.Name = command.Dto.Name ?? externalCompany.Name;
		externalCompany.Email = command.Dto.Email ?? externalCompany.Email;
		externalCompany.Employees.ToList().ForEach(e => ModifyEmployee(e.Employee, command.Dto));

		await _projectRepository.UpdateAsync(externalCompany);
	}

	private void ModifyEmployee(EmployeeEntity employee, EditExternalCompanyDto newCompanyInfo)
	{
		employee.Name = newCompanyInfo.Name ?? employee.Name;
		employee.Email = !string.IsNullOrEmpty(newCompanyInfo.Email)
			? $"{employee.Surname}{newCompanyInfo.Email}"
			: employee.Email;
		employee.UserLogin = !string.IsNullOrEmpty(newCompanyInfo.Name)
			? $"{newCompanyInfo.Name}{employee.Surname}"
			: employee.UserLogin;
	}
}
