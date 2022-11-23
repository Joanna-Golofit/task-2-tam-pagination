using System;
using TeamsAllocationManager.Dtos.Employee;

namespace TeamsAllocationManager.Dtos.Desk;

public class ReservationInfoDto
{
	public Guid Id { get; set; }
	public DeskLocationDto Desk { get; set; } = null!;
	public EmployeeBasicInfoDto Employee { get; set; } = null!;
	public EmployeeBasicInfoDto CreatedBy { get; set; } = null!;
	public DateTime ReservationStart { get; set; }
	public DateTime ReservationEnd { get; set; }
}
