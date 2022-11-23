using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Desks.Commands;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Infrastructure.Exceptions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Desk;

public class ReleaseEmployeesDesksCommandHandler : IAsyncCommandHandler<ReleaseEmployeesDesksCommand>
{
	private readonly IDesksRepository _desksRepository;
	private readonly IEmployeesRepository _employeesRepository;
	private readonly ApplicationDbContext _context;

	public ReleaseEmployeesDesksCommandHandler(
		IDesksRepository desksRepository, 
		IEmployeesRepository employeesRepository,
		ApplicationDbContext context)
	{
		_desksRepository = desksRepository;
		_employeesRepository = employeesRepository;
		_context = context;
	}

	public async Task HandleAsync(ReleaseEmployeesDesksCommand command, CancellationToken cancellationToken = default)
	{
		var employeesToRelease = await _employeesRepository.GetEmployees(command.EmployeesToRelease);

		var fetchedEmployeesIds = employeesToRelease.Select(etr => etr.Id);
		var fetchedAllRequestedEmployees = fetchedEmployeesIds.All(command.EmployeesToRelease.Contains) && command.EmployeesToRelease.All(fetchedEmployeesIds.Contains);

		if (!fetchedAllRequestedEmployees)
		{
			throw new EntityNotFoundException<EmployeeEntity>("Some of employees do not exist in database");
		}

		var allEmployeesDesksIds = employeesToRelease.SelectMany(emp => emp.EmployeeDeskReservations.Where(edr => edr.IsSchedule).Select(edr => edr.DeskId));

		var allEmployeesDesks = await _desksRepository.GetDesks(allEmployeesDesksIds);

		foreach (var employee in employeesToRelease)
		{
			var employeeDesks = allEmployeesDesks.Where(d => d.DeskReservations.Any(dr => dr.EmployeeId == employee.Id)).ToList();

			employeeDesks.ForEach(ed => ed.ReleaseDesk(employee.Id));
		}

		await _desksRepository.UpdateRangeAsync(allEmployeesDesks);
	}
}
