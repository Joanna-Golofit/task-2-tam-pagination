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
	public class EmployeeDeskHistoryRepository : RepositoryBase<EmployeeDeskHistoryEntity>, IEmployeeDeskHistoryRepository
	{
		public EmployeeDeskHistoryRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<IEnumerable<EmployeeDeskHistoryEntity>> GetEmployeeDeskHistoryWithDeletionDate(DateTime deletionDate)
		=> await _applicationDbContext
		         .EmployeeDeskHistory
		         .Where(ewth => deletionDate > ewth.To)
		         .ToListAsync();
	}
}
