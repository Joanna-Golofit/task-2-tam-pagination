namespace TeamsAllocationManager.Dtos.Floor
{
	public class FloorsQueryDto
	{
		public string? BuildingName { get; init; }
		public int? FloorNumber { get; init; }
		public OccupiedDesksRangeFilterDto? OccupiedDesksRange { get; init; }
		public CapacityRangeFilterDto? CapacityRange { get; init; }
	}

	public class OccupiedDesksRangeFilterDto
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
