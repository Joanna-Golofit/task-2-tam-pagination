using TeamsAllocationManager.Database.Repositories.GenericRepository;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.Repositories
{
	public class EmployeeProjectRepository : RepositoryBase<EmployeeProjectEntity>, IEmployeeProjectRepository
	{
		protected EmployeeProjectRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}
