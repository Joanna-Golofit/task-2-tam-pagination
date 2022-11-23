using System;
using System.Collections.Generic;

namespace TeamsAllocationManager.Dtos.Equipment
{
	public class EquipmentDto
	{
		public Guid Id { get; set; }
		public IList<EmployeeForEquipmentDetailsDto> Employees { get; set; } = null!;
		public IList<EquipmentReservationHistoryDto> ReservationsHistory { get; set; } = null!;
		public string Name { get; set; } = "";
		public string AdditionalInfo { get; set; } = "";
		public int Count { get; set; }
		public int AssignedPeopleCount { get; set; }
	}
}
