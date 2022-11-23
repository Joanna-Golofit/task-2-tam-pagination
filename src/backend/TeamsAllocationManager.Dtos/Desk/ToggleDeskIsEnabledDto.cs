using System;
using System.Collections.Generic;

namespace TeamsAllocationManager.Dtos.Desk;

public class ToggleDeskIsEnabledDto
{
	public IEnumerable<Guid> DesksIds { get; set; } = null!;
}
