using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.Employee.Queries;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Dtos.Employee;


namespace TeamsAllocationManager.Infrastructure.Handlers.Employee;

public class GetTeamLeaderProjectsForDropdownHandler : IAsyncQueryHandler<GetTeamLeaderProjectsForDropdownQuery, IEnumerable<TeamLeaderProjectForDropdownDto>>
{
	private readonly IProjectRepository _projectRepository;

	public GetTeamLeaderProjectsForDropdownHandler(IProjectRepository projectRepository)
	{
		_projectRepository = projectRepository;
	}

	public async Task<IEnumerable<TeamLeaderProjectForDropdownDto>> HandleAsync(GetTeamLeaderProjectsForDropdownQuery query, CancellationToken cancellationToken = default)
	{
		var projectsForTeamLeaders = await _projectRepository.GetProjectsForTeamLeaders();

		return projectsForTeamLeaders
		       .Select(p => new TeamLeaderProjectForDropdownDto
		       {
			       Id = p.Id,
			       Name = p.Name
		       });
	}
}
