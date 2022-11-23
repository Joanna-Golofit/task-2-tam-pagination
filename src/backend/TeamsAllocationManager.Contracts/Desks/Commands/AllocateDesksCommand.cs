using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Dtos.Desk;

namespace TeamsAllocationManager.Contracts.Desks.Commands;

public class AllocateDesksCommand : ICommand<bool>
{
	public AllocateDesksDto Dto { get; }
	public AllocateDesksCommand(AllocateDesksDto dto)
	{
		Dto = dto;
	}
}
