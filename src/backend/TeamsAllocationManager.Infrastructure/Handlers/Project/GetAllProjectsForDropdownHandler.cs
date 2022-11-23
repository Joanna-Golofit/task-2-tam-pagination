using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.Project.Queries;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Dtos.Project;
using TeamsAllocationManager.Infrastructure.Extensions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Project;

public class GetAllProjectsForDropdownHandler : IAsyncQueryHandler<GetAllProjectsForDropdownQuery, IEnumerable<ProjectForDropdownDto>>
{
	private readonly ApplicationDbContext _applicationDbContext;

	public GetAllProjectsForDropdownHandler(ApplicationDbContext applicationDbContext)
	{
		_applicationDbContext = applicationDbContext;
	}

	public async Task<IEnumerable<ProjectForDropdownDto>> HandleAsync(GetAllProjectsForDropdownQuery query, CancellationToken cancellationToken = default)
	{
		IEnumerable<ProjectForDropdownDto> dtoList = await _applicationDbContext.Projects
			.Where(!string.IsNullOrEmpty(query.Search), p => p.Name.ToLower().Contains(query.Search!.ToLower()))
			.Select(p => new ProjectForDropdownDto { Id = p.Id, Name = p.Name })
			.OrderBy(p => p.Name)
			.ToListAsync();

		return dtoList;
	}
}
