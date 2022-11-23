using System;
using System.Collections.Generic;
using TeamsAllocationManager.Dtos.Employee;

namespace TeamsAllocationManager.Dtos.Desk
{
	public class ScheduledDeskReservationInfoDto
	{
		public Guid Id { get; set; }
		public DeskLocationDto Desk { get; set; } = null!;
		public EmployeeBasicInfoDto Employee { get; set; } = null!;
		public EmployeeBasicInfoDto CreatedBy { get; set; } = null!;
		public DateTime ReservationStart { get; set; }
		public IEnumerable<string> ScheduledWeekdays { get; set; } = null!;
	}
}
