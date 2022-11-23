using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Equipment.Commands;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Equipment;

public class ReserveEquipmentHandler : IAsyncCommandHandler<ReserveEquipmentCommand, IList<Guid>>
{
	private readonly IEmployeesRepository _employeesRepository;
	private readonly IEquipmentRepository _equipmentRepository;
	private readonly IEmployeeEquipmentRepository _employeeEquipmentRepository;

	public ReserveEquipmentHandler(
		IEmployeesRepository employeesRepository,
		IEquipmentRepository equipmentRepository,
		IEmployeeEquipmentRepository employeeEquipmentRepository)
	{
		_employeesRepository = employeesRepository;
		_equipmentRepository = equipmentRepository;
		_employeeEquipmentRepository = employeeEquipmentRepository;
	}

	public async Task<IList<Guid>> HandleAsync(ReserveEquipmentCommand command, CancellationToken cancellationToken = default)
	{
		var equipmentDto = command.Dto;

		var equipment = await _equipmentRepository.GetEquipment(equipmentDto.EquipmentId);

		if (equipment == null)
		{
			throw new EntityNotFoundException<EquipmentEntity>(nameof(EquipmentEntity.Id), equipmentDto.EquipmentId);
		}

		var existingReservations =
			equipment.EmployeeEquipmentReservations.Where(e => e.EquipmentId == equipmentDto.EquipmentId).ToList();

		var employeesToAdd = equipmentDto.EmployeeReservations.Where(employeeDto
			=> existingReservations.All(reservation => reservation.EmployeeId != employeeDto.EmployeeId));


		foreach (var employeeDto in employeesToAdd)
		{
			var employee = await _employeesRepository.GetEmployee(employeeDto.EmployeeId);

			if (employee == null)
			{
				throw new EntityNotFoundException<EmployeeEntity>(nameof(EmployeeEntity.Id), employeeDto);
			}

			var reservation = EmployeeEquipmentEntity.NewEmployeeEquipment(equipment, employee, employeeDto.Count);

			equipment.ReserveEquipment(reservation, equipmentDto.DateFrom);
		}

		var equipmentToUpdate = existingReservations
								.Where(reservation => equipmentDto.EmployeeReservations.Any(emp 
									=> reservation.EmployeeId == emp.EmployeeId && reservation.Count != emp.Count)).ToList();

		foreach (var equipmentEntity in equipmentToUpdate)
		{
			var equipmentEmployeeDto = equipmentDto.EmployeeReservations.First(er => er.EmployeeId == equipmentEntity.EmployeeId);

			if (equipmentEmployeeDto.Count <= 0)
			{
				continue;
			}

			if (equipmentEntity.Employee == null)
			{
				throw new EntityNotFoundException<EmployeeEntity>(nameof(EmployeeEntity.Id), equipmentEntity);
			}

			var reservation = EmployeeEquipmentEntity.NewEmployeeEquipment(equipment, equipmentEntity.Employee, equipmentEmployeeDto.Count);

			equipment.ReserveEquipment(reservation, equipmentDto.DateFrom);
		}

		var reservationsEndingToday = equipmentToUpdate.SelectMany(e
			=> e.Employee.EquipmentHistory.Where(eh => eh.ReservationEnd == null && eh.EquipmentId == equipmentDto.EquipmentId));
		foreach (var history in reservationsEndingToday)
		{
			history.ReservationEnd = DateTime.Now;
		}

		if (equipmentToUpdate.Any())
		{
			await _employeeEquipmentRepository.RemoveRangeAsync(equipmentToUpdate);
		}

		var result = equipment.EmployeeEquipmentReservations.Select(eE => eE.Id).ToList();

		await _equipmentRepository.UpdateAsync(equipment);

		return result;
	}
}
