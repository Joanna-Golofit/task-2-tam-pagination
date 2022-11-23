using System;
using System.Collections.Generic;
using System.Text;
using TeamsAllocationManager.Dtos.Enums;

namespace TeamsAllocationManager.Dtos.Employee;

public class UserForSearcherDto
{
	public Guid Id { get; set; }
	public string DisplayName { get; set; } = null!;
	public string Email { get; set; } = null!;
	public int? WorkspaceType { get; set; }
	public IEnumerable<string> ProjectsNames { get; set; } = new List<string>();
}
