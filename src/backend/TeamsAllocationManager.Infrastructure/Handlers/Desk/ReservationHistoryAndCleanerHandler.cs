using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Desks.Commands;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Infrastructure.Handlers.Desk;

public class ReservationHistoryAndCleanerHandler : IAsyncCommandHandler<ReservationHistoryAndCleanerCommand>
{
	private readonly IDeskReservationsRepository _deskReservationsRepository;

	public ReservationHistoryAndCleanerHandler(IDeskReservationsRepository deskReservationsRepository)
	{
		_deskReservationsRepository = deskReservationsRepository;
	}

	public async Task HandleAsync(ReservationHistoryAndCleanerCommand command, CancellationToken cancellationToken = default)
	{
		var oldReservations = await _deskReservationsRepository.GetOldReservations();

		foreach (DeskReservationEntity deskReservationEntity in oldReservations)
		{
			deskReservationEntity.Employee.CreateReservationHistory(deskReservationEntity);
		}

		await _deskReservationsRepository.RemoveRangeAsync(oldReservations);
	}
}
