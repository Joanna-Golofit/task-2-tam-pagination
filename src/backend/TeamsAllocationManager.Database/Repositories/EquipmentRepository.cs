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
	public class EquipmentRepository : RepositoryBase<EquipmentEntity>, IEquipmentRepository
	{
		public EquipmentRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<bool> HasEquipment(string name, Guid? ignoreId = null)
			=> ignoreId != null ?
				await _applicationDbContext.Equipments.AnyAsync(e => e.Name == name && e.Id != ignoreId) :
				await _applicationDbContext.Equipments.AnyAsync(e => e.Name == name);

		public async Task<EquipmentEntity?> GetEquipmentForCompany(Guid companyId)
			=> await _applicationDbContext.Equipments
				 .Include(e => e.EmployeeEquipmentReservations)
					 .ThenInclude(ee => ee.Employee)
					 .ThenInclude(eh => eh.EmployeeEquipment)
				 .Where(e => e.Id == companyId)
				 .AsSplitQuery()
				 .FirstOrDefaultAsync();

		public async Task<EquipmentEntity?> GetEquipment(Guid equipmentId)
			=> await _applicationDbContext.Equipments
										  .Include(e => e.EmployeeEquipmentReservations)
											  .ThenInclude(eeh => eeh.Employee)
											  .ThenInclude(e => e.EquipmentHistory)
										  .Include(e => e.EmployeeEquipmentHistory)
										  .AsSplitQuery()
										  .SingleAsync(e => e.Id == equipmentId);

		public async Task<IEnumerable<EquipmentEntity>> GetAllEquipment()
		{
			var dbQuery = _applicationDbContext.Equipments
			                                   .OrderBy(e => e.Name)
			                                   .Include(e => e.EmployeeEquipmentReservations);

			return await dbQuery
			             .AsSplitQuery()
			             .ToListAsync();
		}

		public async Task<EquipmentEntity?> GetEquipmentDetails(Guid equipmentId)
			=> await _applicationDbContext.Equipments
									   .Include(ep => ep.EmployeeEquipmentReservations)
										   .ThenInclude(ep => ep.Employee)
										   .ThenInclude(e => e.Projects)
										   .ThenInclude(ep => ep.Project)
									   .Include(ep => ep.EmployeeEquipmentHistory)
										   .ThenInclude(ep => ep.Employee)
									  .Where(p => p.Id == equipmentId)
									  .AsSplitQuery()
									  .FirstOrDefaultAsync();
	}
}
