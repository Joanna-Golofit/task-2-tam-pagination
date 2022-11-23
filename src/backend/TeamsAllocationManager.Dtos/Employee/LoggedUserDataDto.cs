using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TeamsAllocationManager.Dtos.Enums;

namespace TeamsAllocationManager.Dtos.Employee;

public class LoggedUserDataDto
{
	public Guid Id { get; set; }
	public string Email { get; set; } = null!;
	[JsonProperty(PropertyName = "roles")]
	public IEnumerable<string> RoleNames { get; set; } = new List<string>();
	public WorkspaceType? WorkspaceType { get; set; }
}
