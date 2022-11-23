using System;
using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Dtos.Desk;

namespace TeamsAllocationManager.Contracts.Desks.Queries;

public class GetActiveReservationsForHotDeskQuery : IQuery<IEnumerable<ReservationInfoDto>>
{
	public Guid DeskId { get; }

	public GetActiveReservationsForHotDeskQuery(Guid deskId)
	{
		DeskId = deskId;
	}
}
