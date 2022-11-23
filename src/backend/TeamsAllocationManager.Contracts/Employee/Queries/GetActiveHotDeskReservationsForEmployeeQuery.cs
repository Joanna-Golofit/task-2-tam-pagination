using System;
using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Dtos.Desk;

namespace TeamsAllocationManager.Contracts.Employee.Queries;

public class GetActiveHotDeskReservationsForEmployeeQuery : IQuery<IEnumerable<ReservationInfoDto>>
{
	public Guid EmployeeId { get; }

	public GetActiveHotDeskReservationsForEmployeeQuery(Guid employeeId)
	{
		EmployeeId = employeeId;
	}
}
