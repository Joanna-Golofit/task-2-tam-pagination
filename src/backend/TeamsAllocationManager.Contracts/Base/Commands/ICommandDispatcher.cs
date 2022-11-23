using System.Threading;
using System.Threading.Tasks;

namespace TeamsAllocationManager.Contracts.Base.Commands;

public interface ICommandDispatcher
{
	void Dispatch<TCommand>(TCommand command) where TCommand : ICommand;
	Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand;
	TResult Dispatch<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>;
	Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand<TResult>;
}
