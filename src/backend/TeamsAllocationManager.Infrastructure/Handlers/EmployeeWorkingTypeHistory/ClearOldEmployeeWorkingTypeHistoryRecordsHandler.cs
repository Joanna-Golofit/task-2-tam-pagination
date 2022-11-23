using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.EmployeeWorkingTypeHistory.Commands;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Infrastructure.Handlers.EmployeeWorkingTypeHistory;

public class ClearOldEmployeeWorkingTypeHistoryRecordsHandler : IAsyncCommandHandler<ClearOldEmployeeWorkingTypeHistoryRecordsCommand, bool>
{
	private readonly IEmployeeWorkingTypeHistoryRepository _employeeWorkingTypeHistoryRepository;

	public ClearOldEmployeeWorkingTypeHistoryRecordsHandler(IEmployeeWorkingTypeHistoryRepository employeeWorkingTypeHistoryRepository)
	{
		_employeeWorkingTypeHistoryRepository = employeeWorkingTypeHistoryRepository;
	}

	public async Task<bool> HandleAsync(ClearOldEmployeeWorkingTypeHistoryRecordsCommand command, CancellationToken cancellationToken = default)
	{
		var deletionDate = command.DeletionDate;
		var emloyeeWorkingTypeHistoryRecords = await _employeeWorkingTypeHistoryRepository.GetEmployeeWorkingTypeHistoryWithDeletionDate(deletionDate);

		if (!emloyeeWorkingTypeHistoryRecords.Any())
		{
			return false;
		}

		await _employeeWorkingTypeHistoryRepository.RemoveRangeAsync(emloyeeWorkingTypeHistoryRecords);

		return true;
	}
}
