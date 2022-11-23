namespace TeamsAllocationManager.Dtos.Room
{
	public class RoomsQueryFilterDto
	{
		public string? BuildingName { get; init; }
		public string? RoomName { get; init; }
		public FreeDesksRangeFilterDto? FreeDesksRange { get; init; }
		public CapacityRangeFilterDto? CapacityRange { get; init; }
	}

	public class FreeDesksRangeFilterDto
	{
		public int? Min { get; init; }
		public int? Max { get; init; }
	}

	public class CapacityRangeFilterDto
	{
		public int? Min { get; init; }
		public int? Max { get; init; }
	}
}
