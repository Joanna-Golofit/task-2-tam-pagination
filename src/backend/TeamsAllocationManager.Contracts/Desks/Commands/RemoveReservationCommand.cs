using System;
using TeamsAllocationManager.Contracts.Base.Commands;

namespace TeamsAllocationManager.Contracts.Desks.Commands;

public class RemoveReservationCommand : ICommand
{
	public Guid Id { get; }
	public string User { get; }

	public RemoveReservationCommand(Guid id, string user)
	{
		Id = id;
		User = user;
	}
}
