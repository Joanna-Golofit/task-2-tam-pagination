using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Base.Queries;

namespace TeamsAllocationManager.Contracts.Base;

public interface IDispatcher : ICommandDispatcher, IQueryDispatcher
{
}
