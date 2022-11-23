using System;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Dtos.Room;

namespace TeamsAllocationManager.Contracts.Desks.Queries;

public class GetHotDesksQuery : IQuery<HotDeskRoomsDto>
{
	public (DateTime? startDate, DateTime? endDate) Period { get; set; }

	public GetHotDesksQuery(DateTime? startDate, DateTime? endDate)
	{
		Period = (startDate, endDate);
	}
}
