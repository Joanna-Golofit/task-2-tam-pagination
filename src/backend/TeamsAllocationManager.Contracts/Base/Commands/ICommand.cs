namespace TeamsAllocationManager.Contracts.Base.Commands;

public interface ICommand
{
}

public interface ICommand<out TResult>
{
}
