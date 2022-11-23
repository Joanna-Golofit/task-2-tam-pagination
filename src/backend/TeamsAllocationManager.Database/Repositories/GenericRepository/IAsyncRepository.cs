using System.Collections.Generic;
using System.Threading.Tasks;
using TeamsAllocationManager.Domain;

namespace TeamsAllocationManager.Database.Repositories.GenericRepository
{
	public interface IAsyncRepository<TEntity> where TEntity : Entity
	{
		Task<TEntity?> GetByIdAsync(int id);

		Task<IEnumerable<TEntity>> GetAllAsync();

		Task AddAsync(TEntity entity);

		Task AddRangeAsync(IEnumerable<TEntity> entities);

		Task UpdateAsync(TEntity entity);

		Task UpdateRangeAsync(IEnumerable<TEntity> entities);

		Task RemoveAsync(TEntity entity);

		Task RemoveRangeAsync(IEnumerable<TEntity> entities);

		Task<int> CountAsync();
	}
}
