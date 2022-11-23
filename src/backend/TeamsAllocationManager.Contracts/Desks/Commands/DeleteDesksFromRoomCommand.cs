using System;
using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Commands;

namespace TeamsAllocationManager.Contracts.Desks.Commands;

public class DeleteDesksFromRoomCommand : ICommand<bool>
{
	public Guid RoomId { get; }
	public IEnumerable<Guid> DeskIdsToDelete { get; }
	public DeleteDesksFromRoomCommand(Guid roomId, IEnumerable<Guid> deskIdsToDelete)
	{
		RoomId = roomId;
		DeskIdsToDelete = deskIdsToDelete;
	}
}
