using System;
using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Dtos.Desk;

namespace TeamsAllocationManager.Contracts.Employee.Queries
{
	public class GetActiveDeskReservationsForEmployeeQuery : IQuery<IEnumerable<ScheduledDeskReservationInfoDto>>
	{
		public Guid EmployeeId { get; }

		public GetActiveDeskReservationsForEmployeeQuery(Guid employeeId)
		{
			EmployeeId = employeeId;
		}
	}
}
