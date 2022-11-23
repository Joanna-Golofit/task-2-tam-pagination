using System;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Dtos.Desk;

namespace TeamsAllocationManager.Contracts.Desks.Commands
{
	public class ReserveDeskCommand : ICommand<Guid>
	{
		public NewDeskReservationDto Dto { get; }

		public ReserveDeskCommand(NewDeskReservationDto dto)
		{
			Dto = dto;
		}
	}
}
