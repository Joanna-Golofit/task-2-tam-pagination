namespace TeamsAllocationManager.Dtos.Common;

public class PagedListResultDto<T>
	where T : class
{
	public int Count { get; set; }

	public T? Payload { get; set; }
}
