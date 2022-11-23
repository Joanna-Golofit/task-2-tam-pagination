using System.Threading;
using System.Threading.Tasks;

namespace TeamsAllocationManager.Contracts.Base.Queries;

public interface IAsyncQueryHandler<in TQuery, TResult>
	where TQuery : IQuery<TResult>
{
	Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
