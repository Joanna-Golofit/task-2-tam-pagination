using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamsAllocationManager.Database.Repositories.GenericRepository;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.Repositories.Interfaces;

public interface IEmployeeDeskHistoryRepository : IAsyncRepository<EmployeeDeskHistoryEntity>
{
	Task<IEnumerable<EmployeeDeskHistoryEntity>> GetEmployeeDeskHistoryWithDeletionDate(DateTime deletionDate);
}