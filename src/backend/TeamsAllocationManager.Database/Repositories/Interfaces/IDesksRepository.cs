using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamsAllocationManager.Database.Repositories.GenericRepository;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.Repositories.Interfaces;

public interface IDesksRepository : IAsyncRepository<DeskEntity>
{
	Task<DeskEntity?> GetDesk(Guid deskId);
	Task<IEnumerable<DeskEntity>> GetDeskForRoom(Guid roomId, IEnumerable<Guid> desksToDelete);
	Task<IEnumerable<DeskEntity>> GetDesks(IEnumerable<Guid> deskIds);
	Task<DeskEntity?> GetDeskWithHistoryAndBuildings(Guid deskId);
	Task<DeskEntity?> GetDeskWithLocationAndHistoryAndReservation(Guid deskId);
	Task<IEnumerable<DeskEntity>> GetRoomsDesksWithLocationAndHistoryAndReservation(Guid roomId);
	Task<IEnumerable<DeskEntity>> GetDesksToRelease(Guid roomId, Guid projectId);
	Task<IEnumerable<DeskEntity>> GetDeskForRoom(Guid roomId);
}
