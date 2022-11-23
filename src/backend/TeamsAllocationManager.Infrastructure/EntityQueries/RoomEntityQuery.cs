using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.EntityQueries;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Building;
using TeamsAllocationManager.Dtos.Common;
using TeamsAllocationManager.Dtos.Project;
using TeamsAllocationManager.Dtos.Room;
using TeamsAllocationManager.Infrastructure.Exceptions;
using TeamsAllocationManager.Infrastructure.Extensions;

namespace TeamsAllocationManager.Infrastructure.EntityQueries;

public class RoomEntityQuery : IRoomEntityQuery
{
	private readonly ApplicationDbContext _applicationDbContext;
	private readonly IMapper _mapper;

	public RoomEntityQuery(ApplicationDbContext applicationDbContext, IMapper mapper)
	{
		_applicationDbContext = applicationDbContext;
		_mapper = mapper;
	}

	public async Task<RoomDetailsDto> GetRoomAsync(Guid id)
	{
		RoomEntity? room = await _applicationDbContext.Rooms
			.Include(r => r.Floor)
				.ThenInclude(f => f.Building)
			.Include(r => r.Desks)
				.ThenInclude(d => d.DeskReservations)
				.ThenInclude(dr => dr.Employee)
				.ThenInclude(e => e!.Projects)
				.ThenInclude(p => p.Project)
			.Include(r => r.Desks)
				.ThenInclude(d => d.EmployeeDeskHistory)
				.ThenInclude(edh => edh.Employee)
			.AsSplitQuery()
			.AsNoTracking()
			.SingleOrDefaultAsync(r => r.Id == id);

		if (room == null)
		{
			throw new EntityNotFoundException<RoomEntity>(id);
		}

		room.FilterLastNDaysInDeskHistory(7);

		RoomDetailsDto roomDto = _mapper.Map<RoomDetailsDto>(room);

		foreach (var desk in roomDto.DesksInRoom!)
		{
			foreach (var reservation in desk.Reservations!.Where(r => r.IsSchedule))
			{
				reservation.Employee.ProjectsNames = reservation.Employee.ProjectsNames
					.OrderBy(name => name)
					.ToList();
			}
		}

		roomDto.ProjectsInRoom = room.Desks
			.Where(d => d.DeskReservations.Any(dr => dr.IsSchedule))
			.SelectMany(d => d.DeskReservations.Where(dr => dr.IsSchedule).Select(dr => dr.Employee).SelectMany(e => e.Projects))
			.GroupBy(p => p.ProjectId)
			.Select(async p => {
				var ep = await _applicationDbContext.EmployeeProjects
					.Include(ep => ep.Employee)
					.Include(ep => ep.Project)
					.ThenInclude(p => p!.Employees)
					.ThenInclude(e => e.Employee)
					.Where(ep => ep.Id == p.First().Id)
					.AsSplitQuery()
					.SingleOrDefaultAsync();
				return _mapper.Map<ProjectForRoomDetailsDto>(ep);
			})
			.Select(t => t.Result)
			.OrderBy(p => p.Name)
			.ToList();

		foreach (var project in roomDto.ProjectsInRoom!)
		{
			project.TeamLeaders = project.TeamLeaders.OrderBy(tl => tl.Surname);
		}

		roomDto.AreaMinLevelPerPerson = Configs.AreaMinLevelPerPerson;
		roomDto.SasTokenForRoomPlans = Helpers.GenerateSasTokenForRoomPlansContainer();
		roomDto.DesksInRoom = roomDto.DesksInRoom.OrderBy(d => d.Number);

		return roomDto;
	}

	public async Task<RoomsDto> GetAllRoomsAsync()
	{
		var roomEntities = await _applicationDbContext.Rooms
		                                        .Include(r => r.Floor)
													.ThenInclude(f => f.Building)
		                                        .Include(d => d.Desks)
													.ThenInclude(d => d.DeskReservations)
													.ThenInclude(dr => dr.Employee)
		                                        .AsSplitQuery()
		                                        .AsNoTracking()
		                                        .ToListAsync();

		var rooms = roomEntities.Select(r => _mapper.Map<RoomDto>(r));

		return new RoomsDto
		{
			Rooms = rooms,
			Buildings = _mapper.Map<IEnumerable<BuildingDto>>(_applicationDbContext.Buildings),
			MaxFloor = rooms.Any() ? rooms.Max(r => r.Floor) : 0,
			AreaMinLevelPerPerson = Configs.AreaMinLevelPerPerson
		};
	}

	public async Task<PagedRoomsDto> GetFilteredRoomAsync()
	{
		var roomEntentesQuery = _applicationDbContext.Rooms
		                                            .Include(r => r.Floor)
		                                            .ThenInclude(f => f.Building)
		                                            .Include(d => d.Desks)
		                                            .ThenInclude(d => d.DeskReservations)
		                                            .ThenInclude(dr => dr.Employee)
		                                            .AsSplitQuery()
		                                            .AsNoTracking();

		int roomCount = await roomEntentesQuery.CountAsync();

		var roomEntities = await roomEntentesQuery.ToListAsync();

		var rooms = roomEntities.Select(r => _mapper.Map<RoomDto>(r));

		var pagedListResultDto = new PagedListResultDto<IEnumerable<RoomDto>>
		{
			Payload = rooms,
			Count = roomCount
		};

		return new PagedRoomsDto
		{
			Rooms = pagedListResultDto,
			Buildings = _mapper.Map<IEnumerable<BuildingDto>>(_applicationDbContext.Buildings),
			MaxFloor = rooms.Any() ? rooms.Max(r => r.Floor) : 0,
			AreaMinLevelPerPerson = Configs.AreaMinLevelPerPerson
		};
	}

