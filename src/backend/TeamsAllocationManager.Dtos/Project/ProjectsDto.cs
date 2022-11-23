using System.Collections.Generic;
using TeamsAllocationManager.Dtos.Employee;

namespace TeamsAllocationManager.Dtos.Project;

public class ProjectsDto
{
	public IEnumerable<TeamLeaderDto> TeamLeaders { get; set; } = new List<TeamLeaderDto>();
	public IEnumerable<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
}
