using AutoMapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.Equipment.Queries;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Dtos.Common;
using TeamsAllocationManager.Dtos.Equipment;

namespace TeamsAllocationManager.Infrastructure.Handlers.Equipment;

public class GetAllEquipmentsHandler : IAsyncQueryHandler<GetAllEquipmentsQuery, IEnumerable<EquipmentDto>>
{
	private readonly IEquipmentRepository _equipmentRepository;
	private readonly IMapper _mapper;

	public GetAllEquipmentsHandler(IEquipmentRepository equipmentRepository, IMapper mapper)
	{
		_equipmentRepository = equipmentRepository;
		_mapper = mapper;
	}

	public async Task<IEnumerable<EquipmentDto>> HandleAsync(GetAllEquipmentsQuery query,
		CancellationToken cancellationToken = default)
	{
		var equipments = await _equipmentRepository.GetAllEquipment();

		var result = _mapper.Map<IEnumerable<EquipmentDto>>(equipments);

		return result;
	}
}