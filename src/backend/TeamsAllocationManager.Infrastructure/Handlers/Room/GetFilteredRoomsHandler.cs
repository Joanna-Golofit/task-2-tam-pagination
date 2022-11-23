using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.EntityQueries;
using TeamsAllocationManager.Contracts.Room.Queries;
using TeamsAllocationManager.Dtos.Room;
using TeamsAllocationManager.Infrastructure.Extensions;

namespace TeamsAllocationManager.Infrastructure.Handlers.Room
{
	public class GetFilteredRoomsHandler : IAsyncQueryHandler<GetFilteredRoomsQuery, PagedRoomsDto>
	{
		public async Task<PagedRoomsDto> HandleAsync(GetFilteredRoomsQuery query, CancellationToken cancellationToken = default)
			=> await query.EntityQueries
			              .SingleByType<IEntityQuery, IRoomEntityQuery>()
			              .GetFilteredRoomAsync();
	}
}
