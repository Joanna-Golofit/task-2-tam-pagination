using System;
using System.Collections.Generic;

namespace TeamsAllocationManager.Dtos.Desk;

public class AllocateDesksDto
{
	public Guid RoomId { get; set; }
	public IEnumerable<Guid> DeskIds { get; set; } = null!;
}
