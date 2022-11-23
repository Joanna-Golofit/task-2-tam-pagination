using System;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Dtos.Enums;
using TeamsAllocationManager.Dtos.Project;

namespace TeamsAllocationManager.Contracts.Project.Commands;

public class EditExternalCompanyCommand : ICommand
{
	public EditExternalCompanyDto Dto { get; }
	public Guid CompanyId { get; }

	public EditExternalCompanyCommand(Guid companyId, EditExternalCompanyDto dto)
	{
		CompanyId = companyId;
		Dto = dto;
	}
}
