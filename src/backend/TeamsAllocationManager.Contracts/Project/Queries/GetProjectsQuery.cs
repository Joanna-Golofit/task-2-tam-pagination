
using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Dtos.Project;

namespace TeamsAllocationManager.Contracts.Project.Queries;

public class GetProjectsQuery : IQuery<IEnumerable<ProjectDto>>
{
	public GetProjectsQuery()
	{
	}
}