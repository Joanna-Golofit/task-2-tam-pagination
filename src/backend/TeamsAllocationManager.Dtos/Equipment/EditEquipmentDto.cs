using System;

namespace TeamsAllocationManager.Dtos.Equipment;

public class EditEquipmentDto
{
	public Guid EquipmentId { get; init; }
	public string Name { get; init; } = string.Empty;
	public int Count { get; init; }
}
