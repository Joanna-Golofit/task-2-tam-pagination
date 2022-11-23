using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Desks.Commands;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Common;
using TeamsAllocationManager.Dtos.Desk;
using TeamsAllocationManager.Infrastructure.Exceptions;
using TeamsAllocationManager.Infrastructure.Services.EmailFormatters;
using TeamsAllocationManager.Infrastructure.Services.Interfaces;

namespace TeamsAllocationManager.Infrastructure.Handlers.Desk;

public class SetHotDeskHandler : IAsyncCommandHandler<SetHotDeskCommand, DeskForRoomDetailsDto>
{
	private readonly IDesksRepository _desksRepository;
	private readonly IMailComposer<HotDeskRemovedMailFormatter> _mailComposer;
	private readonly IMailSenderService _mailSenderService;
	private readonly IMapper _mapper;

	public SetHotDeskHandler(IDesksRepository desksRepository,
		IMailComposer<HotDeskRemovedMailFormatter> mailComposer,
		IMailSenderService mailSenderService,
		IMapper mapper)
	{
		_desksRepository = desksRepository;
		_mailComposer = mailComposer;
		_mailSenderService = mailSenderService;
		_mapper = mapper;
	}

	public async Task<DeskForRoomDetailsDto> HandleAsync(SetHotDeskCommand command, CancellationToken cancellationToken = default)
	{
		DeskEntity? desk = await _desksRepository.GetDeskWithLocationAndHistoryAndReservation(command.DeskId);

		if (desk == null)
		{
			throw new EntityNotFoundException<DeskEntity>(command.DeskId);
		}

		if (!desk.IsEnabled)
		{
			throw new DeskException(ExceptionMessage.GetMessage(ExceptionMessage.HotDesks_CannotSetDisabledDesk))
			{
				TranslationKey = ExceptionMessage.HotDesks_CannotSetDisabledDesk
			};
		}

		var mails = new List<MailDto>();
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
						desk.Room.Floor.Building.Name,
						desk.Room.Name,
						r.ReservationStart.Date,
						r.ReservationEnd!.Value.Date // HotDesk reservations always have ReservationEnd date set
					}
				)));
		}
			
		desk.SetHotDeskStatus(command.IsHotDesk);
		if (mails.Count > 0)
		{
			await _mailSenderService.SendMails(mails);
		}

		await _desksRepository.UpdateAsync(desk);

		var deskForRoomDetailsDto = _mapper.Map<DeskForRoomDetailsDto>(desk);

		deskForRoomDetailsDto.DeskHistory = deskForRoomDetailsDto.DeskHistory.Where(d => d.Updated > DateTime.Now.AddDays(-7)).ToList();

		return deskForRoomDetailsDto;
	}
}
