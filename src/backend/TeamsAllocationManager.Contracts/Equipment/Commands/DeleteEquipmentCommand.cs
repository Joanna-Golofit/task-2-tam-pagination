using System;
using TeamsAllocationManager.Contracts.Base.Commands;

namespace TeamsAllocationManager.Contracts.Equipment.Commands;

public class DeleteEquipmentCommand : ICommand
{
	public Guid CompanyId { get; }

	public DeleteEquipmentCommand(Guid companyId)
	{
		CompanyId = companyId;
	}
}
