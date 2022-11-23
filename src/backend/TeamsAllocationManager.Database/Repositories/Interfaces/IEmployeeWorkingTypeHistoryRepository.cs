using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamsAllocationManager.Database.Repositories.GenericRepository;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.Repositories.Interfaces;

public interface IEmployeeWorkingTypeHistoryRepository : IAsyncRepository<EmployeeWorkingTypeHistoryEntity>
{
	Task<IEnumerable<EmployeeWorkingTypeHistoryEntity>> GetEmployeeWorkingTypeHistoryForEmployee(Guid employeeId);

	Task<IEnumerable<EmployeeWorkingTypeHistoryEntity>> GetEmployeeWorkingTypeHistoryWithDeletionDate(DateTime deletionDate);
}