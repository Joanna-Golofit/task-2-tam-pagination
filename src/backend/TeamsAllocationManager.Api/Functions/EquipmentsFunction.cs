using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base;
using TeamsAllocationManager.Contracts.Equipment.Commands;
using TeamsAllocationManager.Contracts.Equipment.Queries;
using TeamsAllocationManager.Contracts.Project.Queries;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Equipment;

namespace TeamsAllocationManager.Api.Functions;

public class EquipmentsFunction : FunctionBase
{
	public EquipmentsFunction(IDispatcher dispatcher) : base(dispatcher)
	{
	}

	[FunctionName("EquipmentsFunction")]
	public override async Task<IActionResult> RunAsync(
		[HttpTrigger(AuthorizationLevel.Anonymous, Route = "Equipments/{*path}")]
		HttpRequest req, string? path, ILogger log)
		=> await base.RunAsync(req, path, log);

	[OnlyForRoles(RoleEntity.Admin)]
	[HttpGet("{equipmentId}")]
	public async Task<EquipmentDetailDto?> GetProjectDetails(Guid equipmentId)
		=> await _dispatcher.DispatchAsync<GetEquipmentDetailQuery, EquipmentDetailDto?>(
			new GetEquipmentDetailQuery(equipmentId));


	[OnlyForRoles(RoleEntity.Admin)]
	[HttpPost("AddEquipment")]
	public async Task<Guid> AddEquipment(AddEquipmentDto addEquipment)
		=> await _dispatcher.DispatchAsync<AddEquipmentCommand, Guid>(new AddEquipmentCommand(addEquipment));

	[OnlyForRoles(RoleEntity.Admin)]
	[HttpGet]
	public async Task<IEnumerable<EquipmentDto>> GetAllEquipments()
		=> await _dispatcher.DispatchAsync<GetAllEquipmentsQuery, IEnumerable<EquipmentDto>>(
			new GetAllEquipmentsQuery());

	[OnlyForRoles(RoleEntity.Admin)]
	[HttpPut("EditEquipment")]
	public async Task<Guid> EditEquipment(EditEquipmentDto editEquipment)
		=> await _dispatcher.DispatchAsync<EditEquipmentCommand, Guid>(new EditEquipmentCommand(editEquipment));

	[OnlyForRoles(RoleEntity.Admin)]
	[HttpPut("ReserveEquipment")]
	public async Task<IList<Guid>> ReserveEquipment(ReservationEquipmentDto reservation)
		=> await _dispatcher.DispatchAsync<ReserveEquipmentCommand, IList<Guid>>(
			new ReserveEquipmentCommand(reservation));

	[OnlyForRoles(RoleEntity.Admin)]
	[HttpDelete("{equipmentId}")]
	public async Task DeleteEquipment(Guid equipmentId)
		=> await _dispatcher.DispatchAsync(new DeleteEquipmentCommand(equipmentId));

	[HttpGet("GetForEmployee")]
	public async Task<IEnumerable<EmployeeEquipmentDetailDto>> GetEquipmentForEmployee(Guid employeeId)
		=> await _dispatcher.DispatchAsync<GetEquipmentsForEmployeeQuery, IEnumerable<EmployeeEquipmentDetailDto>>(
			new GetEquipmentsForEmployeeQuery(employeeId));
}