using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Desks.Commands;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Infrastructure.Handlers.Desk;

public class DeleteDesksFromRoomHandler : IAsyncCommandHandler<DeleteDesksFromRoomCommand, bool>
{
	private readonly IDesksRepository _desksRepository;

	public DeleteDesksFromRoomHandler(IDesksRepository desksRepository)
	{
		_desksRepository = desksRepository;
	}

	public async Task<bool> HandleAsync(DeleteDesksFromRoomCommand command, CancellationToken cancellationToken = default)
	{
		var desks = await _desksRepository.GetDeskForRoom(command.RoomId, command.DeskIdsToDelete);

		if (desks.Count() != command.DeskIdsToDelete.Count())
		{
			return false;
		}

		await _desksRepository.RemoveRangeAsync(desks);

		return true;
	}
}
