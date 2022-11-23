using System;
using TeamsAllocationManager.Dtos.Building;

namespace TeamsAllocationManager.Dtos.Room;

public class RoomDto
{
	public Guid Id { get; set; }
	public BuildingDto Building { get; set; } = null!;
	public decimal Area { get; set; }
	public string Name { get; set; } = null!;
	public int Floor { get; set; }
	public int Capacity { get; set; }
	public int OccupiedDesksCount { get; set; }
	public int HotDesksCount { get; set; }
	public int DisabledDesksCount { get; set; }
	public string? RoomPlanInfo { get; set; }
	public virtual int FreeDesksCount => Capacity - OccupiedDesksCount - HotDesksCount - DisabledDesksCount;
}
