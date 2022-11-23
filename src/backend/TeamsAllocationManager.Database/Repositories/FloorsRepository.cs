using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeamsAllocationManager.Database.Repositories.GenericRepository;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.Repositories
{
	public class FloorsRepository : RepositoryBase<FloorEntity>, IFloorsRepository
	{
		public FloorsRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<IEnumerable<FloorEntity>> GetFloors()
			=> await _applicationDbContext.Floors
			                              .Include(f => f.Building)
			                              .Include(f => f.Rooms)
											.ThenInclude(r => r.Desks)
												.ThenInclude(d => d.DeskReservations)
			                              .AsSplitQuery()
			                              .AsNoTracking()
			                              .ToListAsync();

		public async Task<int> GetFloorsCount()
			=> await _applicationDbContext.Floors
			                              .Include(f => f.Building)
			                              .Include(f => f.Rooms)
											.ThenInclude(r => r.Desks)
												.ThenInclude(d => d.DeskReservations)
			                              .AsSplitQuery()
			                              .AsNoTracking()
			                              .CountAsync();

		public async Task<IEnumerable<FloorEntity>> GetFilteredFloors()
		{
			var floorQuery = _applicationDbContext.Floors
			                                      .Include(f => f.Building)
			                                      .Include(f => f.Rooms)
			                                      .ThenInclude(r => r.Desks)
			                                      .ThenInclude(d => d.DeskReservations)
			                                      .AsSplitQuery()
			                                      .AsNoTracking();

			return await floorQuery.ToListAsync();
		}

	}
}
