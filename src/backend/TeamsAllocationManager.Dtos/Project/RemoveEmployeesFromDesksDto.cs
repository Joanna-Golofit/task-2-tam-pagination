using System;
using System.Collections.Generic;

namespace TeamsAllocationManager.Dtos.Project;

public class RemoveEmployeesFromDesksDto
{
	public IEnumerable<Guid> DeskIds { get; set; } = new List<Guid>();
	public Guid RoomId { get; set; }
}
