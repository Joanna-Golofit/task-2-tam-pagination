using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeamsAllocationManager.Domain;

namespace TeamsAllocationManager.Database.Repositories.GenericRepository;

public class RepositoryBase<TEntity> : IAsyncRepository<TEntity> where TEntity : Entity
{
	protected ApplicationDbContext _applicationDbContext;

	protected RepositoryBase(ApplicationDbContext context)
	{
		_applicationDbContext = context;
	}

	public async Task<IEnumerable<TEntity>> GetAllAsync() => await _applicationDbContext.Set<TEntity>().ToListAsync();

	public async Task<TEntity?> GetByIdAsync(int id) => await _applicationDbContext.Set<TEntity>().FindAsync(id);

	public async Task AddAsync(TEntity entity)
	{
		await _applicationDbContext.Set<TEntity>().AddAsync(entity);
		await _applicationDbContext.SaveChangesAsync();
	}

	public async Task AddRangeAsync(IEnumerable<TEntity> entities)
	{
		foreach (TEntity entity in entities)
		{
			await _applicationDbContext.Set<TEntity>().AddAsync(entity);
			await _applicationDbContext.SaveChangesAsync();
		}
	}

	public async Task UpdateAsync(TEntity entity)
	{
		_applicationDbContext.Entry(entity).State = EntityState.Modified;
		await _applicationDbContext.SaveChangesAsync();
	}

	public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
	{
		foreach (TEntity entity in entities)
		{
			_applicationDbContext.Entry(entity).State = EntityState.Modified;
		}

		await _applicationDbContext.SaveChangesAsync();
	}

	public async Task RemoveAsync(TEntity entity)
	{
		_applicationDbContext.Set<TEntity>().Remove(entity);
		await _applicationDbContext.SaveChangesAsync();
	}

	public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
	{
		_applicationDbContext.Set<TEntity>().RemoveRange(entities);
		await _applicationDbContext.SaveChangesAsync();
	}

	public async Task<int> CountAsync()
	{
		return await _applicationDbContext.Set<TEntity>().CountAsync();
	}
}
