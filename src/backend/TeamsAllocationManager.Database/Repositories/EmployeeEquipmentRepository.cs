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
	public class EmployeeEquipmentRepository : RepositoryBase<EmployeeEquipmentEntity>, IEmployeeEquipmentRepository
	{
		public EmployeeEquipmentRepository(ApplicationDbContext context) : base(context)
		{
			_applicationDbContext = context;
		}

		public async Task<IList<EmployeeEquipmentEntity>> GetEquipmentsForEmployee(Guid employeeId) =>
			await _applicationDbContext.EmployeeEquipments.Include(ee => ee.Equipment)
			                          .ThenInclude(e => e.EmployeeEquipmentHistory)
			                          .Where(ee => ee.EmployeeId == employeeId)
			                          .ToListAsync();
	}
}
