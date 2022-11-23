using System;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Dtos.Equipment;

namespace TeamsAllocationManager.Contracts.Equipment.Commands;

public class EditEquipmentCommand : ICommand<Guid>
{
	public EditEquipmentDto EditEquipmentDto { get; }

	public EditEquipmentCommand(EditEquipmentDto dto)
	{
		EditEquipmentDto = dto;
	}
}
