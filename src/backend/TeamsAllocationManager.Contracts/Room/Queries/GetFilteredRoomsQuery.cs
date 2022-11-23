using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.EntityQueries;
using TeamsAllocationManager.Dtos.Room;

namespace TeamsAllocationManager.Contracts.Room.Queries
{
	public class GetFilteredRoomsQuery : IQuery<PagedRoomsDto>
	{
		public IEnumerable<IEntityQuery> EntityQueries { get; }

		public GetFilteredRoomsQuery(IEnumerable<IEntityQuery> entityQueries)
		{
			EntityQueries = entityQueries;
		}
	}
}
