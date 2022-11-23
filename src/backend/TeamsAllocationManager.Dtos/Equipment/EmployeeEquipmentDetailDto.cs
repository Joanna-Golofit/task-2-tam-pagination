using System;

namespace TeamsAllocationManager.Dtos.Equipment
{
	public class EmployeeEquipmentDetailDto
	{
		public Guid EquipmentId { get; set; }
		public string EquipmentName { get; set; } = null!;
		public int Count { get; set; }
	}
}
