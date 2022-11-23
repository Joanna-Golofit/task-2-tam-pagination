using System;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Dtos.Desk;

namespace TeamsAllocationManager.Contracts.Desks.Commands;

public class ReserveHotDeskCommand : ICommand<Guid>
{
	public NewHotDeskReservationDto Dto { get; }

	public ReserveHotDeskCommand(NewHotDeskReservationDto dto)
	{
		Dto = dto;
	}
}
