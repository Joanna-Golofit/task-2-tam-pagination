using System;

namespace TeamsAllocationManager.Dtos.Desk;

public class ReleaseDeskEmployeeDto
{
	public Guid DeskId { get; set; }

	public Guid EmployeeId { get; set; }
}
