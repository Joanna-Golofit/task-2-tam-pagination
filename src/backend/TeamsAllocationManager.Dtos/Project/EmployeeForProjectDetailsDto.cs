using System;
using System.Collections.Generic;
using TeamsAllocationManager.Dtos.Desk;
using TeamsAllocationManager.Dtos.Enums;

namespace TeamsAllocationManager.Dtos.Project;

public class EmployeeForProjectDetailsDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = null!;
	public string Surname { get; set; } = null!;
	public WorkspaceType? Workmode { get; set; }
	public IEnumerable<string> ProjectsNames { get; set; } = new List<string>();
	public IEnumerable<RoomDeskDto> RoomDeskDtos { get; set; } = new List<RoomDeskDto>();
}
