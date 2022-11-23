using System;
using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Commands;

namespace TeamsAllocationManager.Contracts.Desks.Commands;

public class UpdateReservationCommand : ICommand
{
	public Guid ReservationId { get; set; }

	public Guid EmployeeId { get; set; }

	public IEnumerable<DayOfWeek> ScheduledWeekdays { get; set; } = default!;
}
