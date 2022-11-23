using System;
using System.Collections.Generic;
using TeamsAllocationManager.Dtos.Desk;

namespace TeamsAllocationManager.Dtos.Room;

public class EmployeeForRoomDetailsDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = null!;
	public string Surname { get; set; } = null!;
	public IEnumerable<string> ProjectsNames { get; set; } = new List<string>();
	public IEnumerable<RoomDeskDto> RoomDeskDtos { get; set; } = new List<RoomDeskDto>();
}
