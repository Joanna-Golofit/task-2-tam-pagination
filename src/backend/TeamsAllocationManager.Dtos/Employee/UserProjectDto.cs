using System;
using System.Collections.Generic;
using System.Text;

namespace TeamsAllocationManager.Dtos.Employee;

public class UserProjectDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = null!;
	public IEnumerable<string> TeamLeadersNames { get; set; } = new List<string>();
}
