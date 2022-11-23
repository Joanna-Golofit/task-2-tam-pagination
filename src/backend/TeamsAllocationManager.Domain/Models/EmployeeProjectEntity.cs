using System;
using TeamsAllocationManager.Domain.Enums;

namespace TeamsAllocationManager.Domain.Models;

public class EmployeeProjectEntity : Entity
{
	public Guid EmployeeId { get; set; }
	public EmployeeEntity Employee { get; set; } = null!;
	public Guid ProjectId { get; set; }
	public ProjectEntity Project { get; set; } = null!;
	public bool IsTeamLeaderProjectRole { get; set; }

	public static EmployeeProjectEntity AssignEmployeeToProject(EmployeeEntity employee, ProjectEntity project, bool isTeamLeader)
		=> new EmployeeProjectEntity
		{
			Employee = employee,
			Project = project,
			IsTeamLeaderProjectRole = isTeamLeader
		};

	public static EmployeeProjectEntity AddNewExternalEmployeeToProject(ProjectEntity project, int idx, bool isTeamLeader)
		=> new EmployeeProjectEntity
		{
			Employee = EmployeeEntity.NewExternalEmployee(project, idx),
			Project = project,
			IsTeamLeaderProjectRole = isTeamLeader
		};
}
