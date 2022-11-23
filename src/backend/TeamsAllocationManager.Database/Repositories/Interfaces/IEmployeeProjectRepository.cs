using TeamsAllocationManager.Database.Repositories.GenericRepository;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.Repositories.Interfaces;

public interface IEmployeeProjectRepository : IAsyncRepository<EmployeeProjectEntity>
{
}