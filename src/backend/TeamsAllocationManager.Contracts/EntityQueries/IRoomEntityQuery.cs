using System;
using System.Threading.Tasks;
using TeamsAllocationManager.Dtos.Room;

namespace TeamsAllocationManager.Contracts.EntityQueries;

public interface IRoomEntityQuery : IEntityQuery
{
	Task<RoomDetailsDto> GetRoomAsync(Guid id);
	Task<RoomsDto> GetAllRoomsAsync();
	Task<PagedRoomsDto> GetFilteredRoomAsync();
}