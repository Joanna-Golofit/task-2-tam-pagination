using System;
using System.ComponentModel.DataAnnotations;

namespace TeamsAllocationManager.Dtos.Desk;

public class NewHotDeskReservationDto
{
	[Required]
	public Guid DeskId { get; set; }
	[Required]
	public DateTime ReservationStart { get; set; }
	[Required]
	public DateTime ReservationEnd { get; set; }
	public Guid ReservingEmployee { get; set; }
	public string? CreatedBy { get; set; }
}
