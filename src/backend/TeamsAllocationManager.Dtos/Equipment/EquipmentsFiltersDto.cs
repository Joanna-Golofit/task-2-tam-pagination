using System;
using System.Collections.Generic;

namespace TeamsAllocationManager.Dtos.Equipment;

public class EquipmentsFiltersDto
{
	public IEnumerable<Guid> EquipmentIds { get; set; } = new List<Guid>();

	public string Name { get; set; } = string.Empty;
}
