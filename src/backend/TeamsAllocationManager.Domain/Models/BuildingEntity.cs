using System.Collections.Generic;

namespace TeamsAllocationManager.Domain.Models;

public class BuildingEntity : Entity
{
	public string Name { get; set; } = null!;
	public ICollection<FloorEntity> Floors { get; set; } = new List<FloorEntity>();
}
