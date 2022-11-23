using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Database.Repositories.GenericRepository;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.Repositories;

public class DeskReservationsRepository : RepositoryBase<DeskReservationEntity>, IDeskReservationsRepository
{
	private readonly ApplicationDbContext _context;

	public DeskReservationsRepository(ApplicationDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<DeskReservationEntity?> GetReservation(Guid reservationId) =>
		await _context
			.DeskReservations
			.Include(dr => dr.Desk)
				.ThenInclude(d => d.EmployeeDeskHistory)
			.Include(dr => dr.Employee)
				.ThenInclude(e => e.EmployeeDeskReservations)
			.Include(dr => dr.CreatedBy)
			.Where(dr => dr.Id == reservationId)
			.AsSplitQuery()
			.SingleOrDefaultAsync();

	public async Task<DeskReservationEntity?> GetReservationWithRoomDetails(Guid id)
		=> await _context
		         .DeskReservations
		         .Include(dr => dr.Employee)
		         .Include(dr => dr.Desk)
		         .ThenInclude(d => d.Room)
		         .ThenInclude(r => r.Floor)
		         .ThenInclude(f => f.Building)
		         .Include(dr => dr.CreatedBy)
		         .AsSplitQuery()
		         .SingleOrDefaultAsync(dr => dr.Id == id);

	public async Task<IEnumerable<DeskReservationEntity>> GetHotDeskReservationsForEmployee(Guid employeeId)
		=> await _context
			        .DeskReservations
			        .Include(dr => dr.Desk)
				        .ThenInclude(d => d.Room)
				        .ThenInclude(r => r.Floor)
				        .ThenInclude(f => f.Building)
			        .Include(dr => dr.Employee)
			        .Include(dr => dr.CreatedBy)
			        .Where(dr => dr.EmployeeId == employeeId && !dr.IsSchedule)
			        .AsNoTracking()
			        .AsSplitQuery()
			        .ToListAsync();

	public async Task<IEnumerable<DeskReservationEntity>> GetDeskReservationsForEmployee(Guid employeeId)
		=> await _context
		         .DeskReservations
		         .Include(dr => dr.Desk)
		         .ThenInclude(d => d.Room)
		         .ThenInclude(r => r.Floor)
		         .ThenInclude(f => f.Building)
		         .Include(dr => dr.Employee)
		         .Include(dr => dr.CreatedBy)
		         .Where(dr => dr.EmployeeId == employeeId && dr.IsSchedule)
		         .AsNoTracking()
		         .AsSplitQuery()
		         .ToListAsync();

	public async Task<IEnumerable<DeskReservationEntity>> GetHotDeskReservations(Guid deskId)
		=> await _context
		         .DeskReservations
		         .Include(dr => dr.Desk)
		         .ThenInclude(d => d.Room)
		         .ThenInclude(r => r.Floor)
		         .ThenInclude(f => f.Building)
		         .Include(dr => dr.Employee)
		         .Include(dr => dr.CreatedBy)
		         .Where(dr => dr.DeskId == deskId && !dr.IsSchedule)
		         .AsSplitQuery()
		         .AsNoTracking()
		         .ToListAsync();

	public async Task<IEnumerable<DeskReservationEntity>> GetDeskReservations(Guid deskId)
		=> await _context
		         .DeskReservations
		         .Include(dr => dr.Desk)
					.ThenInclude(d => d.Room)
						.ThenInclude(r => r.Floor)
							.ThenInclude(f => f.Building)
		         .Include(dr => dr.Employee)
		         .Include(dr => dr.CreatedBy)
		         .Where(dr => dr.DeskId == deskId && dr.IsSchedule)
		         .AsSplitQuery()
		         .AsNoTracking()
		         .ToListAsync();

	public async Task<IEnumerable<DeskReservationEntity>> GetOldReservations()
		=> await _context
		         .DeskReservations
		         .Include(dr => dr.Employee)
		         .ThenInclude(e => e.EmployeeDeskHistory)
		         .Include(dr => dr.Desk)
		         .Where(dr => dr.ReservationEnd < DateTime.Now && !dr.IsSchedule)
		         .AsSplitQuery()
		         .ToListAsync();

	public async Task<IEnumerable<DeskReservationEntity>> GetReservationToRemind(int numberOfDaysAheadReservation)
	=> await _context
	         .DeskReservations
	         .Include(dr => dr.Employee)
	         .Include(dr => dr.Desk)
	         .ThenInclude(d => d.Room)
	         .ThenInclude(r => r.Floor)
	         .ThenInclude(f => f.Building)
	         .Where(dr => dr.ReservationStart.Date.Equals(DateTime.Now.Date.AddDays(numberOfDaysAheadReservation)))
	         .AsSplitQuery()
	         .ToListAsync();
}
