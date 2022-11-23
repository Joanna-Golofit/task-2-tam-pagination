using System;
using System.Collections.Generic;
using System.Text;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Dtos.Enums;

namespace TeamsAllocationManager.Contracts.Room.Commands;

public class SetRoomAsHotDeskCommand: ICommand
{
	public Guid RoomId { get; }
	public bool IsHotDesk { get; }

	public SetRoomAsHotDeskCommand(Guid roomId, bool isHotDesk)
	{
		RoomId = roomId;
		IsHotDesk = isHotDesk;
	}
}
