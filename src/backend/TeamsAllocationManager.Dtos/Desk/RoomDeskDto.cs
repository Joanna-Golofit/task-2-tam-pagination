using System;

namespace TeamsAllocationManager.Dtos.Desk;

public class RoomDeskDto
{
	public Guid RoomId { get; set; }
	public string RoomName { get; set; } = null!;
	public Guid DeskId { get; set; }
	public int DeskNumber { get; set; }
}
