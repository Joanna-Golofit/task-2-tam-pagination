using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Project.Commands;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Project;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Project;

public class RemoveEmployeesFromDesksHandler : IAsyncCommandHandler<RemoveEmployeesFromDesksCommand>
{
	private readonly IDesksRepository _desksRepository;

	public RemoveEmployeesFromDesksHandler(IDesksRepository desksRepository)
	{
		_desksRepository = desksRepository;
	}

	public async Task HandleAsync(RemoveEmployeesFromDesksCommand command, CancellationToken cancellationToken = default)
	{
		RemoveEmployeesFromDesksDto dto = command.Dto;

		var desks = await _desksRepository.GetDeskForRoom(dto.RoomId);

		if (desks == null)
		{
			throw new EntityNotFoundException<DeskEntity>($"Couldn't find Desks for Room {dto.RoomId}");
		}

		var desksToUpdate = desks.Where(d => d.RoomId == dto.RoomId && dto.DeskIds.Contains(d.Id));

		foreach (DeskEntity desk in desksToUpdate)
		{
			desk.ReleaseDesk();
		}

		await _desksRepository.UpdateRangeAsync(desksToUpdate);
	}
}
