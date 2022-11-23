using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.Employee.Queries;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Dtos.Desk;

namespace TeamsAllocationManager.Infrastructure.Handlers.Employee;

public class GetActiveDeskReservationsForEmployeeHandler : IAsyncQueryHandler<GetActiveDeskReservationsForEmployeeQuery, IEnumerable<ScheduledDeskReservationInfoDto>>
{
	private readonly IDeskReservationsRepository _deskReservationsRepository;
	private readonly IMapper _mapper;

	public GetActiveDeskReservationsForEmployeeHandler(IDeskReservationsRepository deskReservationsRepository, IMapper mapper)
	{
		_deskReservationsRepository = deskReservationsRepository;
		_mapper = mapper;
	}

	public async Task<IEnumerable<ScheduledDeskReservationInfoDto>> HandleAsync(GetActiveDeskReservationsForEmployeeQuery query, CancellationToken cancellationToken = default)
		=> _mapper.Map<IEnumerable<ScheduledDeskReservationInfoDto>>(await _deskReservationsRepository.GetDeskReservationsForEmployee(query.EmployeeId));
}
