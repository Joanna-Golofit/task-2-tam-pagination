using AutoMapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.Employee.Queries;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Dtos.Employee;


namespace TeamsAllocationManager.Infrastructure.Handlers.Employee;

public class GetTeamLeadersHandler : IAsyncQueryHandler<GetTeamLeadersQuery, IEnumerable<TeamLeaderDto>>
{
	private readonly IEmployeesRepository _employeesRepository;
	private readonly IMapper _mapper;

	public GetTeamLeadersHandler(IEmployeesRepository employeesRepository,
		IMapper mapper)
	{
		_employeesRepository = employeesRepository;
		_mapper = mapper;
	}

	public async Task<IEnumerable<TeamLeaderDto>> HandleAsync(GetTeamLeadersQuery query, CancellationToken cancellationToken = default)
	{
		var teamLeaders = await _employeesRepository.GetTeamLeaders();

		return _mapper.Map<IEnumerable<TeamLeaderDto>>(teamLeaders);
	}
}
