using System;
using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Dtos.Desk;

namespace TeamsAllocationManager.Contracts.Desks.Queries
{
	public class GetActiveReservationsForDeskQuery : IQuery<IEnumerable<ScheduledDeskReservationInfoDto>>
	{
		public Guid DeskId { get; }

		public GetActiveReservationsForDeskQuery(Guid deskId)
		{
			DeskId = deskId;
		}
	}
}
