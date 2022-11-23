using System;
using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.EntityQueries;
using TeamsAllocationManager.Dtos.Room;

namespace TeamsAllocationManager.Contracts.Room.Queries;

public class GetRoomDetailsForProjectQuery : IQuery<RoomForProjectDto>
{
	public IEnumerable<IEntityQuery> EntityQueries { get; }
	public Guid Id { get; }
	public GetRoomDetailsForProjectQuery(IEnumerable<IEntityQuery> entityQueries, Guid id)
	{
		EntityQueries = entityQueries;
		Id = id;
	}
}
