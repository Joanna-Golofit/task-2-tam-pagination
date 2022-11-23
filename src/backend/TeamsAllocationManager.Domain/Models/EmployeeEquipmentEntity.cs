using System;

namespace TeamsAllocationManager.Domain.Models;

public class EmployeeEquipmentEntity : Entity
{
	public Guid EmployeeId { get; set; }
	public EmployeeEntity Employee { get; set; } = null!;
	public Guid EquipmentId { get; set; }
	public EquipmentEntity Equipment { get; set; } = null!;
	public int Count { get; set; }

	public static EmployeeEquipmentEntity NewEmployeeEquipment(EquipmentEntity equipment, EmployeeEntity employee, int count)
		=> new()
		{
			Employee = employee,
			EmployeeId = employee.Id,
			Equipment = equipment,
			EquipmentId = equipment.Id,
			Count = count
		};
}
