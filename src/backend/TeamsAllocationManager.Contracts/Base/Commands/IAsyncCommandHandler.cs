using System.Threading;
using System.Threading.Tasks;

namespace TeamsAllocationManager.Contracts.Base.Commands;

public interface IAsyncCommandHandler<in TCommand>
	where TCommand : ICommand
{
	Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}

public interface IAsyncCommandHandler<in TCommand, TResult>
	where TCommand : ICommand<TResult>
{
	Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}
