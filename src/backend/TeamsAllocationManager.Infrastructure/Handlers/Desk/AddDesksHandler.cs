using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Desks.Commands;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Desk;

namespace TeamsAllocationManager.Infrastructure.Handlers.Desk;

public class AddDesksHandler : IAsyncCommandHandler<AddDesksCommand, bool>
{
	private readonly IRoomRepository _roomRepository;
	private readonly IDesksRepository _desksRepository;

	public AddDesksHandler(IRoomRepository roomRepository, IDesksRepository desksRepository)
	{
		_roomRepository = roomRepository;
		_desksRepository = desksRepository;
	}

	public async Task<bool> HandleAsync(AddDesksCommand command, CancellationToken cancellationToken = default)
	{
		AddDesksDto? addDesksDto = command.AddDesksDto;
		if (addDesksDto.FirstDeskNumber < 1 || addDesksDto.NumberOfDesks < 1)
		{
			return false;
		}

		var roomEntity = await _roomRepository.GetRoomsWithDesks(addDesksDto.RoomId);

		if (roomEntity == null)
		{
			return false;
		}

		var existsingNumbers = roomEntity.Desks.Select(d => d.Number).ToList();

		int currentNumber = addDesksDto.FirstDeskNumber;

		var desks = new List<DeskEntity>();

		for (int i = 0; i < addDesksDto.NumberOfDesks; i++)
		{
			while (existsingNumbers.Contains(currentNumber))
			{
				currentNumber++;
			}

			var desk = new DeskEntity { Number = currentNumber++, IsEnabled = true, RoomId = roomEntity.Id};

			desks.Add(desk);
		}

		await _desksRepository.AddRangeAsync(desks);

		return true;
	}
}
