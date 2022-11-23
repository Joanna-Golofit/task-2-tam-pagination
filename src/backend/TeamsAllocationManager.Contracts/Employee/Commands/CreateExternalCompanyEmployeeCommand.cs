using System;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Dtos.Enums;

namespace TeamsAllocationManager.Contracts.Employee.Commands;

public class CreateExternalCompanyEmployeeCommand : ICommand
{
	public Guid CompanyId { get; }
	public int EmployeeCount { get; }

	public CreateExternalCompanyEmployeeCommand(Guid companyId, int employeeCount)
	{
		CompanyId = companyId;
		EmployeeCount = employeeCount;
	}
}
