using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.Floor.Queries;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Building;
using TeamsAllocationManager.Dtos.Floor;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Floor
{
	public class GetFilteredFloorHandler : IAsyncQueryHandler<GetFilteredFloorsQuery, PagedFloorsDto>
	{
		private readonly IFloorsRepository _floorsRepository;
		private readonly IBuildingsRepository _buildingsRepository;
		private readonly IMapper _mapper;

		public GetFilteredFloorHandler(IFloorsRepository floorsRepository,
			IBuildingsRepository buildingsRepository,
			IMapper mapper)
		{
			_floorsRepository = floorsRepository;
			_buildingsRepository = buildingsRepository;
			_mapper = mapper;
		}

		public async Task<PagedFloorsDto> HandleAsync(GetFilteredFloorsQuery query, CancellationToken cancellationToken = default)
		{


			var floorEntities = await _floorsRepository.GetFilteredFloors();

			var floors = floorEntities
			             .Select(f =>
			             {
				             FloorDto floor = _mapper.Map<FloorDto>(f);
				             floor.Area = f.Rooms.Sum(r => r.Area);
				             floor.Capacity = f.Rooms.Sum(r => r.Desks.Count());
				             floor.OccupiedDesks = f.Rooms.Sum(r => r.Desks.Count(d => d.DeskReservations.Any(dr => dr.IsSchedule && ContainsAllWeekDays(dr))));
				             floor.RoomCount = f.Rooms.Count();
				             return floor;
			             })
			             .ToList();

			return new PagedFloorsDto
			{
				Buildings = _mapper.Map<IEnumerable<BuildingDto>>(await _buildingsRepository.GetAllAsync()),
				MaxFloor = floors.Any() ? floors.Max(r => r.Floor) : 0,
				Floors = floors
			};
		}

		private static void ValidateFilter(FloorsQueryDto filtersFloorFilter)
		{
			if (filtersFloorFilter.OccupiedDesksRange?.Min > filtersFloorFilter.OccupiedDesksRange?.Max)
			{
				throw new InvalidArgumentException(
					ExceptionMessage.GetMessage(ExceptionMessage.InvalidArgument_OccupiedDeskRangeValues))
				{
					TranslationKey = ExceptionMessage.InvalidArgument_OccupiedDeskRangeValues
				};
			}

			if (filtersFloorFilter.CapacityRange?.Min > filtersFloorFilter.CapacityRange?.Max)
			{
				throw new InvalidArgumentException(
					ExceptionMessage.GetMessage(ExceptionMessage.InvalidArgument_IncorrectDeskCapacityRangeValues))
				{
					TranslationKey = ExceptionMessage.InvalidArgument_IncorrectDeskCapacityRangeValues
				};
			}
		}

		private IEnumerable<FloorEntity> FilterOccupiedDesks(IEnumerable<FloorEntity> floorEntities, FloorsQueryDto? floorFilter)
		{
			if (floorFilter?.OccupiedDesksRange != null && floorFilter.OccupiedDesksRange.Min.HasValue
				|| floorFilter?.OccupiedDesksRange != null && floorFilter.OccupiedDesksRange.Max.HasValue)
			{
				if (floorFilter.OccupiedDesksRange.Max.HasValue && floorFilter.OccupiedDesksRange.Min.HasValue)
				{
					floorEntities = floorEntities.Where(f =>
						f.Rooms.SelectMany(r => r.Desks).SelectMany(dr => dr.DeskReservations).Count(d => d.IsSchedule && ContainsAllWeekDays(d)) <= floorFilter.OccupiedDesksRange.Max
						&& f.Rooms.SelectMany(r => r.Desks).SelectMany(dr => dr.DeskReservations).Count(d => d.IsSchedule && ContainsAllWeekDays(d)) >= floorFilter.OccupiedDesksRange.Min);
				}
				else
				{
					if (floorFilter.OccupiedDesksRange.Min.HasValue)
					{
						floorEntities = floorEntities.Where(f => f.Rooms.SelectMany(r => r.Desks).SelectMany(dr => dr.DeskReservations).Count(d => d.IsSchedule && ContainsAllWeekDays(d)) >= floorFilter.OccupiedDesksRange.Min);
					}
					else if (floorFilter.OccupiedDesksRange.Max.HasValue)
					{
						floorEntities = floorEntities.Where(f => f.Rooms.SelectMany(r => r.Desks).SelectMany(dr => dr.DeskReservations).Count(d => d.IsSchedule && ContainsAllWeekDays(d)) <= floorFilter.OccupiedDesksRange.Max);
					}
				}
			}

			return floorEntities;
		}

		private bool ContainsAllWeekDays(DeskReservationEntity deskReservation)
		{
			return deskReservation.ScheduledWeekdays.Contains(DayOfWeek.Monday)
				   && deskReservation.ScheduledWeekdays.Contains(DayOfWeek.Tuesday)
				   && deskReservation.ScheduledWeekdays.Contains(DayOfWeek.Wednesday)
				   && deskReservation.ScheduledWeekdays.Contains(DayOfWeek.Thursday)
				   && deskReservation.ScheduledWeekdays.Contains(DayOfWeek.Friday);
		}
	}
}
