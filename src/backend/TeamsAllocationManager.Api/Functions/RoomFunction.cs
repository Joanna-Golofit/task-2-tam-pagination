using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base;
using TeamsAllocationManager.Contracts.Desks.Commands;
using TeamsAllocationManager.Contracts.Desks.Queries;
using TeamsAllocationManager.Contracts.Employee.Queries;
using TeamsAllocationManager.Contracts.EntityQueries;
using TeamsAllocationManager.Contracts.Room.Commands;
using TeamsAllocationManager.Contracts.Room.Queries;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Desk;
using TeamsAllocationManager.Dtos.Room;

namespace TeamsAllocationManager.Api.Functions;

public class RoomFunction : FunctionBase
{
	private readonly IEnumerable<IEntityQuery> _entityQueries;

	public RoomFunction(IDispatcher dispatcher, IEnumerable<IEntityQuery> entityQueries) : base(dispatcher)
	{
		_entityQueries = entityQueries;
	}

	[FunctionName("RoomFunction")]
	public override async Task<IActionResult> RunAsync(
		[HttpTrigger(AuthorizationLevel.Anonymous, Route = "Room/{*path}")]
		HttpRequest req, string? path, ILogger log)
		=> await base.RunAsync(req, path, log);

	[HttpGet]
	public async Task<RoomsDto> GetAllRooms()
		=> await _dispatcher.DispatchAsync<GetAllRoomsQuery, RoomsDto>(new GetAllRoomsQuery(_entityQueries));

	[HttpGet("Filter")]
	public async Task<PagedRoomsDto> GetFilteredRooms([FromQuery] RoomsQueryFilterDto query)
		=> await _dispatcher.DispatchAsync<GetFilteredRoomsQuery, PagedRoomsDto>(
			new GetFilteredRoomsQuery(_entityQueries));

	[HttpGet("{id}")]
	public async Task<RoomDetailsDto> GetRoom(Guid id)
		=> await _dispatcher.DispatchAsync<GetRoomDetailsQuery, RoomDetailsDto>(
			new GetRoomDetailsQuery(_entityQueries, id));

	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpGet("{id}/GetRoomDetailsForProject")]
	public async Task<RoomForProjectDto> GetRoomDetailsForProject(Guid id)
		=> await _dispatcher.DispatchAsync<GetRoomDetailsForProjectQuery, RoomForProjectDto>(
			new GetRoomDetailsForProjectQuery(_entityQueries, id));

	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpPut("{roomId}/RemoveTeamFromRoom/{projectId}")]
	public async Task RemoveTeamFromRoom(Guid roomId, Guid projectId)
		=> await _dispatcher.DispatchAsync(new RemoveTeamFromRoomCommand(roomId, projectId));

	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpDelete("{roomId}/DeleteDesks")]
	public async Task<IActionResult> DeleteDesks(Guid roomId, IEnumerable<Guid> deskIdsToDelete)
		=> await _dispatcher.DispatchAsync<DeleteDesksFromRoomCommand, bool>(
			new DeleteDesksFromRoomCommand(roomId, deskIdsToDelete))
			? new OkObjectResult(new {roomId, deskIds = deskIdsToDelete}) as IActionResult
			: new NotFoundObjectResult(new {roomId, deskIds = deskIdsToDelete});

	// TODO: not used?
	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpPut("AllocateDesks")]
	public async Task<IActionResult> AllocateDesks(AllocateDesksDto dto)
		=> await _dispatcher.DispatchAsync<AllocateDesksCommand, bool>(new AllocateDesksCommand(dto))
			? new OkObjectResult(dto) as IActionResult
			: new NotFoundObjectResult(dto);

	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpPost("AddDesks")]
	public async Task<IActionResult> AddDesks(AddDesksDto dto)
		=> await _dispatcher.DispatchAsync<AddDesksCommand, bool>(new AddDesksCommand(dto))
			? new OkObjectResult(dto) as IActionResult
			: new NotFoundObjectResult(dto);

	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpPut("ToggleDesksIsEnabled")]
	public async Task ToggleDeskIsEnabled(ToggleDeskIsEnabledDto dto)
		=> await _dispatcher.DispatchAsync(new ToggleDeskIsEnabledCommand(dto));

	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpPost("ReserveDesk")]
	public async Task<Guid> ReserveDesk(NewDeskReservationDto newDeskReservation)
	{
		newDeskReservation.CreatedBy = CurrentUsername;
		return await _dispatcher.DispatchAsync<ReserveDeskCommand, Guid>(new ReserveDeskCommand(newDeskReservation));
	}

	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpPost("ReleaseDeskEmployee")]
	public async Task ReleaseDeskEmployee(ReleaseDeskEmployeeDto dto)
		=> await _dispatcher.DispatchAsync(new ReleaseDeskEmployeeCommand(dto.DeskId, dto.EmployeeId));

	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpPost("ReleaseEmployeesDesks")]
	public async Task ReleasesEmployeeDesks(IEnumerable<Guid> employeesToRelease)
		=> await _dispatcher.DispatchAsync(new ReleaseEmployeesDesksCommand(employeesToRelease));

	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpPut("UpdateReservation")]
	public async Task UpdateReservation(UpdateReservationCommand command) => await _dispatcher.DispatchAsync(command);

	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpGet("GetActiveReservationsForEmployee/{employeeId}")]
	public async Task<IEnumerable<ScheduledDeskReservationInfoDto>> GetActiveReservationsForEmployee(Guid employeeId) =>
		await _dispatcher
			.DispatchAsync<GetActiveDeskReservationsForEmployeeQuery, IEnumerable<ScheduledDeskReservationInfoDto>>(
				new GetActiveDeskReservationsForEmployeeQuery(employeeId));

	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpGet("GetActiveReservationsForDesk/{deskId}")]
	public async Task<IEnumerable<ScheduledDeskReservationInfoDto>> GetActiveReservationsForDesk(Guid deskId) =>
		await _dispatcher
			.DispatchAsync<GetActiveReservationsForDeskQuery, IEnumerable<ScheduledDeskReservationInfoDto>>(
				new GetActiveReservationsForDeskQuery(deskId));

	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpGet("{roomId}/GetEmployeesForRoomDetails/{projectId}")]
	public async Task<IEnumerable<EmployeeForRoomDetailsDto>> GetEmployeesForRoomDetails(Guid projectId, Guid roomId)
		=> await _dispatcher.DispatchAsync<GetEmployeesForRoomDetailsQuery, IEnumerable<EmployeeForRoomDetailsDto>>(
			new GetEmployeesForRoomDetailsQuery(projectId, roomId));

	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpPut("SetHotDesk")]
	public async Task<DeskForRoomDetailsDto> SetHotDesk(SetHotDeskDto dto)
		=> await _dispatcher.DispatchAsync<SetHotDeskCommand, DeskForRoomDetailsDto>(
			new SetHotDeskCommand(dto.DeskId, dto.RoomId, dto.IsHotDesk));

	[OnlyForRoles(RoleEntity.TeamLeader)]
	[HttpPut("SetRoomAsHotDesk")]
	public async Task SetRoomAsHotDesk(SetRoomAsHotDeskDto dto)
		=> await _dispatcher.DispatchAsync<SetRoomAsHotDeskCommand>(
			new SetRoomAsHotDeskCommand(dto.RoomId, dto.IsHotDesk));
}