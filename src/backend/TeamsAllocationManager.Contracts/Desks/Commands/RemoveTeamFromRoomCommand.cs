using System;
using TeamsAllocationManager.Contracts.Base.Commands;

namespace TeamsAllocationManager.Contracts.Desks.Commands;

public class RemoveTeamFromRoomCommand : ICommand
{
	public Guid RoomId { get; }
	public Guid ProjectId { get; }

	public RemoveTeamFromRoomCommand(Guid roomId, Guid projectId)
	{
		RoomId = roomId;
		ProjectId = projectId;
	}
}
