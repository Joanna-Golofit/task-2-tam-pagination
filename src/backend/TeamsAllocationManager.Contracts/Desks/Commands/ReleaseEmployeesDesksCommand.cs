using System;
using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Commands;

namespace TeamsAllocationManager.Contracts.Desks.Commands;

public class ReleaseEmployeesDesksCommand : ICommand
{
	public IEnumerable<Guid> EmployeesToRelease { get; }

	public ReleaseEmployeesDesksCommand(IEnumerable<Guid> employeesToRelease)
	{
		EmployeesToRelease = employeesToRelease;
	}
}
