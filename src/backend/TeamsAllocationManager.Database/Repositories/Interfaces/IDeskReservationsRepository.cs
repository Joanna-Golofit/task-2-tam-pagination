using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamsAllocationManager.Database.Repositories.GenericRepository;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.Repositories.Interfaces;

public interface IDeskReservationsRepository : IAsyncRepository<DeskReservationEntity>
{
	Task<DeskReservationEntity?> GetReservation(Guid id);

	Task<DeskReservationEntity?> GetReservationWithRoomDetails(Guid id);

	Task<IEnumerable<DeskReservationEntity>> GetHotDeskReservationsForEmployee(Guid employeeId);

	Task<IEnumerable<DeskReservationEntity>> GetDeskReservationsForEmployee(Guid employeeId);

	Task<IEnumerable<DeskReservationEntity>> GetHotDeskReservations(Guid deskId);
	Task<IEnumerable<DeskReservationEntity>> GetDeskReservations(Guid deskId);

	Task<IEnumerable<DeskReservationEntity>> GetOldReservations();

	Task<IEnumerable<DeskReservationEntity>> GetReservationToRemind(int numberOfDaysAheadReservation);
}
