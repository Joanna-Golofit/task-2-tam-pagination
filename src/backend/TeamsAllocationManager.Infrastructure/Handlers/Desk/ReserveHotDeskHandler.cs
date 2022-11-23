using System;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Desks.Commands;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Desk;
using TeamsAllocationManager.Infrastructure.Exceptions;
using TeamsAllocationManager.Infrastructure.Services.EmailFormatters;
using TeamsAllocationManager.Infrastructure.Services.Interfaces;

namespace TeamsAllocationManager.Infrastructure.Handlers.Desk;

public class ReserveHotDeskHandler : IAsyncCommandHandler<ReserveHotDeskCommand, Guid>
{
	private readonly IDesksRepository _desksRepository;
	private readonly IDeskReservationsRepository _deskReservationsRepository;
	private readonly IEmployeesRepository _employeesRepository;
	private readonly IMailComposer<HotDeskReservationConfirmationMailFormatter> _mailComposer;
	private readonly IMailSenderService _mailSenderService;

	public ReserveHotDeskHandler(
		IDesksRepository desksRepository,
		IDeskReservationsRepository deskReservationsRepository,
		IEmployeesRepository employeesRepository,
		IMailComposer<HotDeskReservationConfirmationMailFormatter> mailComposer,
		IMailSenderService mailSenderService)
	{
		_desksRepository = desksRepository;
		_deskReservationsRepository = deskReservationsRepository;
		_employeesRepository = employeesRepository;
		_mailComposer = mailComposer;
		_mailSenderService = mailSenderService;
	}

