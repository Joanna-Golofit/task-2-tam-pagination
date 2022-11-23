using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using TeamsAllocationManager.Contracts.Base;
using TeamsAllocationManager.Contracts.Desks.Commands;
using TeamsAllocationManager.Contracts.Desks.Queries;
using TeamsAllocationManager.Contracts.Employee.Queries;
using TeamsAllocationManager.Dtos.Desk;
using TeamsAllocationManager.Dtos.Room;

namespace TeamsAllocationManager.Api.Functions;

public class HotDeskFunction : FunctionBase
{
	private const string ReservationMaintenanceSchedule = "0 0 2 * * *"; // every day at 2:00 AM
	private const string ReservationReminderSchedule = "0 0 2 * * *"; // every day at 2:00 AM

	public HotDeskFunction(IDispatcher dispatcher) : base(dispatcher)
	{
	}

	[FunctionName("HotDeskFunction")]
	public override async Task<IActionResult> RunAsync(
		[HttpTrigger(AuthorizationLevel.Anonymous, Route = "HotDesk/{*path}")]
		HttpRequest req, string? path, ILogger log)
		=> await base.RunAsync(req, path, log);

	[HttpGet]
	public async Task<HotDeskRoomsDto> GetHotDeskRooms([FromQuery] string? startDate, [FromQuery] string? endDate)
	{
		var start = DateTime.TryParse(startDate, out DateTime startResult) ? startResult : DateTime.Today;
		var end = DateTime.TryParse(endDate, out DateTime endResult) ? endResult : DateTime.MaxValue;
		return await _dispatcher.DispatchAsync<GetHotDesksQuery, HotDeskRoomsDto>(new GetHotDesksQuery(start, end));
	}

	[HttpGet("Filter")]
	public async Task<PagedHotDeskRoomsDto> GetHotDeskFilteredRooms([FromQuery] GetHotDesksQueryDto hotDesksQueryDto)
		=> await _dispatcher.DispatchAsync<GetFilteredHotDesksQuery, PagedHotDeskRoomsDto>(new GetFilteredHotDesksQuery());

	[HttpGet("GetActiveReservationsForEmployee/{employeeId}")]
	public async Task<IEnumerable<ReservationInfoDto>> GetActiveReservationsForEmployee(Guid employeeId)
		=> await _dispatcher
			.DispatchAsync<GetActiveHotDeskReservationsForEmployeeQuery, IEnumerable<ReservationInfoDto>>(
				new GetActiveHotDeskReservationsForEmployeeQuery(employeeId));

	[HttpGet("GetActiveReservationsForDesk/{deskId}")]
	public async Task<IEnumerable<ReservationInfoDto>> GetActiveReservationsForDesk(Guid deskId)
		=> await _dispatcher.DispatchAsync<GetActiveReservationsForHotDeskQuery, IEnumerable<ReservationInfoDto>>(
			new GetActiveReservationsForHotDeskQuery(deskId));

	[HttpPost]
	public async Task<Guid> CreateNewReservation(NewHotDeskReservationDto newHotDeskReservation)
	{
		newHotDeskReservation.CreatedBy = CurrentUsername;
		return await _dispatcher.DispatchAsync<ReserveHotDeskCommand, Guid>(
			new ReserveHotDeskCommand(newHotDeskReservation));
	}

	[HttpDelete("/{reservationId}")]
	public async Task DeleteReservation(Guid reservationId)
		=> await _dispatcher.DispatchAsync(new RemoveReservationCommand(reservationId, CurrentUsername));

	[FunctionName("ReservationMaintenance")]
	public async Task ReservationMaintenance([TimerTrigger(ReservationMaintenanceSchedule)] TimerInfo timer,
		ILogger logger)
	{
		await RunJob("HotDeskMaintenance", logger,
			async () => { await _dispatcher.DispatchAsync(new ReservationHistoryAndCleanerCommand()); });
	}

	[FunctionName("ReservationReminder")]
	public async Task ReservationReminder([TimerTrigger(ReservationReminderSchedule)] TimerInfo timer, ILogger logger)
	{
		await RunJob("ReservationReminder", logger,
			async () => { await _dispatcher.DispatchAsync(new ReservationReminderCommand()); });
	}

	private async Task RunJob(string actionName, ILogger logger, Func<Task> jobAction)
	{
		using var scope = logger.BeginScope(new Dictionary<string, object>
		{
			["AppVersion"] = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "-",
			["Action"] = actionName,
		});

		try
		{
			logger.LogInformation($"{actionName} action started.");

			await jobAction();
		}
		catch (Exception ex)
		{
			LogException(logger, ex);
			throw;
		}
	}
}