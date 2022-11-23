using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Desks.Commands;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Desk;

public class ReleaseDeskEmployeeCommandHandler : IAsyncCommandHandler<ReleaseDeskEmployeeCommand>
{
	private readonly IDesksRepository _desksRepository;
	private readonly IEmployeesRepository _employeesRepository;

	public ReleaseDeskEmployeeCommandHandler(
		IDesksRepository desksRepository,
		IEmployeesRepository employeesRepository
	)
	{
		_desksRepository = desksRepository;
		_employeesRepository = employeesRepository;
	}

	public async Task HandleAsync(ReleaseDeskEmployeeCommand command, CancellationToken cancellationToken = default)
	{
		var desk = await _desksRepository.GetDesk(command.DeskId);
		var employee = await _employeesRepository.GetEmployee(command.EmployeeId);

		if (employee == null)
		{
			throw new EntityNotFoundException<EmployeeEntity>(nameof(EmployeeEntity.Id), command.EmployeeId);
		}

		if (desk == null)
		{
			throw new EntityNotFoundException<DeskEntity>(nameof(DeskEntity.Id), command.DeskId);
		}

		desk.ReleaseDesk(command.EmployeeId);

		await _desksRepository.UpdateAsync(desk);
	}
}
