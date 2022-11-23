using System;
using System.Collections.Generic;

namespace TeamsAllocationManager.Dtos.Equipment;

public class ReservationEquipmentDto
{
	public Guid EquipmentId { get; set; }
	public IList<ReservationEquipmentEmployeeDto> EmployeeReservations { get; set; } = null!;
	public DateTime DateFrom { get; set; }
}
