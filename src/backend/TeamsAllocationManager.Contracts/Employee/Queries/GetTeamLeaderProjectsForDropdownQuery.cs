using System;
using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Dtos.Employee;

namespace TeamsAllocationManager.Contracts.Employee.Queries;

public class GetTeamLeaderProjectsForDropdownQuery : IQuery<IEnumerable<TeamLeaderProjectForDropdownDto>>
{
	public Guid Id { get; set; }

	public GetTeamLeaderProjectsForDropdownQuery(Guid id)
	{
		Id = id;
	}
}
