using System.Threading;
using System.Threading.Tasks;

namespace TeamsAllocationManager.Contracts.Base.Queries;

public interface IQueryDispatcher
{
	TResult Dispatch<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
	Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default) where TQuery : IQuery<TResult>;
}
