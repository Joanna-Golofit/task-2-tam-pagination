using System;

namespace TeamsAllocationManager.Dtos.Desk
{
	public class GetHotDesksQueryDto
	{
		public string? BuildingName { get; init; }
		public string? RoomName { get; init; }
		public FreeDesksRangeFilterDto? FreeDesksRange { get; init; }
		public ReservationDateRangeFilterDto? ReservationDateRangeFilter { get; init; }
	}

	public class FreeDesksRangeFilterDto
	{
		public int? Min { get; init; }
		public int? Max { get; init; }
	}

	public class ReservationDateRangeFilterDto
	{
		public string? StartDate { get; init; }
		public string? EndDate { get; init; }
	}
}
