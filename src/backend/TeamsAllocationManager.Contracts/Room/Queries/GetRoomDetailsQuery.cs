using System;
using System.Collections.Generic;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.EntityQueries;
using TeamsAllocationManager.Dtos.Room;

namespace TeamsAllocationManager.Contracts.Room.Queries;

public class GetRoomDetailsQuery : IQuery<RoomDetailsDto>
{
	public IEnumerable<IEntityQuery> EntityQueries { get; }
	public Guid Id { get; }
	public GetRoomDetailsQuery(IEnumerable<IEntityQuery> entityQueries, Guid id)
	{
		EntityQueries = entityQueries;
		Id = id;
	}
}
