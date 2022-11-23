using System;
using TeamsAllocationManager.Dtos.Building;

namespace TeamsAllocationManager.Dtos.Floor;

public class FloorDto
{
	public Guid Id { get; set; }
	public BuildingDto Building { get; set; } = null!;
	public int Floor { get; set; }
	public int RoomCount { get; set; }
	public int Capacity { get; set; }
	public int OccupiedDesks { get; set; }
	public decimal Area { get; set; }
}
