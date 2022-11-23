using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamsAllocationManager.Domain.Models;

public class DeskReservationEntity : Entity
{
	public Guid DeskId { get; set; }

	public DeskEntity Desk { get; set; } = null!;

	public Guid EmployeeId { get; set; }

	public EmployeeEntity Employee { get; set; } = null!;

	public Guid? CreatedById { get; set; }

	public EmployeeEntity? CreatedBy { get; set; }

	public DateTime ReservationStart { get; set; }

	public DateTime? ReservationEnd { get; set; }

	public bool IsSchedule { get; set; }

	public IEnumerable<DayOfWeek> ScheduledWeekdays { get; set; } = new List<DayOfWeek>();

	public static DeskReservationEntity NewHotDeskReservation(DateTime startDate, DateTime endDate, DeskEntity desk,
		EmployeeEntity employee, EmployeeEntity? createdBy = null)
		=> new()
		{
			DeskId = desk.Id,
			Desk = desk,
			EmployeeId = employee.Id,
			Employee = employee,
			CreatedById = createdBy?.Id ?? employee.Id,
			CreatedBy = createdBy ?? employee,
			ReservationStart = startDate.Date,
			ReservationEnd = endDate.Date.AddDays(1).AddMilliseconds(-1),
			IsSchedule = false
		};

	public static DeskReservationEntity NewDeskReservation(DateTime from, IEnumerable<DayOfWeek> scheduledWeekdays,
		DeskEntity desk, EmployeeEntity? employee, EmployeeEntity? createdBy = null) =>
		new()
		{
			DeskId = desk.Id,
			Desk = desk,
			EmployeeId = employee!.Id,
			Employee = employee,
			CreatedById = createdBy?.Id ?? employee.Id,
			CreatedBy = createdBy ?? employee,
			ReservationStart = from,
			ReservationEnd = null,
			IsSchedule = true,
			ScheduledWeekdays = scheduledWeekdays,
		};

	public bool IsOnDate(DateTime day) => IsWithinReservationPeriod(day) || IsWithinScheduledWeekdays(day);

	public bool IsWithinPeriod(DateTime startDate, DateTime endDate)
		=> !IsSchedule && startDate < ReservationStart && endDate > ReservationEnd;

	public bool ContainsAllWeekDays()
	{
		return ScheduledWeekdays.Contains(DayOfWeek.Monday)
		       && ScheduledWeekdays.Contains(DayOfWeek.Tuesday)
		       && ScheduledWeekdays.Contains(DayOfWeek.Wednesday)
		       && ScheduledWeekdays.Contains(DayOfWeek.Thursday)
		       && ScheduledWeekdays.Contains(DayOfWeek.Friday);
	}

	private bool IsWithinReservationPeriod(DateTime day) =>
		!IsSchedule && day.Date >= ReservationStart && day.Date <= ReservationEnd;

	private bool IsWithinScheduledWeekdays(DateTime day) => IsSchedule && ScheduledWeekdays.Contains(day.DayOfWeek);

	public bool CanBeDeletedBy(string email)
		=> Employee.Email == email || (CreatedBy != null && CreatedBy.Email == email);
}

public static class DeskReservationEntityExtension
{
	public static bool AnyOnDate(this IEnumerable<DeskReservationEntity> reservations, DateTime day)
		=> reservations.Any(r => r.IsOnDate(day));

	public static bool AnyOnDateRange(this IEnumerable<DeskReservationEntity> reservations, DateTime? startDate,
		DateTime? endDate)
		=> reservations.Any(r =>
			r.IsOnDate(startDate ?? DateTime.Now) || r.IsOnDate(endDate ?? DateTime.MaxValue) ||
			r.IsWithinPeriod(startDate ?? DateTime.Now, endDate ?? DateTime.MaxValue));

	public static bool AnyOnWeekdays(this IEnumerable<DeskReservationEntity> reservations,
		IEnumerable<DayOfWeek> weekdays)
		=> reservations.Any(r => r.IsSchedule && r.ScheduledWeekdays.Intersect(weekdays).Any());
}