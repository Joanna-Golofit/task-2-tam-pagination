using System;
using System.Collections.Generic;
using TeamsAllocationManager.Dtos.Employee;

namespace TeamsAllocationManager.Dtos.Project;

public class ProjectDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = null!;
	public List<TeamLeaderDto> TeamLeaders { get; set; } = new List<TeamLeaderDto>();
	public string Email { get; set; } = "";
	public int AssignedPeopleCount { get; set; }
	public int PeopleCount { get; set; }
	public int OfficeEmployeesCount { get; set; }
	public int RemoteEmployeesCount { get; set; }
	public int HybridEmployeesCount { get; set; }

	public int NotSetMembersCount =>
		PeopleCount - OfficeEmployeesCount - RemoteEmployeesCount - HybridEmployeesCount;
	public int UnassignedMembersCount =>
		PeopleCount - NotSetMembersCount - AssignedPeopleCount;
}
