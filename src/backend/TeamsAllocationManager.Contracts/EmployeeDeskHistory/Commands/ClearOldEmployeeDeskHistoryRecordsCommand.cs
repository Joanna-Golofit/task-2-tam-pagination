using System;
using TeamsAllocationManager.Contracts.Base.Commands;

namespace TeamsAllocationManager.Contracts.EmployeeDeskHistory.Commands;

public class ClearOldEmployeeDeskHistoryRecordsCommand : ICommand<bool>
{
	public DateTime DeletionDate { get; }
	public ClearOldEmployeeDeskHistoryRecordsCommand()
	{
		DeletionDate = DateTime.Now.AddMonths(-18);
	}
}
