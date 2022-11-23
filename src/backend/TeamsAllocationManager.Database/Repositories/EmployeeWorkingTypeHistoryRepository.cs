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
	public class EmployeeWorkingTypeHistoryRepository : RepositoryBase<EmployeeWorkingTypeHistoryEntity>, IEmployeeWorkingTypeHistoryRepository
	{
		public EmployeeWorkingTypeHistoryRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<IEnumerable<EmployeeWorkingTypeHistoryEntity>> GetEmployeeWorkingTypeHistoryForEmployee(Guid employeeId)
		=> await _applicationDbContext.EmployeeWorkingTypeHistory
		                              .Where(e => employeeId.Equals(e.EmployeeId))
		                              .ToListAsync();

		public async Task<IEnumerable<EmployeeWorkingTypeHistoryEntity>> GetEmployeeWorkingTypeHistoryWithDeletionDate(DateTime deletionDate)
		=> await _applicationDbContext
		         .EmployeeWorkingTypeHistory
		         .Where(ewth => deletionDate > ewth.To)
		         .ToListAsync();
	}
}
