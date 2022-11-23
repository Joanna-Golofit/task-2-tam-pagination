using TeamsAllocationManager.Dtos.Common;

namespace TeamsAllocationManager.Contracts.Base.Queries;

public class ListQuery<TFilters, TResult> : IQuery<TResult>
	where TFilters : class, new()
{
	public PagedListQueryDto<TFilters> PageOptions { get; }

	public ListQuery(PagedListQueryDto<TFilters> pageOptions)
	{
		PageOptions = pageOptions;
	}
}