	private static void ValidateFilter(RoomsQueryFilterDto roomsQueryFilter)
	{
		if (roomsQueryFilter.FreeDesksRange?.Min > roomsQueryFilter.FreeDesksRange?.Max)
		{
			throw new InvalidArgumentException(
				ExceptionMessage.GetMessage(ExceptionMessage.InvalidArgument_IncorrectFreeDeskRangeValues))
			{
				TranslationKey = ExceptionMessage.InvalidArgument_IncorrectFreeDeskRangeValues
			};
		}

		if (roomsQueryFilter.CapacityRange?.Min > roomsQueryFilter.CapacityRange?.Max)
		{
			throw new InvalidArgumentException(
				ExceptionMessage.GetMessage(ExceptionMessage.InvalidArgument_IncorrectDeskCapacityRangeValues))
			{
				TranslationKey = ExceptionMessage.InvalidArgument_IncorrectDeskCapacityRangeValues
			};
		}
	}

	private static IQueryable<RoomEntity> FilterFreeDesksRange(RoomsQueryFilterDto? roomsQueryFilter, IQueryable<RoomEntity> roomEntitesQuery)
	{
		if (roomsQueryFilter?.FreeDesksRange != null &&
		    (roomsQueryFilter.FreeDesksRange.Min.HasValue || roomsQueryFilter.FreeDesksRange.Max.HasValue))
		{
			if (roomsQueryFilter.FreeDesksRange.Max.HasValue && roomsQueryFilter.FreeDesksRange.Min.HasValue)
			{
				roomEntitesQuery = roomEntitesQuery.Where(f =>
					f.Desks.Count(d => !d.DeskReservations.Any(dr => dr.IsSchedule) && !d.IsHotDesk && d.IsEnabled) <=
					roomsQueryFilter.FreeDesksRange.Max
					&& f.Desks.Count(d => !d.DeskReservations.Any(dr => dr.IsSchedule) && !d.IsHotDesk && d.IsEnabled) >=
					roomsQueryFilter.FreeDesksRange.Min);
			}
			else
			{
				if (roomsQueryFilter.FreeDesksRange.Min.HasValue)
				{
					roomEntitesQuery = roomEntitesQuery.Where(r =>
						r.Desks.Count(d => !d.DeskReservations.Any(dr => dr.IsSchedule) && !d.IsHotDesk && d.IsEnabled) >=
						roomsQueryFilter.FreeDesksRange.Min);
				}
				else if (roomsQueryFilter.FreeDesksRange.Max.HasValue)
				{
					roomEntitesQuery = roomEntitesQuery.Where(r =>
						r.Desks.Count(d => !d.DeskReservations.Any(dr => dr.IsSchedule) && !d.IsHotDesk && d.IsEnabled) <=
						roomsQueryFilter.FreeDesksRange.Max);
				}
			}
		}

		return roomEntitesQuery;
	}

	private static IQueryable<RoomEntity> FilterCapacityRange(RoomsQueryFilterDto? roomsQueryFilter, IQueryable<RoomEntity> roomEntitesQuery)
	{
		if (roomsQueryFilter?.CapacityRange?.Min != null || roomsQueryFilter?.CapacityRange?.Max != null)
		{
			if (roomsQueryFilter.CapacityRange.Max.HasValue && roomsQueryFilter.CapacityRange.Min.HasValue)
			{
				roomEntitesQuery = roomEntitesQuery.Where(f =>
					f.Desks.Count <= roomsQueryFilter.CapacityRange.Max &&
					f.Desks.Count >= roomsQueryFilter.CapacityRange.Min);
			}
			else
			{
				if (roomsQueryFilter.CapacityRange.Min.HasValue)
				{
					roomEntitesQuery = roomEntitesQuery.Where(r => r.Desks.Count >= roomsQueryFilter.CapacityRange.Min);
				}
				else if (roomsQueryFilter.CapacityRange.Max.HasValue)
				{
					roomEntitesQuery = roomEntitesQuery.Where(r => r.Desks.Count >= roomsQueryFilter.CapacityRange.Max);
				}
			}
		}

		return roomEntitesQuery;
	}

	private static IQueryable<RoomEntity> FilterBuildingName(RoomsQueryFilterDto? roomsQueryFilter, IQueryable<RoomEntity> roomEntitesQuery)
	{
		if (!string.IsNullOrEmpty(roomsQueryFilter?.BuildingName))
		{
			roomEntitesQuery = roomEntitesQuery.Where(r => r.Floor.Building.Name.Equals(roomsQueryFilter.BuildingName));
		}

		return roomEntitesQuery;
	}

	private static IQueryable<RoomEntity> FilterRoomName(RoomsQueryFilterDto? roomsQueryFilter, IQueryable<RoomEntity> roomEntitesQuery)
	{
		if (!string.IsNullOrEmpty(roomsQueryFilter?.RoomName))
		{
			roomEntitesQuery = roomEntitesQuery.Where(r => r.Name.Equals(roomsQueryFilter.RoomName));
		}

		return roomEntitesQuery;
	}
}
