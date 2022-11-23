using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeamsAllocationManager.Database.Repositories.GenericRepository;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.Repositories;

public class RoomRepository : RepositoryBase<RoomEntity>, IRoomRepository
{
	private readonly ApplicationDbContext _context;

	public RoomRepository(ApplicationDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<RoomEntity?> GetRoom(Guid id)
		=> await _context
			        .Rooms
			        .SingleOrDefaultAsync(r => r.Id == id);

	public async Task<RoomEntity?> GetRoomsWithDesks(Guid id)
		=> await _context.Rooms
		                              .Include(r => r.Desks)
		                              .AsSplitQuery()
		                              .SingleOrDefaultAsync(r => r.Id == id);

	public async Task<RoomEntity?> GetRoomWithDetails(Guid roomId)
		=> await _applicationDbContext.Rooms
		                              .Include(r => r.Floor).ThenInclude(f => f.Building)
		                              .Include(d => d.Desks)
		                              .ThenInclude(ep => ep.DeskReservations)
		                              .ThenInclude(dr => dr.Employee)
		                              .AsSplitQuery()
		                              .AsNoTracking()
		                              .SingleOrDefaultAsync(r => r.Id == roomId);

	public async Task<IEnumerable<RoomEntity>> GetRoomsWithDetails(IEnumerable<Guid> roomIds)
	=> await _context.Rooms
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
	                 .AsNoTracking()
	                 .AsSplitQuery()
	                 .Where(r => roomIds.Contains(r.Id))
	                 .ToListAsync();

	public async Task<RoomEntity?> GetRoomWithDesksAndHistoryAndReservationsAndLocation(Guid id)
		=> await _context
			        .Rooms
					.Include(r => r.Floor)
					.ThenInclude(b => b.Building)
					.Include(r => r.Desks)
				        .ThenInclude(d => d.EmployeeDeskHistory)
				        .ThenInclude(dh => dh.Employee)
			        .Include(r => r.Desks)
					.ThenInclude(d => d.DeskReservations)
					.ThenInclude(dr => dr.Employee)
			        .AsSplitQuery()
			        .SingleOrDefaultAsync(r => r.Id == id);

	public async Task<IEnumerable<RoomEntity>> GetHotDesks()
		=> await _context.Rooms
				   .Include(r => r.Floor)
						.ThenInclude(f => f.Building)
				   .Include(r => r.Desks)
						.ThenInclude(d => d.DeskReservations)
				   .Where(r => r.Desks.Any(d => d.IsHotDesk))
				   .AsSplitQuery()
				   .ToListAsync();

	public async Task<int> GetHotDesksCount()
		=> await _context.Rooms
				   .Include(r => r.Floor)
						.ThenInclude(f => f.Building)
				   .Include(r => r.Desks)
						.ThenInclude(d => d.DeskReservations)
				   .Where(r => r.Desks.Any(d => d.IsHotDesk))
				   .AsSplitQuery()
				   .CountAsync();

	public async Task<IEnumerable<RoomEntity>> GetFilteredHotDesks(DateTime start,
		DateTime end,
		string? roomName,
		string? buildingName,
		int? freeDesksRangeMin,
		int? freeDesksRangeMax)
	{
		var roomsQuery = _context.Rooms
				   .Include(r => r.Floor)
						.ThenInclude(f => f.Building)
				   .Include(r => r.Desks)
						.ThenInclude(d => d.DeskReservations)
				   .Where(r => r.Desks.Any(d => d.IsHotDesk))
				   .AsSplitQuery();

		if (!string.IsNullOrEmpty(roomName))
		{
			roomsQuery = roomsQuery.Where(r => r.Name.Equals(roomName));
		}

		if (!string.IsNullOrEmpty(buildingName))
		{
			roomsQuery = roomsQuery.Where(r => r.Floor.Building.Name.Equals(buildingName));
		}

		var rooms = await roomsQuery.ToListAsync();

		if (freeDesksRangeMin.HasValue || freeDesksRangeMax.HasValue)
		{
			if (freeDesksRangeMax.HasValue && freeDesksRangeMin.HasValue)
			{
				rooms = rooms.Where(r =>
					r.Desks.Count(d => d.IsHotDesk && d.AvailableInPeriod(start, end)) >=
					freeDesksRangeMin &&
					r.Desks.Count(d =>
						d.IsHotDesk && d.AvailableInPeriod(start, end)) <=
					freeDesksRangeMax).ToList();
			}
			else
			{
				if (freeDesksRangeMin.HasValue)
				{
					rooms = rooms.Where(r =>
						r.Desks.Count(d => d.IsHotDesk && d.AvailableInPeriod(start, end)) >=
						freeDesksRangeMin).ToList();
				}
				else if (freeDesksRangeMax.HasValue)
				{
					rooms = rooms.Where(r =>
						r.Desks.Count(d => d.IsHotDesk && d.AvailableInPeriod(start, end)) <=
						freeDesksRangeMax).ToList();
				}
			}
		}

		return rooms;
	}		
}
