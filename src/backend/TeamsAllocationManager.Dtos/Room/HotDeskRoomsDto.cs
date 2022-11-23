using System.Collections.Generic;
using TeamsAllocationManager.Dtos.Building;

namespace TeamsAllocationManager.Dtos.Room;

public class HotDeskRoomsDto
{
	public int MaxFloor { get; set; }
	public decimal AreaMinLevelPerPerson { get; set; }
	public IEnumerable<HotDeskRoomDto> Rooms { get; set; } = null!;
	public IEnumerable<BuildingDto> Buildings { get; set; } = null!;
}
