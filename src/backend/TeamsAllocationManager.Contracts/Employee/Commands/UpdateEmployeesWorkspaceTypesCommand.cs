using System;
using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Dtos.Employee;

namespace TeamsAllocationManager.Contracts.Employee.Commands;

public class UpdateEmployeesWorkspaceTypesCommand : ICommand<bool>
{ 
	public IEnumerable<UpdateEmployeeWorkspaceTypeDto> Dtos { get; }
	public UpdateEmployeesWorkspaceTypesCommand(IEnumerable<UpdateEmployeeWorkspaceTypeDto> dtos)
	{
		Dtos = dtos;
	}
}
