using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TeamsAllocationManager.Contracts.Base;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Base.Queries;

namespace TeamsAllocationManager.Infrastructure;

public class Dispatcher : IDispatcher
{
	private readonly IServiceScopeFactory _scopeFactory;

	public Dispatcher(IServiceScopeFactory scopeFactory)
	{
		_scopeFactory = scopeFactory;
	}

	void ICommandDispatcher.Dispatch<TCommand>(TCommand command)
	{
		Type type = typeof(ICommandHandler<>);
		var argumentTypes = new Type[] { command.GetType() };
		Type handlerType = type.MakeGenericType(argumentTypes);

		using var scope = _scopeFactory.CreateScope();
		var handler = scope.ServiceProvider.GetService(handlerType) as ICommandHandler<TCommand>;
		handler!.Handle(command);
	}

	async Task ICommandDispatcher.DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
	{
		Type type = typeof(IAsyncCommandHandler<>);
		var argumentTypes = new Type[] { command.GetType() };
		Type handlerType = type.MakeGenericType(argumentTypes);

		using var scope = _scopeFactory.CreateScope();
		var handler = scope.ServiceProvider.GetService(handlerType) as IAsyncCommandHandler<TCommand>;
		await handler!.HandleAsync(command, cancellationToken);
	}

	TResult ICommandDispatcher.Dispatch<TCommand, TResult>(TCommand command)
	{
		Type type = typeof(ICommandHandler<,>);
		var argumentTypes = new Type[] { command.GetType(), typeof(TResult) };
		Type handlerType = type.MakeGenericType(argumentTypes);

		using var scope = _scopeFactory.CreateScope();
		var handler = scope.ServiceProvider.GetService(handlerType) as ICommandHandler<TCommand, TResult>;
		return handler!.Handle(command);
	}

	async Task<TResult> ICommandDispatcher.DispatchAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken)
	{
		Type type = typeof(IAsyncCommandHandler<,>);
		var argumentTypes = new Type[] { command.GetType(), typeof(TResult) };
		Type handlerType = type.MakeGenericType(argumentTypes);

		using var scope = _scopeFactory.CreateScope();
		var handler = scope.ServiceProvider.GetService(handlerType) as IAsyncCommandHandler<TCommand, TResult>;
		return await handler!.HandleAsync(command, cancellationToken);
	}

	TResult IQueryDispatcher.Dispatch<TQuery, TResult>(TQuery query)
	{
		Type type = typeof(IQueryHandler<,>);
		var argumentTypes = new Type[] { query.GetType(), typeof(TResult) };
		Type handlerType = type.MakeGenericType(argumentTypes);

		using var scope = _scopeFactory.CreateScope();
		var handler = scope.ServiceProvider.GetService(handlerType) as IQueryHandler<TQuery, TResult>;
		return handler!.Handle(query);
	}

	async Task<TResult> IQueryDispatcher.DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken)
	{
		Type type = typeof(IAsyncQueryHandler<,>);
		var argumentTypes = new Type[] { query.GetType(), typeof(TResult) };
		Type handlerType = type.MakeGenericType(argumentTypes);

		using var scope = _scopeFactory.CreateScope();
		var handler = scope.ServiceProvider.GetService(handlerType) as IAsyncQueryHandler<TQuery, TResult>;
		return await handler!.HandleAsync(query, cancellationToken);
	}
}
