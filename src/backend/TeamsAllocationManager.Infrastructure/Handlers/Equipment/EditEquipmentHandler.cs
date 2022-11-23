using System;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Equipment.Commands;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Equipment;

public class EditEquipmentHandler : IAsyncCommandHandler<EditEquipmentCommand, Guid>
{
	private readonly IEquipmentRepository _equipmentRepository;

	public EditEquipmentHandler(IEquipmentRepository equipmentRepository)
	{
		_equipmentRepository = equipmentRepository;
	}

	public async Task<Guid> HandleAsync(EditEquipmentCommand command, CancellationToken cancellationToken = default)
	{
		var dbEquipment = await _equipmentRepository.GetEquipment(command.EditEquipmentDto.EquipmentId);
	
		if (dbEquipment == null)
		{
			throw new EntityNotFoundException<EquipmentEntity>(nameof(EquipmentEntity.Id), command.EditEquipmentDto.EquipmentId);
		}

		if (await _equipmentRepository.HasEquipment(command.EditEquipmentDto.Name, dbEquipment.Id))
		{
			throw new EquipmentEntityDuplicateException(command.EditEquipmentDto.Name);
		}

		dbEquipment.Name = command.EditEquipmentDto.Name;
		dbEquipment.Count = command.EditEquipmentDto.Count;

		await _equipmentRepository.UpdateAsync(dbEquipment);

		return dbEquipment.Id;
	}
}
