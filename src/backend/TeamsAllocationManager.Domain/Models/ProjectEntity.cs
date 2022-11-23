using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamsAllocationManager.Domain.Models;

public class ProjectEntity : Entity
{
	public int? ExternalId { get; set; }
	public string Name { get; set; } = null!;
	public DateTime? EndDate { get; set; }
	public bool IsExternal { get; set; }
	public int? DivisionExternalId { get; set; }
	public string? Email { get; set; }

	public ICollection<EmployeeProjectEntity> Employees { get; set; } = new List<EmployeeProjectEntity>();

	public virtual ICollection<EmployeeEntity> TeamLeaders
		=> Employees
			.Where(ep => ep.IsTeamLeaderProjectRole)
			.Select(ep => ep.Employee)
			.ToList();
}
