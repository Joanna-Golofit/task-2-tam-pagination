using System;

namespace TeamsAllocationManager.Dtos.Desk;

public class DeskLocationDto
{
	public Guid Id { get; set; }
	public Guid RoomId { get; set; }
	public string LocationName { get; set; } = null!;
	public int DeskNumber { get; set; }
}
