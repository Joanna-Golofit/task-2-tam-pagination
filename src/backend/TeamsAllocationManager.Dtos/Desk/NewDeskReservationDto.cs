using System;
using System.Collections.Generic;

namespace TeamsAllocationManager.Dtos.Desk;

public class NewDeskReservationDto
{
	public Guid DeskId { get; set; }

	public Guid EmployeeId { get; set; }

	public DateTime? ReservationStart { get; set; }

	public DateTime? ReservationEnd { get; set; }

	public IEnumerable<DayOfWeek> ScheduledWeekdays { get; set; } = default!;
	public string? CreatedBy { get; set; }
}