using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Desks.Commands;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Infrastructure.Exceptions;
using TeamsAllocationManager.Infrastructure.Services.EmailFormatters;
using TeamsAllocationManager.Infrastructure.Services.Interfaces;

namespace TeamsAllocationManager.Infrastructure.Handlers.Desk;

public class RemoveReservationHandler : IAsyncCommandHandler<RemoveReservationCommand>
{
	private readonly IDeskReservationsRepository _deskReservationsRepository;
	private readonly IMailComposer<HotDeskReservationRemovedMailFormatter> _mailComposer;
	private readonly IMailSenderService _mailSenderService;

	public RemoveReservationHandler(IDeskReservationsRepository deskReservationsRepository,
		IMailComposer<HotDeskReservationRemovedMailFormatter> mailComposer,
		IMailSenderService mailSenderService)
	{
		_deskReservationsRepository = deskReservationsRepository;
		_mailComposer = mailComposer;
		_mailSenderService = mailSenderService;
	}

	public async Task HandleAsync(RemoveReservationCommand command, CancellationToken cancellationToken = default)
	{
		var reservation = await _deskReservationsRepository.GetReservationWithRoomDetails(command.Id);
			
		if (reservation == null)
		{
			throw new EntityNotFoundException<DeskReservationEntity>(command.Id);
		}
		if (!reservation.CanBeDeletedBy(command.User))
		{
			throw new HotDeskReservationRemovalException(ExceptionMessage.GetMessage(ExceptionMessage.HotDesks_CannotRemoveOtherReservation))
			{
				TranslationKey = ExceptionMessage.HotDesks_CannotRemoveOtherReservation
			};
		}

		var reservationConfirmationEmail = _mailComposer.Compose
		(
			reservation.Employee.Email,
			null,
			new object[] {
				reservation.Desk.Number.ToString(),
				reservation.Desk.Room.Floor.Building.Name,
				reservation.Desk.Room.Name,
				reservation.ReservationStart.Date,
				reservation.ReservationEnd!.Value.Date // HotDesk reservations always have ReservationEnd date set
			}
		);

		await _deskReservationsRepository.RemoveAsync(reservation);
		
		await _mailSenderService.SendMails(reservationConfirmationEmail);
	}
}
