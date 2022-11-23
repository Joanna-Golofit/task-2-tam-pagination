using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Dtos.Project;

namespace TeamsAllocationManager.Contracts.Project.Commands;

public class RemoveEmployeesFromDesksCommand : ICommand
{
	public RemoveEmployeesFromDesksDto Dto { get; }
	public RemoveEmployeesFromDesksCommand(RemoveEmployeesFromDesksDto dto)
	{
		Dto = dto;
	}
}
