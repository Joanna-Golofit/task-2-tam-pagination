using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Employee.Commands;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Employee;

public class DeleteExternalCompanyEmployeeHandler : IAsyncCommandHandler<DeleteExternalCompanyEmployeeCommand>
{
	private readonly IEmployeesRepository _employeesRepository;

	public DeleteExternalCompanyEmployeeHandler(IEmployeesRepository employeesRepository)
	{
		_employeesRepository = employeesRepository;
	}

	public async Task HandleAsync(DeleteExternalCompanyEmployeeCommand command, CancellationToken cancellationToken = default)
	{
		var employee = await _employeesRepository.GetExternalEmployee(command.EmployeeId);

		if (employee == null)
		{
			throw new EntityNotFoundException<EmployeeEntity>(command.EmployeeId);
		}

		var projectEmployees = await _employeesRepository.GetEmployeesForProject(employee.Projects.First().ProjectId);

		var projectEmployeesList = projectEmployees.ToList();

		employee.EmployeeDeskReservations.Where(dr => dr.IsSchedule).Select(dr => dr.Desk).ToList().ForEach(d => d.ReleaseDesk());

		projectEmployeesList.Remove(employee);

		UpdateNumeration(projectEmployeesList, employee);

		await _employeesRepository.RemoveAsync(employee);
	}

	private void UpdateNumeration(List<EmployeeEntity> projectEmployees, EmployeeEntity employee)
	{
		bool hasNumber = int.TryParse(employee.Surname ?? "-1", out int number);

		if (!hasNumber)
		{
			return;
		}

		var employeesToUpdate = projectEmployees.Where(e => int.TryParse(e.Surname ?? "-1", out int _)).ToList();
		var email = employee.Projects.First()?.Project?.Email ?? "";

		for (int i = number - 1; i < employeesToUpdate.Count; i++)
		{
			employeesToUpdate[i].Surname = $"{i + 1}";
			employeesToUpdate[i].UserLogin = $"{projectEmployees[i].Name}{i + 1}";
			employeesToUpdate[i].Email = $"{i + 1}{email}";
		}
	}
}
