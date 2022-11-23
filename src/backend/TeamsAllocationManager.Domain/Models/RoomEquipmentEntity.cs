namespace TeamsAllocationManager.Domain.Models;

public class RoomEquipmentEntity : Entity
{
	public RoomEntity Room { get; set; } = null!;

	public EquipmentEntity Equipment { get; set; } = null!;

	public int Count { get; set; }
}
