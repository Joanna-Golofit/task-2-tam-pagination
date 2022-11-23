using System;
using TeamsAllocationManager.Dtos.Enums;

namespace TeamsAllocationManager.Dtos.Employee;

public class UpdateEmployeeWorkspaceTypeDto
{
	public Guid EmployeeId { get; set; }
	public WorkspaceType? WorkspaceType { get; set; }
}
