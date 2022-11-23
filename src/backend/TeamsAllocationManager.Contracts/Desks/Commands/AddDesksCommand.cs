using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Dtos.Desk;

namespace TeamsAllocationManager.Contracts.Desks.Commands;

public class AddDesksCommand : ICommand<bool>
{
	public AddDesksDto AddDesksDto { get; }
	public AddDesksCommand(AddDesksDto addDesksDto)
	{
		AddDesksDto = addDesksDto;
	}
}
