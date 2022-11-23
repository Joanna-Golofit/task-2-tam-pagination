using System;
using System.Collections.Generic;
using System.Text;

namespace TeamsAllocationManager.Dtos.Room;

public class SetRoomAsHotDeskDto
{
	public Guid RoomId { get; set;  }
	public bool IsHotDesk { get; set; }
}
