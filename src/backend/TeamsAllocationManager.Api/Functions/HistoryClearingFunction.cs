using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base;
using TeamsAllocationManager.Contracts.EmployeeDeskHistory.Commands;
using TeamsAllocationManager.Contracts.EmployeeWorkingTypeHistory.Commands;

namespace TeamsAllocationManager.Api.Functions;

public class HistoryClearingFunction : FunctionBase
{
	public HistoryClearingFunction(IDispatcher dispatcher) : base(dispatcher)
	{
	}

    [FunctionName("EmployeeWorkingTypeHistoryClearingFunction")]
    public async Task EmployeeWorkingTypeHistoryClearingFunction([TimerTrigger("0 0 0 1 * *")]TimerInfo myTimer) 
	{
		await _dispatcher.DispatchAsync<ClearOldEmployeeWorkingTypeHistoryRecordsCommand, bool>(new ClearOldEmployeeWorkingTypeHistoryRecordsCommand());
	}
	[FunctionName("EmployeeDeskHistoryClearingFunction")]
	public async Task EmployeeDeskHistoryClearingFunction([TimerTrigger("0 0 0 1 * *")] TimerInfo myTimer)
	{
		await _dispatcher.DispatchAsync<ClearOldEmployeeDeskHistoryRecordsCommand, bool>(new ClearOldEmployeeDeskHistoryRecordsCommand());
	}
}
