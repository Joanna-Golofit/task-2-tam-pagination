using System;
using System.Collections.Generic;
using TeamsAllocationManager.Dtos.Employee;

namespace TeamsAllocationManager.Dtos.Room;

public class DeskReservationDto
{
	public Guid Id { get; set; }

	public EmployeeForRoomDetailsDto Employee { get; set; } = null!;

	public DateTime ReservationDate { get; set; }

	public DateTime? ReservationEnd { get; set; }

	public bool IsSchedule { get; set; }

	public IEnumerable<DayOfWeek>? ScheduledWeekdays { get; set; }
}
