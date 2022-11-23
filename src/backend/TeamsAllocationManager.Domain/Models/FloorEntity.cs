using System.Collections.Generic;

namespace TeamsAllocationManager.Domain.Models;

public class FloorEntity : Entity
{
	public BuildingEntity Building { get; set; } = null!;
	public int FloorNumber { get; set; }
	public ICollection<RoomEntity> Rooms { get; set; } = new List<RoomEntity>();
}