	public async Task<Guid> HandleAsync(ReserveHotDeskCommand command, CancellationToken cancellationToken = default)
	{
		var reservationDto = command.Dto;

		ValidateReservationDates(reservationDto.ReservationStart, reservationDto.ReservationEnd);

		var desk = await GetDesk(reservationDto.DeskId);

		var reservingEmployee = await GetReservingEmployee(reservationDto.ReservingEmployee);

		var employeeWithReservation = await GetEmployeeWithExistingReservation(reservationDto.CreatedBy!);

		ValidateEmployeesUserRoles(reservingEmployee!, employeeWithReservation!);

		await CheckOtherReservationsForDeskAndEmployee(reservingEmployee!, desk!, reservationDto);

		ValidateEndDateForEmployeeRole(employeeWithReservation!, reservationDto.ReservationEnd);

		var reservation = DeskReservationEntity.NewHotDeskReservation(
			reservationDto.ReservationStart,
			reservationDto.ReservationEnd,
			desk!,
			reservingEmployee!,
			employeeWithReservation
		);

		await _deskReservationsRepository.AddAsync(reservation);

		await SendReservationEmail(reservingEmployee!, employeeWithReservation!, desk!, reservationDto);

		return reservation.Id;
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

	private async Task<EmployeeEntity?> GetReservingEmployee(Guid reservingEmployeeId)
	{
		var reservingEmployee = await _employeesRepository.GetEmployeeWithReservations(reservingEmployeeId);

		if (reservingEmployee == null)
		{
			throw new EntityNotFoundException<EmployeeEntity>(nameof(EmployeeEntity.Id), reservingEmployeeId);
		}

		return reservingEmployee;
	}

	private async Task SendReservationEmail(
		EmployeeEntity employee,
		EmployeeEntity createdBy,
		DeskEntity desk,
		NewHotDeskReservationDto reservationDto)
	{
		var reservationConfirmationEmail = _mailComposer.Compose
		(
			employee.Email,
			createdBy.Id != employee.Id ? new[] {createdBy.Email} : null,
			new object[]
			{
				desk.Number.ToString(),
				desk.Room.Floor.Building.Name,
				desk.Room.Name,
				reservationDto.ReservationStart.Date,
				reservationDto.ReservationEnd.Date
			}
		);
		await _mailSenderService.SendMails(reservationConfirmationEmail);
	}

	private static void ValidateEmployeesUserRoles(EmployeeEntity employee, EmployeeEntity createdBy)
	{
		if (employee.Id != createdBy.Id && !createdBy.IsTeamLeaderOrAdmin())
		{
			throw new InvalidUserRoleException(
				ExceptionMessage.GetMessage(ExceptionMessage.InvalidUserRole_OnlyAdminOrTeamLeaderCanReserveForOthers))
			{
				TranslationKey = ExceptionMessage.InvalidUserRole_OnlyAdminOrTeamLeaderCanReserveForOthers
			};
		}
	}

	private static void ValidateReservationDates(DateTime reservationStart, DateTime reservationEnd)
	{
		if (reservationStart.Date < DateTime.Today)
		{
			throw new DeskReservationException(
				ExceptionMessage.GetMessage(ExceptionMessage.HotDesks_CannotReserveForPastDate))
			{
				TranslationKey = ExceptionMessage.HotDesks_CannotReserveForPastDate
			};
		}

		if (reservationEnd < reservationStart)
		{
			throw new DeskReservationException(
				ExceptionMessage.GetMessage(ExceptionMessage.HotDesks_ReservationEndMustBeGraterThanStart))
			{
				TranslationKey = ExceptionMessage.HotDesks_ReservationEndMustBeGraterThanStart
			};
		}
	}

	private async Task<DeskEntity?> GetDesk(Guid deskId)
	{
		var desk = await _desksRepository.GetDesk(deskId);

		if (desk is not {IsHotDesk: true})
		{
			throw new EntityNotFoundException<DeskEntity>(deskId)
			{
				TranslationKey = ExceptionMessage.HotDesks_DeskNotAvailable
			};
		}

		return desk;
	}

	private static void ValidateEndDateForEmployeeRole(EmployeeEntity employee, DateTime endDate)
	{
		if (employee.IsTeamLeaderOrAdmin())
		{
			if (endDate > DateTime.Today.AddDays(31))
			{
				throw new DeskReservationException(
					ExceptionMessage.GetMessage(ExceptionMessage.HotDesks_EndDateLimit30days))
				{
					TranslationKey = ExceptionMessage.HotDesks_EndDateLimit30days
				};
			}
		}
		else
		{
			if (endDate > DateTime.Today.AddDays(8))
			{
				throw new DeskReservationException(
					ExceptionMessage.GetMessage(ExceptionMessage.HotDesks_EndDateLimit7days))
				{
					TranslationKey = ExceptionMessage.HotDesks_EndDateLimit7days
				};
			}
		}
	}

	private async Task CheckOtherReservationsForDeskAndEmployee(
		EmployeeEntity employee,
		DeskEntity desk,
		NewHotDeskReservationDto reservationDto)
	{
		if (!desk.AvailableOnDateRange(reservationDto.ReservationStart, reservationDto.ReservationEnd))
		{
			throw new DeskReservationException(
				ExceptionMessage.GetMessage(ExceptionMessage.HotDesks_ReservationExistsForDates))
			{
				TranslationKey = ExceptionMessage.HotDesks_ReservationExistsForDates
			};
		}

		await ValidateExistingEmployeeReservations(employee, reservationDto);
	}

	private async Task ValidateExistingEmployeeReservations(EmployeeEntity employee,
		NewHotDeskReservationDto reservationDto)
	{
		var employeeReservations = await _deskReservationsRepository.GetHotDeskReservationsForEmployee(employee.Id);
		if (employeeReservations.AnyOnDateRange(reservationDto.ReservationStart, reservationDto.ReservationEnd))
		{
			throw new DeskReservationException(
				ExceptionMessage.GetMessage(ExceptionMessage.HotDesks_EmployeeHasReservation))
			{
				TranslationKey = ExceptionMessage.HotDesks_EmployeeHasReservation
			};
		}
	}
}