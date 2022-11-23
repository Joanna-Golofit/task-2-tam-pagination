using System.Collections.Generic;
using TeamsAllocationManager.Dtos.Building;
using TeamsAllocationManager.Dtos.Common;

namespace TeamsAllocationManager.Dtos.Room
{
	public class PagedHotDeskRoomsDto
	{
		public int MaxFloor { get; set; }
		public decimal AreaMinLevelPerPerson { get; set; }
		public IEnumerable<HotDeskRoomDto> Rooms { get; set; } = null!;
		public IEnumerable<BuildingDto> Buildings { get; set; } = null!;
	}
}
