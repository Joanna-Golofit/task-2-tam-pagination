using System;
using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Dtos.Equipment;

namespace TeamsAllocationManager.Contracts.Equipment.Commands;

public class ReserveEquipmentCommand : ICommand<IList<Guid>>
{
	public ReservationEquipmentDto Dto { get; }

	public ReserveEquipmentCommand(ReservationEquipmentDto dto)
	{
		Dto = dto;
	}
}
