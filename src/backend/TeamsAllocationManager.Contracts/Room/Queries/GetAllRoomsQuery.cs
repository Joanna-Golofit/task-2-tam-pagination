using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.EntityQueries;
using TeamsAllocationManager.Dtos.Room;

namespace TeamsAllocationManager.Contracts.Room.Queries;

public class GetAllRoomsQuery : IQuery<RoomsDto>
{
	public IEnumerable<IEntityQuery> EntityQueries { get; }

	public GetAllRoomsQuery(IEnumerable<IEntityQuery> entityQueries)
	{
		EntityQueries = entityQueries;
	}

}
