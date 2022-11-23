using System;

namespace TeamsAllocationManager.Domain.Models;

public class EmployeeDeskHistoryEntity : Entity
{
	public Guid? EmployeeId { get; set; }
	public EmployeeEntity? Employee { get; set; }
	public Guid DeskId { get; set; }
	public DeskEntity Desk { get; set; } = null!;
	public DateTime? From { get; set; }
	public DateTime? To { get; set; }

	public static EmployeeDeskHistoryEntity NewEntryFromDeskReservation(DeskReservationEntity reservation)
		=> new EmployeeDeskHistoryEntity
		{
			Employee = reservation.Employee,
			EmployeeId = reservation.EmployeeId,
			Desk = reservation.Desk,
			DeskId = reservation.DeskId,
			From = reservation.ReservationStart,
			To = reservation.ReservationEnd
		};

	public static EmployeeDeskHistoryEntity NewHistoryEntryForDeskReservation(DeskReservationEntity reservation) =>
		new EmployeeDeskHistoryEntity
		{
			EmployeeId = reservation.EmployeeId,
			DeskId = reservation.DeskId,
			From = reservation.ReservationStart,
			To = null,
		};
}
