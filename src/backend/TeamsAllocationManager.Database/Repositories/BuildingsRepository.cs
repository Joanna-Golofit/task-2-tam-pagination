using TeamsAllocationManager.Database.Repositories.GenericRepository;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.Repositories
{
	public class BuildingsRepository : RepositoryBase<BuildingEntity>, IBuildingsRepository
	{
		public BuildingsRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}
