using System;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Dtos.Enums;

namespace TeamsAllocationManager.Contracts.Employee.Commands;

public class DeleteExternalCompanyEmployeeCommand : ICommand
{
	public Guid EmployeeId { get; }

	public DeleteExternalCompanyEmployeeCommand(Guid employeeId)
	{
		EmployeeId = employeeId;
	}
}
