using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Equipment.Commands;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Equipment;

public class AddEquipmentHandler : IAsyncCommandHandler<AddEquipmentCommand, Guid>
{
	private readonly IEquipmentRepository _equipmentRepository;

	public AddEquipmentHandler(IEquipmentRepository equipmentRepository)
	{
		_equipmentRepository = equipmentRepository;
	}

	public async Task<Guid> HandleAsync(AddEquipmentCommand command, CancellationToken cancellationToken = default)
	{
		if (await _equipmentRepository.HasEquipment(command.Dto.Name))
		{
			throw new EquipmentEntityDuplicateException(command.Dto.Name);
		}

		if (command.Dto.Name.Length > 100)
		{
			throw new InvalidArgumentException(ExceptionMessage.GetMessage(ExceptionMessage.Equipments_NameLengthExceeded))
			{
				TranslationKey = ExceptionMessage.Equipments_NameLengthExceeded
			};
		};

		var newEquipment = new EquipmentEntity
		{
			Name = command.Dto.Name,
			AdditionalInfo = command.Dto.AdditionalInfo,
			EmployeeEquipmentReservations = new List<EmployeeEquipmentEntity>(),
			Count = command.Dto.Count
		};

		await _equipmentRepository.AddAsync(newEquipment);

		return newEquipment.Id;
	}
}
