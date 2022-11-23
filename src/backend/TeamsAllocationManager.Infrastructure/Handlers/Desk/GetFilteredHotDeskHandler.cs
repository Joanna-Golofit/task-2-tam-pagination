using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.Desks.Queries;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Building;
using TeamsAllocationManager.Dtos.Desk;
using TeamsAllocationManager.Dtos.Room;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Desk
{
	public class GetFilteredHotDesksHandler : IAsyncQueryHandler<GetFilteredHotDesksQuery, PagedHotDeskRoomsDto>
	{
		private readonly IRoomRepository _roomRepository;
		private readonly IBuildingsRepository _buildingsRepository;
		private readonly IMapper _mapper;

		public GetFilteredHotDesksHandler(IRoomRepository roomRepository, IBuildingsRepository buildingsRepository,
			IMapper mapper)
		{
			_roomRepository = roomRepository;
			_buildingsRepository = buildingsRepository;
			_mapper = mapper;
		}

		public async Task<PagedHotDeskRoomsDto> HandleAsync(GetFilteredHotDesksQuery query,
			CancellationToken cancellationToken = default)
		{

			var rooms = await _roomRepository.GetFilteredHotDesks(DateTime.Now, DateTime.MaxValue, null, null, null, null);

			IEnumerable<RoomEntity> roomEntities = rooms.ToList();
			var hotDeskCount = roomEntities.Count();

			var hotDeskRoomDtos = roomEntities.Select(r =>
			{
				HotDeskRoomDto hotDeskRoomDto = _mapper.Map<HotDeskRoomDto>(r);
				hotDeskRoomDto.FreeHotDeskCount = r.Desks.Count(d => d.IsHotDesk && d.AvailableInPeriod(DateTime.Now, DateTime.MaxValue));

				return hotDeskRoomDto;
			});

			IEnumerable<HotDeskRoomDto> deskRoomDtos = hotDeskRoomDtos.ToList();
			return new PagedHotDeskRoomsDto
			{
				MaxFloor = roomEntities.Any() ? deskRoomDtos.Max(r => r.Floor) : 0,
				AreaMinLevelPerPerson = Configs.AreaMinLevelPerPerson,
				Rooms = deskRoomDtos,
				Buildings = _mapper.Map<IEnumerable<BuildingDto>>(await _buildingsRepository.GetAllAsync()),
			};
		}

		private static void ValidateFilter(GetHotDesksQueryDto? queryHotDesksFilter, DateTime start, DateTime end)
		{
			if (string.IsNullOrEmpty(queryHotDesksFilter?.ReservationDateRangeFilter?.StartDate) ||
			    string.IsNullOrEmpty(queryHotDesksFilter?.ReservationDateRangeFilter?.EndDate))
			{
				throw new HotDeskGetReservationException(
					ExceptionMessage.GetMessage(ExceptionMessage.HotDesks_StartDateOrEndDateIsEmpty))
				{
					TranslationKey = ExceptionMessage.HotDesks_StartDateOrEndDateIsEmpty
				};
			}

			if (start > end)
			{
				throw new HotDeskGetReservationException(
					ExceptionMessage.GetMessage(ExceptionMessage.HotDesks_StartDateGreaterThanEndDate))
				{
					TranslationKey = ExceptionMessage.HotDesks_StartDateGreaterThanEndDate
				};
			}

			if (queryHotDesksFilter?.FreeDesksRange?.Min > queryHotDesksFilter?.FreeDesksRange?.Max)
			{
				throw new HotDeskGetReservationException(
					ExceptionMessage.GetMessage(ExceptionMessage.InvalidArgument_IncorrectFreeDeskRangeValues))
				{
					TranslationKey = ExceptionMessage.InvalidArgument_IncorrectFreeDeskRangeValues
				};
			}
		}
	}
}
