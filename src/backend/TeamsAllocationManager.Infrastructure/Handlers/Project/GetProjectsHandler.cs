using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.Project.Queries;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Domain.Enums;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Employee;
using TeamsAllocationManager.Dtos.Project;

namespace TeamsAllocationManager.Infrastructure.Handlers.Project;

public class GetProjectsHandler : IAsyncQueryHandler<GetProjectsQuery, IEnumerable<ProjectDto>>
{
	private readonly ApplicationDbContext _applicationDbContext;
	private readonly IMapper _mapper;

	public GetProjectsHandler(
		ApplicationDbContext applicationDbContext,
		IMapper mapper)
	{
		_applicationDbContext = applicationDbContext;
		_mapper = mapper;
	}

	public async Task<IEnumerable<ProjectDto>> HandleAsync(GetProjectsQuery query, CancellationToken cancellationToken = default)
	{

		IQueryable<ProjectEntity> dbQuery = _applicationDbContext.Projects
		                                                         .OrderBy(p => p.Name);

		int projectCount = await dbQuery.CountAsync(cancellationToken: cancellationToken);

		var projects = await dbQuery
			.Include(p => p.Employees)
				.ThenInclude(ep => ep.Employee)
				.ThenInclude(ep => ep.EmployeeDeskReservations)
				.ThenInclude(dr => dr.Desk)
			.AsSplitQuery()
			.AsNoTracking()
			.ToListAsync(cancellationToken: cancellationToken);

		var result = _mapper.Map<IEnumerable<ProjectDto>>(projects);

		IEnumerable<ProjectDto> projectDtos = result as ProjectDto[] ?? result.ToArray();
		foreach (var res in projectDtos)
		{
			ProjectEntity? project = projects.Single(p => p.Id == res.Id);

			var teamLeaders = project.Employees
				.Where(e => e.IsTeamLeaderProjectRole)
				.Select(ep => ep.Employee);

			res.TeamLeaders = _mapper.Map<List<TeamLeaderDto>>(teamLeaders)
				.OrderBy(tl => tl.Surname).ToList();

			res.PeopleCount = project.Employees.Count();
			res.OfficeEmployeesCount = project.Employees.Count(e => e.Employee.WorkspaceType == WorkspaceType.Office);
			res.RemoteEmployeesCount = project.Employees.Count(e => e.Employee.WorkspaceType == WorkspaceType.Remote);
			res.HybridEmployeesCount = project.Employees.Count(e => e.Employee.WorkspaceType == WorkspaceType.Hybrid);
			res.AssignedPeopleCount = project.Employees.Select(e => e.Employee).Count(e => e.EmployeeDeskReservations.Any(dr => dr.IsSchedule));

		}

		return projectDtos;
	}
}
