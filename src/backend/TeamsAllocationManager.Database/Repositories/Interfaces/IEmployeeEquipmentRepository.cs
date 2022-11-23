using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamsAllocationManager.Database.Repositories.GenericRepository;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.Repositories.Interfaces;

public interface IEmployeeEquipmentRepository : IAsyncRepository<EmployeeEquipmentEntity>
{
	Task<IList<EmployeeEquipmentEntity>> GetEquipmentsForEmployee(Guid employeeId);
}