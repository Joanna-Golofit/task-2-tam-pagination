using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.Employee.Queries;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Dtos.Desk;

namespace TeamsAllocationManager.Infrastructure.Handlers.Employee;

public class GetActiveHotDeskReservationsForEmployeeHandler : IAsyncQueryHandler<GetActiveHotDeskReservationsForEmployeeQuery, IEnumerable<ReservationInfoDto>>
{
	private readonly IDeskReservationsRepository _deskReservationsRepository;
	private readonly IMapper _mapper;

	public GetActiveHotDeskReservationsForEmployeeHandler(IDeskReservationsRepository deskReservationsRepository, IMapper mapper)
	{
		_deskReservationsRepository = deskReservationsRepository;
		_mapper = mapper;
	}

	public async Task<IEnumerable<ReservationInfoDto>> HandleAsync(GetActiveHotDeskReservationsForEmployeeQuery query, CancellationToken cancellationToken = default)
		=> _mapper.Map<IEnumerable<ReservationInfoDto>>(await _deskReservationsRepository.GetHotDeskReservationsForEmployee(query.EmployeeId));
}
