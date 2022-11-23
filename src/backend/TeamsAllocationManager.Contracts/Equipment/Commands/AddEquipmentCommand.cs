using System;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Dtos.Equipment;

namespace TeamsAllocationManager.Contracts.Equipment.Commands;

public class AddEquipmentCommand : ICommand<Guid>
{
	public AddEquipmentDto Dto { get; }

	public AddEquipmentCommand(AddEquipmentDto dto)
	{
		Dto = dto;
	}
}
