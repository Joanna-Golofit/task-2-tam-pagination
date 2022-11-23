using System;
using TeamsAllocationManager.Contracts.Base.Commands;

namespace TeamsAllocationManager.Contracts.Desks.Commands;

public class ReleaseDeskEmployeeCommand : ICommand
{
	public Guid DeskId { get; }

	public Guid EmployeeId { get; }

	public ReleaseDeskEmployeeCommand(Guid deskId, Guid employeeId)
	{
		DeskId = deskId;
		EmployeeId = employeeId;
	}
}
