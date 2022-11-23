using System;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Dtos.Project;

namespace TeamsAllocationManager.Contracts.Project.Queries;

public class GetProjectDetailsQuery : IQuery<ProjectDetailsDto>
{
	public Guid Id { get; }

	public GetProjectDetailsQuery(Guid id)
	{
		Id = id;
	}
}
