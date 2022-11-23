using System;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Dtos.Project;

namespace TeamsAllocationManager.Contracts.Project.Commands;

public class CreateExternalCompanyCommand : ICommand<Guid>
{
	public NewExternalCompanyDto Dto { get; }

	public CreateExternalCompanyCommand(NewExternalCompanyDto dto)
	{
		Dto = dto;
	}
}
