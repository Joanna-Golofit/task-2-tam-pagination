using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.Room.Queries;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Room;
using TeamsAllocationManager.Infrastructure.Extensions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Room;

public class GetRoomDetailsForProjectHandler : IAsyncQueryHandler<GetRoomDetailsForProjectQuery, RoomForProjectDto>
{
	private readonly IRoomRepository _roomRepository;
	private readonly IMapper _mapper;

	public GetRoomDetailsForProjectHandler(IRoomRepository roomRepository, IMapper mapper)
	{
		_roomRepository = roomRepository;
		_mapper = mapper;
	}

	public async Task<RoomForProjectDto> HandleAsync(GetRoomDetailsForProjectQuery query, CancellationToken cancellationToken = default)
	{
		RoomEntity? room = await _roomRepository.GetRoomWithDetails(query.Id);

		RoomForProjectDto roomDto = _mapper.Map<RoomForProjectDto>(room);

		roomDto.DesksInRoom = roomDto.DesksInRoom.OrderBy(d => d.Number);
		roomDto.SasTokenForRoomPlans = Helpers.GenerateSasTokenForRoomPlansContainer();
		roomDto.OccupiedDesksCount = roomDto.DesksInRoom?.Count(d => d.Reservations?.Any(dr => dr.IsSchedule) ?? false) ?? 0;
		roomDto.HotDesksCount = roomDto.DesksInRoom?.Count(d => d.IsHotDesk == true) ?? 0;
		roomDto.DisabledDesksCount = roomDto.DesksInRoom?.Count(d => !d.IsEnabled) ?? 0;
		return roomDto;
	}
}
