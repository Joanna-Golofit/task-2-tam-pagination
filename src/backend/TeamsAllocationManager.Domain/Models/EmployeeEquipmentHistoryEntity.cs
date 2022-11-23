using System;

namespace TeamsAllocationManager.Domain.Models;

public class EmployeeEquipmentHistoryEntity : Entity
{
	public Guid? EmployeeId { get; set; }
	public EmployeeEntity Employee { get; set; } = null!;
	public DateTime ReservationStart { get; set; }
	public DateTime? ReservationEnd { get; set; }
	public Guid? EquipmentId { get; set; }
	public EquipmentEntity Equipment { get; set; } = null!;
	public int Count { get; set; }

	public static EmployeeEquipmentHistoryEntity NewHistoryEntryFromEmployeeEquipment(
		EmployeeEquipmentEntity equipment, DateTime dateFrom) =>
		new()
		{
			EmployeeId = equipment.EmployeeId,
			EquipmentId = equipment.EquipmentId,
			ReservationStart = dateFrom,
			ReservationEnd = null,
			Count = equipment.Count
		};
}
