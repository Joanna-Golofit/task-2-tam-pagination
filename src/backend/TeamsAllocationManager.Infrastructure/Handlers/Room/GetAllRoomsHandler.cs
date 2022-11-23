using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.EntityQueries;
using TeamsAllocationManager.Contracts.Room.Queries;
using TeamsAllocationManager.Dtos.Room;
using TeamsAllocationManager.Infrastructure.Extensions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Room;

public class GetAllRoomsHandler : IAsyncQueryHandler<GetAllRoomsQuery, RoomsDto>
{
	public async Task<RoomsDto> HandleAsync(GetAllRoomsQuery query, CancellationToken cancellationToken = default)
		=> await query.EntityQueries
			.SingleByType<IEntityQuery, IRoomEntityQuery>()
			.GetAllRoomsAsync();
}
