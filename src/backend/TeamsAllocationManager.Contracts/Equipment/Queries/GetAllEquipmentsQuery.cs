using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Dtos.Equipment;

namespace TeamsAllocationManager.Contracts.Equipment.Queries;

public class GetAllEquipmentsQuery : IQuery<IEnumerable<EquipmentDto>>
{
	public GetAllEquipmentsQuery()
	{
	}
}