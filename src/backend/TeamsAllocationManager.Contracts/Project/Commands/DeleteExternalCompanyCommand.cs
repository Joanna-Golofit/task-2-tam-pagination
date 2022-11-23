using System;
using TeamsAllocationManager.Contracts.Base.Commands;

namespace TeamsAllocationManager.Contracts.Project.Commands;

public class DeleteExternalCompanyCommand : ICommand
{
	public Guid CompanyId { get; }

	public DeleteExternalCompanyCommand(Guid companyId)
	{
		CompanyId = companyId;
	}
}
