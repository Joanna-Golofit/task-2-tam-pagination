using System;

namespace TeamsAllocationManager.Dtos.Equipment
{
	public  class ReservationEquipmentEmployeeDto
	{
		public Guid EmployeeId { get; set; }
		public string? Name { get; set; }
		public int Count { get; set; }
	}
}
