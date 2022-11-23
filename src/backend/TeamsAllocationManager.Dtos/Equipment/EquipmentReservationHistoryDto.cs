using System;

namespace TeamsAllocationManager.Dtos.Equipment
{
	public class EquipmentReservationHistoryDto
	{
		public Guid? EmployeeId { get; set; }
		public string EmployeeName { get; set; } = null!;
		public string EmployeeSurname { get; set; } = null!;
		public DateTime ReservationStart { get; set; }
		public DateTime? ReservationEnd { get; set; }
		public Guid? EquipmentId { get; set; }
		public int Count { get; set; }
	}
}
