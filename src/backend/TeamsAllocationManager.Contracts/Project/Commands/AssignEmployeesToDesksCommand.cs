using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Dtos.Enums;
using TeamsAllocationManager.Dtos.Project;

namespace TeamsAllocationManager.Contracts.Project.Commands;

public class AssignEmployeesToDesksCommand : ICommand
{
	public AssignEmployeesToDesksDto Dto { get; }
	public AssignEmployeesToDesksCommand(AssignEmployeesToDesksDto dto)
	{
		Dto = dto;
	}
}
