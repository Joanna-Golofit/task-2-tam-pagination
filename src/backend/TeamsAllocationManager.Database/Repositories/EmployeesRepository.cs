using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Database.Repositories.GenericRepository;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.Repositories;

public class EmployeesRepository : RepositoryBase<EmployeeEntity>, IEmployeesRepository
{
	private readonly ApplicationDbContext _context;

	public EmployeesRepository(ApplicationDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<EmployeeEntity?> GetEmployee(string email) =>
		await _context
			.Employees
			.Where(e => e.Email == email)
			.SingleOrDefaultAsync();

	public async Task<EmployeeEntity?> GetEmployee(Guid id) =>
		await _context
			.Employees
			.Where(e => e.Id == id)
			.SingleOrDefaultAsync();

	public async Task<EmployeeEntity?> GetEmployeeWithReservations(Guid id)
		=> await _context
		    .Employees
		    .Include(e => e.EmployeeDeskReservations)
		    .Include(e => e.UserRoles)
		    .ThenInclude(e => e.Role)
			.SingleOrDefaultAsync(e => e.Id == id);

	public async Task<EmployeeEntity?> GetEmployeeWithReservations(string email)
		=> await _context
		    .Employees
		    .Include(e => e.EmployeeDeskReservations)
		    .Include(e => e.UserRoles)
		    .ThenInclude(e => e.Role)
			.SingleOrDefaultAsync(e => e.Email == email);

	public async Task<IEnumerable<EmployeeEntity>> GetEmployees(IEnumerable<Guid> employeesIds) =>
		await _context
			    .Employees
			    .Where(e => employeesIds.Contains(e.Id))
			    .Include(e => e.EmployeeDeskReservations)
			    .AsSplitQuery()
			    .ToListAsync();

	public async Task<EmployeeEntity?> GetExternalEmployee(Guid employeeId)
		=> await _applicationDbContext
		         .Employees
		         .Include(e => e.Projects)
		         .ThenInclude(ep => ep.Project)
		         .Include(e => e.EmployeeDeskReservations)
		         .ThenInclude(dr => dr.Desk)
		         .Where(e => e.Id == employeeId && e.IsExternal)
		         .AsSplitQuery()
		         .FirstOrDefaultAsync();

	public async Task<IEnumerable<EmployeeEntity>> GetEmployeesForProject(Guid projectId)
		=> await _applicationDbContext
		         .Employees
		         .Where(e => e.Projects.Any(ep => ep.ProjectId == projectId))
		         .ToListAsync();

	public async Task<IEnumerable<EmployeeEntity>> GetEmployeesForSearcher()
		=> await _applicationDbContext.Employees
		                              .Include(e => e.Projects)
		                              .ThenInclude(p => p.Project)
		                              .Where(e => e.Email!.Contains("ANONEMAIL") == false)
		                              .AsSplitQuery()
		                              .ToListAsync();

	public async Task<IEnumerable<EmployeeEntity>> GetTeamLeaders()
		=> await _applicationDbContext.Employees
		                              .Where(e => e.UserRoles.Any(ur => ur.Role.Name.Equals(RoleEntity.TeamLeader)))
		                              .OrderBy(e => e.Surname)
		                              .ToListAsync();

	public async Task<EmployeeEntity?> GetEmployeeWithDetails(Guid employeeId)
	=> await _applicationDbContext.Employees
	                              .Include(e => e.Projects)
	                              .ThenInclude(ep => ep.Project)
	                              .ThenInclude(p => p.Employees)
	                              .ThenInclude(ep => ep.Employee)
	                              .ThenInclude(dr => dr.EmployeeDeskReservations)
	                              .ThenInclude(dr => dr.Desk)
	                              .ThenInclude(d => d.Room)
	                              .ThenInclude(r => r.Floor)
	                              .ThenInclude(f => f.Building)
	                              .Include(e => e.EmployeeDeskReservations)
	                              .ThenInclude(dr => dr.Desk)
	                              .ThenInclude(d => d.Room)
	                              .ThenInclude(r => r.Floor)
	                              .ThenInclude(f => f.Building)
	                              .Include(e => e.UserRoles)
	                              .ThenInclude(ur => ur.Role)
	                              .Where(e => e.Id == employeeId)
	                              .AsSplitQuery()
	                              .SingleOrDefaultAsync();

	public async Task<bool> IsUserAdmin(string userName)
		=> await _applicationDbContext.Employees.Include(ur => ur.UserRoles)
		                              .AnyAsync(x =>
			                              x.Email == userName &&
			                              x.UserRoles.Any(ur => ur.Role.Name == RoleEntity.Admin));

	public async Task<EmployeeEntity?> GetLoggedEmployee(string? name, string? email)
		=> await _context
		         .Employees
		         .AsNoTracking()
		         .Include(e => e.UserRoles)
		         .ThenInclude(ur => ur.Role)
		         .Include(e => e.EmployeeDeskReservations)
		         .ThenInclude(edr => edr.Desk)
		         .ThenInclude(d => d.Room)
		         .ThenInclude(r => r.Floor)
		         .ThenInclude(f => f.Building)
		         .AsSplitQuery()
		         .AsNoTracking()
		         .SingleOrDefaultAsync(e => e.Email == (email ?? name));

	public async Task<IEnumerable<string>> GetUserRoleNames(string userName)
		=> await _applicationDbContext
		         .Employees
		         .Include(ur => ur.UserRoles)
		         .Where(x => x.Email == userName)
		         .SelectMany(x => x.UserRoles.Select(r => r.Role.Name))
		         .AsSplitQuery()
		         .AsNoTracking()
		         .ToListAsync();
}
