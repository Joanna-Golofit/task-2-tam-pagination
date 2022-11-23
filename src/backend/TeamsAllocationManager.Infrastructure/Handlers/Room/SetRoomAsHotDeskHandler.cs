using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Room.Commands;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Common;
using TeamsAllocationManager.Infrastructure.Exceptions;
using TeamsAllocationManager.Infrastructure.Services.EmailFormatters;
using TeamsAllocationManager.Infrastructure.Services.Interfaces;

namespace TeamsAllocationManager.Infrastructure.Handlers.Room;

public class SetRoomAsHotDeskHandler : IAsyncCommandHandler<SetRoomAsHotDeskCommand>
{
	private readonly ApplicationDbContext _applicationDbContext;
	private readonly IMailComposer<HotDeskRemovedMailFormatter> _mailComposer;
	private readonly IMailSenderService _mailSenderService;
	private readonly IRoomRepository _roomRepository;

	public SetRoomAsHotDeskHandler(
		ApplicationDbContext applicationDbContext,
		IMailComposer<HotDeskRemovedMailFormatter> mailComposer,
		IMailSenderService mailSenderService,
		IRoomRepository roomRepository)
	{
		_applicationDbContext = applicationDbContext;
		_mailComposer = mailComposer;
		_mailSenderService = mailSenderService;
		_roomRepository = roomRepository;
	}

	public async Task HandleAsync(SetRoomAsHotDeskCommand command, CancellationToken cancellationToken = default)
	{
		var room = await _roomRepository.GetRoomWithDesksAndHistoryAndReservationsAndLocation(command.RoomId);
		if (room == null)
		{
			throw new EntityNotFoundException<RoomEntity>(command.RoomId);
		}

		var desks = room.Desks.ToList();
		var mails = new List<MailDto>();
		desks.ForEach(desk =>
		{
			if (desk.IsHotDesk && !command.IsHotDesk)
			{
				desk.DeskReservations
					.Where(r => !r.IsSchedule)
					.ToList()
					.ForEach(r => mails.AddRange(_mailComposer.Compose
					(
						r.Employee.Email,
						null,
						new object[] {
							desk.Number.ToString(),
							room.Floor.Building.Name,
							room.Name,
							r.ReservationStart.Date,
							r.ReservationEnd!.Value.Date // HotDesk reservations always have EndDate date set
						}
					)));
			}
			desk.SetHotDeskStatus(command.IsHotDesk);
		});

		if (mails.Count > 0)
		{
			await _mailSenderService.SendMails(mails);
		}
			
		await _applicationDbContext.SaveChangesAsync(cancellationToken);
	}
}
