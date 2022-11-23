using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.EntityQueries;
using TeamsAllocationManager.Contracts.Room.Queries;
using TeamsAllocationManager.Dtos.Room;
using TeamsAllocationManager.Infrastructure.Extensions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Room;

public class GetRoomDetailsHandler : IAsyncQueryHandler<GetRoomDetailsQuery, RoomDetailsDto>
{
	public async Task<RoomDetailsDto> HandleAsync(GetRoomDetailsQuery query, CancellationToken cancellationToken = default)
	{
		return await query.EntityQueries
			.SingleByType<IEntityQuery, IRoomEntityQuery>()
			.GetRoomAsync(query.Id);
	}
}
