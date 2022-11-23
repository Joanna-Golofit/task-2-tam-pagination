using System.Collections.Generic;
using TeamsAllocationManager.Dtos.Desk;

namespace TeamsAllocationManager.Dtos.Room;

public class RoomForProjectDto : RoomDto
{
	public string SasTokenForRoomPlans { get; set; } = null!;
	public IEnumerable<DeskForProjectDetailsDto> DesksInRoom { get; set; } = new List<DeskForProjectDetailsDto>();
}
