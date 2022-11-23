using System.Collections.Generic;
using TeamsAllocationManager.Dtos.Desk;
using TeamsAllocationManager.Dtos.Project;

namespace TeamsAllocationManager.Dtos.Room;

public class RoomDetailsDto : RoomDto
{
	public decimal AreaMinLevelPerPerson { get; set; }
	public string SasTokenForRoomPlans { get; set; } = null!;
	public IEnumerable<ProjectForRoomDetailsDto>? ProjectsInRoom { get; set; }
	public IEnumerable<DeskForRoomDetailsDto>? DesksInRoom { get; set; }
}
