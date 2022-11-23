using System;
using System.Collections.Generic;
using TeamsAllocationManager.Dtos.Room;

namespace TeamsAllocationManager.Dtos.Project;

public class ProjectDetailsDto : ProjectDto
{
	public IEnumerable<RoomForProjectDto> Rooms { get; set; } = new List<RoomForProjectDto>();
	public IEnumerable<EmployeeForProjectDetailsDto> Employees { get; set; } = new List<EmployeeForProjectDetailsDto>();
	public DateTime? EndDate { get; set; }
}
