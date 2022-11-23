using System;
using TeamsAllocationManager.Contracts.Base.Commands;

namespace TeamsAllocationManager.Contracts.EmployeeWorkingTypeHistory;

public class ClearOldEmployeeWorkingTypeHistoryRecordsCommand : ICommand<bool>
{
	public DateTime DeletionDate { get; }
	public ClearOldEmployeeWorkingTypeHistoryRecordsCommand()
	{
		DeletionDate = DateTime.Now.AddMonths(-18);
	}
}
