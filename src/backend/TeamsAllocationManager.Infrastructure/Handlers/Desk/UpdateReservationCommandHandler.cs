using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Desks.Commands;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Desk;

public class UpdateReservationCommandHandler : IAsyncCommandHandler<UpdateReservationCommand>
{
	private readonly IDeskReservationsRepository _deskReservationsRepository;
	private readonly IEmployeesRepository _employeesRepository;

	public UpdateReservationCommandHandler(
		IDeskReservationsRepository deskReservationsRepository,
		IEmployeesRepository employeesRepository) 
	{
		_deskReservationsRepository = deskReservationsRepository;
		_employeesRepository = employeesRepository;
	}

	public async Task HandleAsync(UpdateReservationCommand command, CancellationToken cancellationToken = default)
	{
		var reservation = await _deskReservationsRepository.GetReservation(command.ReservationId);

		if (reservation == null)
		{
			throw new EntityNotFoundException<DeskReservationEntity>(command.ReservationId);
		}

		var desk = reservation.Desk;

		if (!command.ScheduledWeekdays.Any())
		{
			throw new DeskReservationException(ExceptionMessage.GetMessage(ExceptionMessage.HotDesks_ReservationCannotBeEmpty))
			{
				TranslationKey = ExceptionMessage.HotDesks_ReservationCannotBeEmpty
			};
		}

		if (command.ScheduledWeekdays.Any(weekday => weekday > DayOfWeek.Saturday || weekday < DayOfWeek.Sunday))
		{
			throw new InvalidArgumentException(ExceptionMessage.GetMessage(ExceptionMessage.InvalidArgument_IncorrectWeekdays))
			{
				TranslationKey = ExceptionMessage.InvalidArgument_IncorrectWeekdays
			};
		}

		// This is small workaround for validation. We remove current reservation from desk, we check if incomming days are free and can be reserved, then we add reservation back to desk.
		desk.DeskReservations.Remove(reservation);

		if (!desk.AvailableInWeekdays(command.ScheduledWeekdays))
		{
			throw new DeskReservationException(ExceptionMessage.GetMessage(ExceptionMessage.HotDesks_ReservationExistsForDates))
			{
				TranslationKey = ExceptionMessage.HotDesks_ReservationExistsForDates
			};
		}

		reservation.ScheduledWeekdays = command.ScheduledWeekdays;
		desk.DeskReservations.Add(reservation);

		if (command.EmployeeId != reservation.EmployeeId)
		{
			desk.ReleaseDesk(reservation.EmployeeId);
			var newEmployee = await _employeesRepository.GetEmployee(command.EmployeeId);

			if (newEmployee == null)
			{
				throw new EntityNotFoundException<EmployeeEntity>(nameof(EmployeeEntity.Id), command.EmployeeId);
			}

			desk.ReserveDesk(DeskReservationEntity.NewDeskReservation(DateTime.Now, command.ScheduledWeekdays, desk, newEmployee));
		}

		await _deskReservationsRepository.UpdateAsync(reservation);
	}
}
