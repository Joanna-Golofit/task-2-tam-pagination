using System;
using System.Collections.Generic;
using System.Text;

namespace TeamsAllocationManager.Dtos.Employee;

public class UserLocationDto
{
	public Guid RoomId { get; set; }
	public string BuildingName { get; set; } = null!;
	public int FloorNumber { get; set; }
	public string RoomName { get; set; } = null!;
	public int DeskNumber { get; set; }
}
