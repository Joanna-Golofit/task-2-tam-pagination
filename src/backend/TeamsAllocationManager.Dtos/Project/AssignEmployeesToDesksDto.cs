using System;
using System.Collections.Generic;

namespace TeamsAllocationManager.Dtos.Project;

public class AssignEmployeesToDesksDto
{
	public Guid ProjectId { get; set; }
	public Guid RoomId { get; set; }
	public IEnumerable<DeskEmployeeDto> DeskEmployeeDtos { get; set; } = new List<DeskEmployeeDto>();
}
