using System.Collections.Generic;
using TeamsAllocationManager.Dtos.Building;

namespace TeamsAllocationManager.Dtos.Floor;

public class FloorsDto
{
	public IEnumerable<BuildingDto> Buildings { get; set; } = null!;

	public int MaxFloor { get; set; }

	public IEnumerable<FloorDto> Floors { get; set; } = null!;
}
