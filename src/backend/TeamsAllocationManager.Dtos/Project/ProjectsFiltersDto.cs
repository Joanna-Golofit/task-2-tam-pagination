using System;
using System.Collections.Generic;

namespace TeamsAllocationManager.Dtos.Project;

public class ProjectsFiltersDto
{
	public IEnumerable<Guid> TeamLeaderIds { get; set; } = new List<Guid>();

	public IEnumerable<Guid> ProjectIds { get; set; } = new List<Guid>();

	public int? UnassignedPeopleMinCount { get; set; }

	public int? UnassignedPeopleMaxCount { get; set; }

	public int? PeopleMinCount { get; set; }

	public int? PeopleMaxCount { get; set; }

	public bool? ShowActiveProjects { get; set; }
	public bool ShowLoggedUserProjects { get; set; }
	public bool ExternalCompanies { get; set; }
}
