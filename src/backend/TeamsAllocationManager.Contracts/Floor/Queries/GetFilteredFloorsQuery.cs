using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Dtos.Floor;

namespace TeamsAllocationManager.Contracts.Floor.Queries
{
	public class GetFilteredFloorsQuery : IQuery<PagedFloorsDto>
	{
		public GetFilteredFloorsQuery()
		{
		}
	}
}