using System;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Dtos.Equipment;

namespace TeamsAllocationManager.Contracts.Project.Queries;

public class GetEquipmentDetailQuery : IQuery<EquipmentDetailDto>
{
	public Guid Id { get; }

	public GetEquipmentDetailQuery(Guid id)
	{
		Id = id;
	}
}
