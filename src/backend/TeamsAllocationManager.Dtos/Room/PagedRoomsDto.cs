﻿using System.Collections.Generic;
using TeamsAllocationManager.Dtos.Building;
using TeamsAllocationManager.Dtos.Common;
using TeamsAllocationManager.Dtos.Enums;

namespace TeamsAllocationManager.Dtos.Room
{
	public class PagedRoomsDto
	{
		public int MaxFloor { get; set; }
		public decimal AreaMinLevelPerPerson { get; set; }
		public ErrorCodes ErrorCode { get; set; }
		public PagedListResultDto<IEnumerable<RoomDto>> Rooms { get; set; } = null!;
		public IEnumerable<BuildingDto> Buildings { get; set; } = null!;
	}
}
