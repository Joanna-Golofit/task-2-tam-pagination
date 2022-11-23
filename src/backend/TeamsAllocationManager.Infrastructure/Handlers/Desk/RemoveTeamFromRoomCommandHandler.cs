using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Desks.Commands;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Infrastructure.Handlers.Desk;

public class RemoveTeamFromRoomCommandHandler : IAsyncCommandHandler<RemoveTeamFromRoomCommand>
{
	private readonly IDesksRepository _desksRepository;

	public RemoveTeamFromRoomCommandHandler(IDesksRepository desksRepository)
	{
		_desksRepository = desksRepository;
	}

	public async Task HandleAsync(RemoveTeamFromRoomCommand command, CancellationToken cancellationToken = default)
	{
		var desksToRelease = await _desksRepository.GetDesksToRelease(command.RoomId, command.ProjectId);

		foreach (DeskEntity desk in desksToRelease)
		{
			var teamEmployees = desk.DeskReservations
			                        .Where(dr => dr.IsSchedule && dr.Employee.Projects.Any(p => p.ProjectId == command.ProjectId))
			                        .Select(dr => dr.EmployeeId).ToList();

			teamEmployees.ForEach(employee => desk.ReleaseDesk(employee));
		}

		await _desksRepository.UpdateRangeAsync(desksToRelease);
	}
}
