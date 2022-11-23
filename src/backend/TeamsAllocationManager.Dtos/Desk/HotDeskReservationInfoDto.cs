using System;

namespace TeamsAllocationManager.Dtos.Desk;

public class HotDeskReservationInfoDto
{
	public Guid Id { get; set; }
	public DeskLocationDto Desk { get; set; } = null!; 
	public string? EmployeeName { get; set; }
	public DateTime ReservationStart { get; set; }
	public DateTime ReservationEnd { get; set; }
}
