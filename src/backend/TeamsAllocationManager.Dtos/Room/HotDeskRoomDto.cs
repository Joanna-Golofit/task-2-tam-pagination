using System;
using TeamsAllocationManager.Dtos.Building;

namespace TeamsAllocationManager.Dtos.Room;

public class HotDeskRoomDto
{
	public Guid Id { get; set; }
	public BuildingDto Building { get; set; } = null!;
	public decimal Area { get; set; }
	public string Name { get; set; } = null!;
	public int Floor { get; set; }
	public int HotDesksCount { get; set; }
	public int FreeHotDeskCount { get; set; }
}
