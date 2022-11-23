using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamsAllocationManager.Database.Repositories.GenericRepository;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.Repositories.Interfaces;

public interface IEquipmentRepository : IAsyncRepository<EquipmentEntity>
{
	Task<bool> HasEquipment(string name, Guid? ignoreId = null);

	Task<EquipmentEntity?> GetEquipmentForCompany(Guid companyId);

	Task<EquipmentEntity?> GetEquipment(Guid equipmentId);

	Task<IEnumerable<EquipmentEntity>> GetAllEquipment();

	Task<EquipmentEntity?> GetEquipmentDetails(Guid equipmentId);
}