using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Dtos.Project;

namespace TeamsAllocationManager.Contracts.Project.Queries;

public class GetAllProjectsForDropdownQuery : IQuery<IEnumerable<ProjectForDropdownDto>>
{
	public string? Search { get; }

	public GetAllProjectsForDropdownQuery(string? search)
	{
		Search = search;
	}
}
