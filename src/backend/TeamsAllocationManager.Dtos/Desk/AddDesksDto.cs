using System;

namespace TeamsAllocationManager.Dtos.Desk;

public class AddDesksDto
{
	public Guid RoomId { get; set; }
	public int FirstDeskNumber { get; set; }
	public int NumberOfDesks { get; set; }
}
