using System;
using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Dtos.Desk;

namespace TeamsAllocationManager.Contracts.Desks.Commands;

public class ToggleDeskIsEnabledCommand : ICommand
{
	public IEnumerable<Guid> DesksIdsToToggleEnable { get; }

	public ToggleDeskIsEnabledCommand(ToggleDeskIsEnabledDto dto)
	{
		DesksIdsToToggleEnable = dto.DesksIds;
	}
}
