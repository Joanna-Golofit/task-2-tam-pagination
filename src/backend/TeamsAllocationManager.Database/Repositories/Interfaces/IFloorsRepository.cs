using System.Collections.Generic;
using System.Threading.Tasks;
using TeamsAllocationManager.Database.Repositories.GenericRepository;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.Repositories.Interfaces;

public interface IFloorsRepository : IAsyncRepository<FloorEntity>
{
	Task<IEnumerable<FloorEntity>> GetFloors();

	Task<IEnumerable<FloorEntity>> GetFilteredFloors();

	Task<int> GetFloorsCount();
}