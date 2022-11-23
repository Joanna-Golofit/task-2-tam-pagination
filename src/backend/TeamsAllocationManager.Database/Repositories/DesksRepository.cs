using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Database.Repositories.GenericRepository;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.Repositories;

public class DesksRepository : RepositoryBase<DeskEntity>, IDesksRepository
{
	private readonly ApplicationDbContext _context;

	public DesksRepository(ApplicationDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<DeskEntity?> GetDesk(Guid deskId) =>
		await _context
			.Desks
			.Include(d => d.DeskReservations)
			.Include(d => d.EmployeeDeskHistory)
			.Include(d => d.Room)
				.ThenInclude(r => r.Floor)
				.ThenInclude(f => f.Building)
			.Where(d => d.Id == deskId)
			.AsSplitQuery()
			.SingleOrDefaultAsync();

	public async Task<IEnumerable<DeskEntity>> GetDeskForRoom(Guid roomId, IEnumerable<Guid> desksToDelete) 
		=> await _applicationDbContext
		         .Desks
		         .Include(r => r.Room)
		         .Where(d => desksToDelete.Contains(d.Id) && d.Room.Id == roomId)
		         .AsSplitQuery()
		         .ToListAsync();

	public async Task<IEnumerable<DeskEntity>> GetDeskForRoom(Guid roomId)
		=> await _applicationDbContext
		         .Desks
		         .Where(d => d.RoomId == roomId)
		         .ToListAsync();

	public async Task<IEnumerable<DeskEntity>> GetDesks(IEnumerable<Guid> ids)
		=> await _context
			.Desks
			.Include(d => d.DeskReservations)
			.Include(d => d.EmployeeDeskHistory)
				.ThenInclude(edh => edh.Employee)
			.Include(d => d.Room)
				.ThenInclude(r => r.Floor)
				.ThenInclude(f => f.Building)
			.Where(d => ids.Contains(d.Id))
			.AsSplitQuery()
			.ToListAsync();

	public async Task<DeskEntity?> GetDeskWithHistoryAndBuildings(Guid deskId) =>
		await _context
			.Desks
			.Include(d => d.DeskReservations)
				.ThenInclude(dr => dr.Employee)
			.Include(d => d.Room)
				.ThenInclude(r => r.Floor)
				.ThenInclude(b => b.Building)
			.Include(d => d.EmployeeDeskHistory)
			.Where(d => d.Id == deskId)
			.AsSplitQuery()
			.SingleOrDefaultAsync();

	public async Task<DeskEntity?> GetDeskWithLocationAndHistoryAndReservation(Guid deskId) =>
		await _context
			.Desks
			.Include(d => d.Room)
				.ThenInclude(r => r.Floor)
				.ThenInclude(b => b.Building)
			.Include(d => d.EmployeeDeskHistory)
				.ThenInclude(dh => dh.Employee)
			.Include(d => d.DeskReservations)
				.ThenInclude(dr => dr.Employee)
			.Where(d => d.Id == deskId)
			.AsSplitQuery()
			.SingleOrDefaultAsync();

	public async Task<IEnumerable<DeskEntity>> GetRoomsDesksWithLocationAndHistoryAndReservation(Guid roomId) =>
		await _context
			.Desks
			.Include(d => d.Room)
				.ThenInclude(r => r.Floor)
				.ThenInclude(b => b.Building)
			.Include(d => d.EmployeeDeskHistory)
				.ThenInclude(dh => dh.Employee)
			.Include(d => d.DeskReservations)
				.ThenInclude(dr => dr.Employee)
			.Where(d => d.RoomId == roomId)
			.AsSplitQuery()
			.ToListAsync();

	public async Task<IEnumerable<DeskEntity>> GetDesksToRelease(Guid roomId, Guid projectId)
		=> await _applicationDbContext.Desks
		                              .Include(d => d.DeskReservations)
		                              .ThenInclude(dr => dr.Employee)
		                              .ThenInclude(e => e.Projects)
		                              .Include(d => d.EmployeeDeskHistory)
		                              .Where(d => d.RoomId == roomId && d.DeskReservations.Any(dr =>
			                              dr.IsSchedule && dr.Employee.Projects.Any(p => p.ProjectId == projectId)))
		                              .ToListAsync();
}
