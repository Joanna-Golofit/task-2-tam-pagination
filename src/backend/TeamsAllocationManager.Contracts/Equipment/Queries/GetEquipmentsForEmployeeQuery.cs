using System;
using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Dtos.Equipment;

namespace TeamsAllocationManager.Contracts.Equipment.Queries
{
	public class GetEquipmentsForEmployeeQuery : IQuery<IEnumerable<EmployeeEquipmentDetailDto>>
	{
		public  Guid EmployeeId { get; }

		public GetEquipmentsForEmployeeQuery(Guid employeeId) => EmployeeId = employeeId;
	}
}
