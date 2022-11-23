using System;
using System.Collections.Generic;
using TeamsAllocationManager.Dtos.Employee;

namespace TeamsAllocationManager.Dtos.Project;

public class ProjectForRoomDetailsDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = null!;
	public IEnumerable<TeamLeaderDto> TeamLeaders { get; set; } = new List<TeamLeaderDto>();
}
