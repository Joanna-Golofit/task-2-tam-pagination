using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamsAllocationManager.Domain.Models;

public class DeskEntity : Entity
{
	public Guid RoomId { get; set; }

	public RoomEntity Room { get; set; } = null!;

	public int Number { get; set; }

	public bool IsHotDesk { get; set; }

	public IList<DeskReservationEntity> DeskReservations { get; set; } = new List<DeskReservationEntity>();

	public bool IsEnabled { get; set; }

	public IList<EmployeeDeskHistoryEntity> EmployeeDeskHistory { get; set; } = new List<EmployeeDeskHistoryEntity>();

	public void ReleaseDesk() =>
		DeskReservations.Where(dr => dr.IsSchedule).ToList().ForEach(dr => ReleaseDesk(dr.EmployeeId));

	public void ReleaseDesk(Guid employeeId)
	{
		var deskReservation = DeskReservations.Where(dr => dr.IsSchedule && dr.EmployeeId == employeeId)
		                                      .SingleOrDefault();

		DeskReservations.Remove(deskReservation!);

		var historyEntry = EmployeeDeskHistory.OrderByDescending(edh => edh.Created)
		                                      .FirstOrDefault(edh => edh.EmployeeId == employeeId);

		if (historyEntry != null)
		{
			historyEntry.To = DateTime.Now;
		}
	}

	public void ReserveDesk(DeskReservationEntity deskReservation)
	{
		DeskReservations.Add(deskReservation);
		EmployeeDeskHistory.Add(EmployeeDeskHistoryEntity.NewHistoryEntryForDeskReservation(deskReservation));
	}

	public void SetHotDeskStatus(bool isHotDesk)
	{
		IsHotDesk = isHotDesk;

		if (isHotDesk)
		{
			ReleaseDesk();
		}
		else
		{
			DeskReservations = new List<DeskReservationEntity>();
		}
	}

	public void ToggleIsEnabled()
	{
		IsEnabled = !IsEnabled;
		if (!IsEnabled)
		{
			ReleaseDesk();
			SetHotDeskStatus(false);
		}
	}

	public bool AvailableInPeriod(DateTime? from, DateTime? to)
		=> DeskReservations.Count == 0 || EachDay(from, to).Any(day => !DeskReservations.AnyOnDate(day));

	public bool AvailableOnDateRange(DateTime? startDate, DateTime? endDate)
		=> DeskReservations.Count == 0 || !DeskReservations.AnyOnDateRange(startDate, endDate);

	public bool AvailableInWeekdays(IEnumerable<DayOfWeek> weekdays)
		=> DeskReservations.Count == 0 || !DeskReservations.AnyOnWeekdays(weekdays);

	public static IEnumerable<DateTime> EachDay(DateTime? from, DateTime? to)
	{
		for (var day = (from ?? default).Date; day.Date <= (to ?? default).Date; day = day.AddDays(1))
		{
			yield return day;
		}
	}
}