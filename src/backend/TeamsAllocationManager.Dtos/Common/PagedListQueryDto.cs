namespace TeamsAllocationManager.Dtos.Common;

public class PagedListQueryDto<T>
	where T : class, new()
{
	public int PageNumber { get; set; } = 0;

	public int PageSize { get; set; } = 20;

	public int Offset => PageNumber * PageSize;

	public T Filters { get; set; } = new T();
}
