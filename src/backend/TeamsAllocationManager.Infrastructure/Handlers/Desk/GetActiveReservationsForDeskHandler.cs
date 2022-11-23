using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.Desks.Queries;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Dtos.Desk;

namespace TeamsAllocationManager.Infrastructure.Handlers.Desk;

public class GetActiveReservationsForDeskHandler : IAsyncQueryHandler<GetActiveReservationsForDeskQuery, IEnumerable<ScheduledDeskReservationInfoDto>>
{
	private readonly IDeskReservationsRepository _deskReservationsRepository;
	private readonly IMapper _mapper;

	public GetActiveReservationsForDeskHandler(IDeskReservationsRepository deskReservationsRepository, IMapper mapper)
	{
		_deskReservationsRepository = deskReservationsRepository;
		_mapper = mapper;
	}

	public async Task<IEnumerable<ScheduledDeskReservationInfoDto>> HandleAsync(GetActiveReservationsForDeskQuery query, CancellationToken cancellationToken = default)
	{
		var deskReservations = await _deskReservationsRepository.GetDeskReservations(query.DeskId);

		return _mapper.Map<IEnumerable<ScheduledDeskReservationInfoDto>>(deskReservations);
	}
}
