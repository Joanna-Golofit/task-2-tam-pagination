using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamsAllocationManager.Domain.Models;

public class RoomEntity : Entity
{
	public FloorEntity Floor { get; set; } = null!;
	public decimal Area { get; set; }
	public string Name { get; set; } = null!;
	public string? RoomPlanInfo { get; set; } = null;
	public ICollection<DeskEntity> Desks { get; set; } = new List<DeskEntity>();

	public void FilterLastNDaysInDeskHistory(int dayCount)
		=> Desks.ToList()
			    .ForEach(d => 
				    d.EmployeeDeskHistory = d.EmployeeDeskHistory
				                                .Where(edh => edh.Updated > DateTime.Now.AddDays(-dayCount) && edh.Employee != null)
				                                .OrderByDescending(edh => edh.From)
				                                .ToList());
}
