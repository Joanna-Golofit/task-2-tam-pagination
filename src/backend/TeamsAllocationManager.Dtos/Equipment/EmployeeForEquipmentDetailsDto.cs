using System;

namespace TeamsAllocationManager.Dtos.Equipment
{
	public class EmployeeForEquipmentDetailsDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = null!;
		public string Surname { get; set; } = null!;
		public int Count { get; set; }
	}
}
