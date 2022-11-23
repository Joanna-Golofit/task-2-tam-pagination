using System;

namespace TeamsAllocationManager.Dtos.Desk;

public class SetHotDeskDto
{
	public Guid DeskId { get; set; }
	public Guid RoomId { get; set; }
	public bool IsHotDesk { get; set; }
}
