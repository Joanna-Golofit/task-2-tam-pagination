using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeamsAllocationManager.Database.Repositories.GenericRepository;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.Repositories
{
	public class ProjectsRepository : RepositoryBase<ProjectEntity>, IProjectRepository
	{
		public ProjectsRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<ProjectEntity?> GetProject(Guid companyId)
			=> await _applicationDbContext.Projects
			                              .Include(p => p.Employees)
			                              .ThenInclude(e => e.Employee)
			                              .Where(p => p.Id == companyId)
			                              .AsSplitQuery()
			                              .SingleOrDefaultAsync();

		public async Task<ProjectEntity?> GetExternalProject(Guid companyId)
			=> await _applicationDbContext.Projects
			                              .Include(p => p.Employees)
			                              .ThenInclude(ep => ep.Employee)
			                              .Where(p => p.Id == companyId && p.IsExternal)
			                              .AsSplitQuery()
			                              .SingleOrDefaultAsync();

		public async Task<ProjectEntity?> GetExternalProjectWithDesks(Guid companyId)
		=> await _applicationDbContext
		         .Projects
		         .Include(p => p.Employees)
		         .ThenInclude(ep => ep.Employee)
		         .ThenInclude(e => e.EmployeeDeskReservations)
		         .ThenInclude(dr => dr.Desk)
		         .Where(p => p.Id == companyId && p.IsExternal)
		         .AsSplitQuery()
		         .AsNoTracking()
		         .FirstOrDefaultAsync();

		public async Task<IEnumerable<ProjectEntity>> GetProjectsForTeamLeaders()
			=> await _applicationDbContext.Projects
			                              .Include(p => p.Employees)
			                              .ThenInclude(e => e.Employee)
			                              .Where(p => p.Employees.Any(ep => ep.IsTeamLeaderProjectRole))
			                              .AsSplitQuery()
			                              .ToListAsync();

		public async Task<IEnumerable<ProjectEntity>> GetProjectsWithEmployeesAssigned()
			=> await _applicationDbContext
			         .Projects
			         .Where(obj => obj.Employees.Any())
			         .ToListAsync();

		public async Task<ProjectEntity?> GetProjectsWithDetails(Guid projectId)
			=> await _applicationDbContext.Projects
			                              .Include(ep => ep.Employees)
			                              .ThenInclude(ep => ep.Employee)
			                              .ThenInclude(e => e.EmployeeDeskReservations)
			                              .ThenInclude(dr => dr.Desk)
			                              .ThenInclude(d => d.Room)
			                              .ThenInclude(r => r.Floor)
			                              .ThenInclude(f => f.Building)
			                              .Include(ep => ep.Employees)
			                              .ThenInclude(ep => ep.Employee)
			                              .ThenInclude(e => e.Projects)
			                              .ThenInclude(ep => ep.Project)
			                              .Where(p => p.Id == projectId)
			                              .AsSplitQuery()
			                              .FirstOrDefaultAsync();
	}
}
