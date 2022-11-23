using System;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Dtos.Desk;

namespace TeamsAllocationManager.Contracts.Desks.Commands;

public class SetHotDeskCommand : ICommand<DeskForRoomDetailsDto>
{
	public Guid DeskId { get; }
	public Guid RoomId { get; }
	public bool IsHotDesk { get; }
	public SetHotDeskCommand(Guid deskId, Guid roomId, bool isHotDesk)
	{
		DeskId = deskId;
		RoomId = roomId;
		IsHotDesk = isHotDesk;
	}
}
