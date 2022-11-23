using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Equipment.Commands;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Equipment;

public class DeleteEquipmentHandler : IAsyncCommandHandler<DeleteEquipmentCommand>
{
	private readonly IEquipmentRepository _equipmentRepository;
	private readonly IEmployeeEquipmentRepository _employeeEquipmentRepository;

	public DeleteEquipmentHandler(IEquipmentRepository equipmentRepository, IEmployeeEquipmentRepository employeeEquipmentRepository)
	{
		_equipmentRepository = equipmentRepository;
		_employeeEquipmentRepository = employeeEquipmentRepository;
	}

	public async Task HandleAsync(DeleteEquipmentCommand command, CancellationToken cancellationToken = default)
	{
		EquipmentEntity? equipment = await _equipmentRepository.GetEquipmentForCompany(command.CompanyId);

		if (equipment == null)
		{
			throw new EntityNotFoundException<EquipmentEntity>(command.CompanyId);
		}

		await _employeeEquipmentRepository.RemoveRangeAsync(equipment.EmployeeEquipmentReservations);

		await _equipmentRepository.RemoveAsync(equipment);
	}
}
