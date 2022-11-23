using System;
using System.Collections.Generic;

namespace TeamsAllocationManager.Domain.Models;

public class EquipmentEntity : Entity
{
	public string Name { get; set; } = null!;

	public string AdditionalInfo { get; set; } = null!;
	public int Count { get; set; }
	public IList<EmployeeEquipmentHistoryEntity> EmployeeEquipmentHistory { get; set; } = new List<EmployeeEquipmentHistoryEntity>();
	public IList<EmployeeEquipmentEntity> EmployeeEquipmentReservations { get; set; } = new List<EmployeeEquipmentEntity>();
	public IList<RoomEquipmentEntity> RoomEquipmentReservations { get; set; } = new List<RoomEquipmentEntity>();

	public void ReserveEquipment(EmployeeEquipmentEntity employeeEquipment, DateTime dateFrom)
	{
		EmployeeEquipmentReservations.Add(employeeEquipment);
		EmployeeEquipmentHistory.Add(EmployeeEquipmentHistoryEntity.NewHistoryEntryFromEmployeeEquipment(employeeEquipment, dateFrom));
	}

}
