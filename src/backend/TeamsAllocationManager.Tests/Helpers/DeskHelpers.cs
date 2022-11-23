using System;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Tests.Helpers;

internal static class DeskHelpers
{
	public static DeskEntity CreateDeskWithReservation(RoomEntity room, int deskNumber, EmployeeEntity employee)
	{
		var desk = new DeskEntity {Room = room, Number = 3, IsEnabled = true};
		var deskReservation = DeskReservationEntity.NewDeskReservation(DateTime.Now,
			new[] {DayOfWeek.Monday, DayOfWeek.Tuesday}, desk, employee);
		desk.DeskReservations.Add(deskReservation);

		return desk;
	}

	public static DeskEntity CreateHotDeskWithReservation(RoomEntity room, int deskNumber, EmployeeEntity employee)
	{
		var desk = new DeskEntity {Room = room, Number = deskNumber, IsEnabled = true, IsHotDesk = true};
		var hotDeskReservation =
			DeskReservationEntity.NewHotDeskReservation(DateTime.Today, DateTime.Today.AddDays(7), desk, employee);
		desk.DeskReservations.Add(hotDeskReservation);

		return desk;
	}

	public static DeskEntity CreateDeskWithReservationForWholeWeek(RoomEntity room, int deskNumber,
		EmployeeEntity employee)
	{
		var desk = new DeskEntity {Room = room, Number = 3, IsEnabled = true};
		var deskReservation = DeskReservationEntity.NewDeskReservation(DateTime.Now,
			new[] {DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday},
			desk, employee);
		desk.DeskReservations.Add(deskReservation);

		return desk;
	}
}