using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.Desks.Queries;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Dtos.Building;
using TeamsAllocationManager.Dtos.Room;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Desk;

public class GetHotDesksHandler : IAsyncQueryHandler<GetHotDesksQuery, HotDeskRoomsDto>
{
	private readonly IRoomRepository _roomRepository;
	private readonly IBuildingsRepository _buildingsRepository;
	private readonly IMapper _mapper;

	public GetHotDesksHandler(IRoomRepository roomRepository, IBuildingsRepository buildingsRepository, IMapper mapper)
	{
		_roomRepository = roomRepository;
		_buildingsRepository = buildingsRepository;
		_mapper = mapper;
	}

	public async Task<HotDeskRoomsDto> HandleAsync(GetHotDesksQuery query, CancellationToken cancellationToken = default)
	{
		if (query.Period.startDate > query.Period.endDate)
		{
			throw new HotDeskGetReservationException(ExceptionMessage.GetMessage(ExceptionMessage.HotDesks_StartDateGreaterThanEndDate))
			{
				TranslationKey = ExceptionMessage.HotDesks_StartDateGreaterThanEndDate
			};
		}

		var rooms = await _roomRepository.GetHotDesks();

		var hotDeskRoomDtos = rooms.Select(r => {
			HotDeskRoomDto hotDeskRoomDto = _mapper.Map<HotDeskRoomDto>(r);
			hotDeskRoomDto.FreeHotDeskCount = r.Desks.Count(d => d.IsHotDesk && d.AvailableInPeriod(query.Period.startDate, query.Period.endDate));

			return hotDeskRoomDto;
		});

		return new HotDeskRoomsDto
		{
			MaxFloor = rooms.Any() ? hotDeskRoomDtos.Max(r => r.Floor) : 0,
			AreaMinLevelPerPerson = Configs.AreaMinLevelPerPerson,
			Rooms = hotDeskRoomDtos,
			Buildings = _mapper.Map<IEnumerable<BuildingDto>>(await _buildingsRepository.GetAllAsync()),
		};
	}
}
