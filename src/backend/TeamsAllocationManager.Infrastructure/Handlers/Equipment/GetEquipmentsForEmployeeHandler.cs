using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.Equipment.Queries;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Dtos.Equipment;

namespace TeamsAllocationManager.Infrastructure.Handlers.Equipment
{
	public class GetEquipmentsForEmployeeHandler : IAsyncQueryHandler<GetEquipmentsForEmployeeQuery, IEnumerable<EmployeeEquipmentDetailDto>>
	{
		private readonly IMapper _mapper;
		private readonly IEmployeeEquipmentRepository _equipmentRepository;

		public GetEquipmentsForEmployeeHandler(IMapper mapper, IEmployeeEquipmentRepository equipmentRepository)
		{
			_equipmentRepository = equipmentRepository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<EmployeeEquipmentDetailDto>> HandleAsync(GetEquipmentsForEmployeeQuery query,
			CancellationToken cancellationToken = default)
		=> _mapper.Map<IEnumerable<EmployeeEquipmentDetailDto>>(await _equipmentRepository.GetEquipmentsForEmployee(query.EmployeeId));
	}
}
