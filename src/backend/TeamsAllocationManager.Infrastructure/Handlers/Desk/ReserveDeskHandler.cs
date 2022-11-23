using System;
using System.Linq;
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

public class ReserveDeskHandler : IAsyncCommandHandler<ReserveDeskCommand, Guid>
{
	private readonly IEmployeesRepository _employeesRepository;
	private readonly IDesksRepository _desksRepository;
	private readonly IMailComposer<SendMailInfoNewEmployeeAssignedToSameDeskMailFormatter> _mailComposer;
	private readonly IMailSenderService _mailSenderService;

	public ReserveDeskHandler(
		IEmployeesRepository employeesRepository,
		IDesksRepository desksRepository,
		IMailComposer<SendMailInfoNewEmployeeAssignedToSameDeskMailFormatter> mailComposer,
		IMailSenderService mailSenderService)
	{
		_employeesRepository = employeesRepository;
		_desksRepository = desksRepository;
		_mailComposer = mailComposer;
		_mailSenderService = mailSenderService;
	}

	public async Task<Guid> HandleAsync(ReserveDeskCommand command, CancellationToken cancellationToken = default)
	{
		var reservationDto = command.Dto;

		var desk = await _desksRepository.GetDeskWithHistoryAndBuildings(reservationDto.DeskId);

		if (desk == null)
		{
			throw new EntityNotFoundException<DeskEntity>(reservationDto.DeskId);
		}

		if (desk.IsHotDesk)
		{
			throw new DeskReservationException(
				ExceptionMessage.GetMessage(ExceptionMessage.HotDesks_CannotCreateScheduledReservation))
			{
				TranslationKey = ExceptionMessage.HotDesks_CannotCreateScheduledReservation
			};
		}

		if (!desk.AvailableInWeekdays(reservationDto.ScheduledWeekdays))
		{
			throw new DeskReservationException(
				ExceptionMessage.GetMessage(ExceptionMessage.HotDesks_ReservationExistsForDates))
			{
				TranslationKey = ExceptionMessage.HotDesks_ReservationExistsForDates
			};
		}

		var employee = await _employeesRepository.GetEmployee(reservationDto.EmployeeId);

		if (employee == null)
		{
			throw new EntityNotFoundException<EmployeeEntity>(nameof(EmployeeEntity.Id), reservationDto.EmployeeId);
		}

		var employeeWithReservation = await GetEmployeeWithExistingReservation(reservationDto.CreatedBy!);

		var reservation = DeskReservationEntity.NewDeskReservation(reservationDto.ReservationStart ?? DateTime.Now,
			reservationDto.ScheduledWeekdays, desk, employee, employeeWithReservation);

		desk.ReserveDesk(reservation);

		await _desksRepository.UpdateAsync(desk);

		await SendMailsAboutNewEmployeesAssignedToDesk(desk, employee);

		return reservation.Id;
	}

	private async Task SendMailsAboutNewEmployeesAssignedToDesk(DeskEntity desk, EmployeeEntity employee)
	{
		if (desk.DeskReservations.Count > 1)
		{
			var reservationConfirmationEmail = _mailComposer.Compose
			(
				desk.DeskReservations.Select(dr => dr.Employee.Email).Where(e => e != employee.Email).ToArray(),
				null,
				new object[]
				{
					desk.Number.ToString(),
					desk.Room.Floor.Building.Name,
					desk.Room.Name
				}
			);

			await _mailSenderService.SendMails(reservationConfirmationEmail);
		}
	}

	private async Task<EmployeeEntity?> GetEmployeeWithExistingReservation(string existingEmployeeReservationEmail)
	{
		var employeeWithReservation =
			await _employeesRepository.GetEmployeeWithReservations(existingEmployeeReservationEmail);

		if (employeeWithReservation == null)
		{
			throw new EntityNotFoundException<EmployeeEntity>(nameof(EmployeeEntity.Id),
				existingEmployeeReservationEmail);
		}

		return employeeWithReservation;
	}
}