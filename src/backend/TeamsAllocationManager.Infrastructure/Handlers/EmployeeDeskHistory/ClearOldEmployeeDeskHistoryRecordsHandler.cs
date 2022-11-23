using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.EmployeeDeskHistory.Commands;
using TeamsAllocationManager.Database.Repositories.Interfaces;

namespace TeamsAllocationManager.Infrastructure.Handlers.EmployeeDeskHistory;

public class ClearOldEmployeeDeskHistoryRecordsHandler : IAsyncCommandHandler<ClearOldEmployeeDeskHistoryRecordsCommand, bool>
{
	private readonly IEmployeeDeskHistoryRepository _employeeDeskHistoryRepository;

	public ClearOldEmployeeDeskHistoryRecordsHandler(IEmployeeDeskHistoryRepository employeeDeskHistoryRepository)
	{
		_employeeDeskHistoryRepository = employeeDeskHistoryRepository;
	}
	public async Task<bool> HandleAsync(ClearOldEmployeeDeskHistoryRecordsCommand command, CancellationToken cancellationToken = default)
	{
		var deletionDate = command.DeletionDate;

		var emloyeeDeskHistoryRecords = await _employeeDeskHistoryRepository.GetEmployeeDeskHistoryWithDeletionDate(deletionDate);

		if (!emloyeeDeskHistoryRecords.Any())
		{
			return false;
		}

		await _employeeDeskHistoryRepository.RemoveRangeAsync(emloyeeDeskHistoryRecords);

		return true;
	}

}
