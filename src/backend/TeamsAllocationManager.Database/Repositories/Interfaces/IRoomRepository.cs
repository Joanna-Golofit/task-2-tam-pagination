using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Database.Repositories.GenericRepository;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.Repositories.Interfaces;

public interface IRoomRepository : IAsyncRepository<RoomEntity>
{
	Task<RoomEntity?> GetRoom(Guid id);
	Task<RoomEntity?> GetRoomsWithDesks(Guid id);
	Task<RoomEntity?> GetRoomWithDetails(Guid roomId);
	Task<IEnumerable<RoomEntity>> GetRoomsWithDetails(IEnumerable<Guid> roomIds);
	Task<RoomEntity?> GetRoomWithDesksAndHistoryAndReservationsAndLocation(Guid id);
	Task<IEnumerable<RoomEntity>> GetHotDesks();
	Task<IEnumerable<RoomEntity>> GetFilteredHotDesks(DateTime start, DateTime end, string? roomName,
		string? buildingName, int? freeDesksRangeMin, int? freeDesksRangeMax);
	Task<int> GetHotDesksCount();
}
