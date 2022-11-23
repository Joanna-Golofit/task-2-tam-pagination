using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Desks.Commands;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Common;
using TeamsAllocationManager.Infrastructure.Services.EmailFormatters;
using TeamsAllocationManager.Infrastructure.Services.Interfaces;

namespace TeamsAllocationManager.Infrastructure.Handlers.Desk;

public class ReservationReminderCommandHandler : IAsyncCommandHandler<ReservationReminderCommand>
{
	private const int NumberOfDaysAheadReservation = 1;

	private readonly IDeskReservationsRepository _deskReservationsRepository;
	private readonly IMailSenderService _mailSender;
	private readonly IMailComposer<HotDeskReservationReminderMailFormatter> _mailComposer;

	public ReservationReminderCommandHandler(
		IDeskReservationsRepository deskReservationsRepository,
		IMailSenderService mailSender,
		IMailComposer<HotDeskReservationReminderMailFormatter> mailComposer)
	{
		_deskReservationsRepository = deskReservationsRepository;
		_mailSender = mailSender;
		_mailComposer = mailComposer;
	}

	public async Task HandleAsync(ReservationReminderCommand command, CancellationToken cancellationToken = default)
	{
		var reservationsToRemind = await _deskReservationsRepository.GetReservationToRemind(NumberOfDaysAheadReservation);

		var reminderEmails = reservationsToRemind.SelectMany(r => GenerateReminder(r));

		await _mailSender.SendMails(reminderEmails);
	}

	private IEnumerable<MailDto> GenerateReminder(DeskReservationEntity reservation) =>
		_mailComposer.Compose(
			reservation.Employee.Email,
			null,
			new object[] {
				reservation.Desk.Number.ToString(),
				reservation.Desk.Room.Floor.Building.Name,
				reservation.Desk.Room.Name,
				reservation.ReservationStart.Date.ToShortDateString()
			}
		);
}
