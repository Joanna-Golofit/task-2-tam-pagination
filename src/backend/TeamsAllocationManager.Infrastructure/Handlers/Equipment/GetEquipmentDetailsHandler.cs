using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.Project.Queries;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Equipment;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Equipment;

public class GetEquipmentDetailsHandler : IAsyncQueryHandler<GetEquipmentDetailQuery, EquipmentDetailDto>
{
	private readonly IEquipmentRepository _equipmentRepository;
	private readonly IMapper _mapper;

	public GetEquipmentDetailsHandler(IEquipmentRepository equipmentRepository, IMapper mapper)
	{
		_equipmentRepository = equipmentRepository;
		_mapper = mapper;
	}
	public async Task<EquipmentDetailDto> HandleAsync(GetEquipmentDetailQuery query, CancellationToken cancellationToken = default)
	{
		EquipmentEntity? result = await _equipmentRepository.GetEquipmentDetails(query.Id);

		if (result == null)
		{
			throw new EntityNotFoundException<EquipmentEntity>(query.Id);
		}

		EquipmentDetailDto equipment = _mapper.Map<EquipmentDetailDto>(result);

		return equipment;
	}
}
