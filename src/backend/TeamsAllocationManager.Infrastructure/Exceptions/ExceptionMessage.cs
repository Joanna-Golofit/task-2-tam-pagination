using System;
using System.Collections.Generic;

namespace TeamsAllocationManager.Infrastructure.Exceptions
{
	public static class ExceptionMessage
	{
		public const string InvalidUserRole_OnlyAdminOrTeamLeaderCanReserveForOthers = "exception.invalidUserRole.onlyAdminOrTeamLeaderCanReserveForOthers";
		public const string InvalidArgument_IncorrectWeekdays = "exception.invalidArgument.incorrectWeekdays";
		public const string InvalidArgument_IncorrectFreeDeskRangeValues = "exception.invalidArgument.IncorrectFreeDeskRangeValues";
		public const string InvalidArgument_OccupiedDeskRangeValues = "exception.invalidArgument.OccupiedDeskRangeValues";
		public const string InvalidArgument_IncorrectDeskCapacityRangeValues = "exception.invalidArgument.IncorrectDeskCapacityRangeValues";
		public const string InvalidArgument_InvalidWorkspaceType = "exception.invalidArgument.InvalidWorkspaceType";
		public const string HotDesks_ReservationExistsForDates = "exception.hotDesks.reservationExistsForDates";
		public const string HotDesks_CannotCreateScheduledReservation = "exception.hotDesks.cannotCreateScheduledReservation";
		public const string HotDesks_CannotReserveForPastDate = "exception.hotDesks.cannotReserveForPastDate";
		public const string HotDesks_ReservationEndMustBeGraterThanStart = "exception.hotDesks.reservationEndMustBeGraterThanStart";
		public const string HotDesks_EndDateLimit30days = "exception.hotDesks.endDateLimit30days";
		public const string HotDesks_EndDateLimit7days = "exception.hotDesks.endDateLimit7days";
		public const string HotDesks_EmployeeHasReservation = "exception.hotDesks.employeeHasReservation";
		public const string HotDesks_CannotSetDisabledDesk = "exception.hotDesks.cannotSetDisabledDesk";
		public const string HotDesks_StartDateGreaterThanEndDate = "exception.hotDesks.startDateGreaterThanEndDate";
		public const string HotDesks_StartDateOrEndDateIsEmpty = "exception.hotDesks.startDateOrEndDateIsEmpty";
		public const string HotDesks_ReservationCannotBeEmpty = "exception.hotDesks.reservationCannotBeEmpty";
		public const string HotDesks_CannotRemoveOtherReservation = "exception.hotDesks.cannotRemoveOtherReservation";
		public const string HotDesks_DeskNotAvailable = "exception.hotDesks.deskNotAvailable";
		public const string Equipments_DuplicateName = "exception.equipments.duplicateName";
		public const string Equipments_NameLengthExceeded = "exception.equipments.nameLengthExceeded";

		public static string GetMessage(string key)
		{
			var hasValue = _messages.TryGetValue(key, out var message);
			if (hasValue)
			{
				return message ?? string.Empty;
			}
			else
			{
				throw new Exception($"Translation key '{key}' does not exist in ExceptionMessage's dictionary.");
			}
		}

		private static readonly Dictionary<string, string> _messages = new()
		{
			{ InvalidUserRole_OnlyAdminOrTeamLeaderCanReserveForOthers, "Only Admins or Team Leaders can create reservations for other Employees." },
			{ InvalidArgument_IncorrectWeekdays, "Incorrect weekdays. Allowed values are 0-6 (Sun-Sat)." },
			{ HotDesks_ReservationExistsForDates, "There's already a reservation within given dates." },
			{ HotDesks_CannotCreateScheduledReservation, "Cannot create scheduled reservation for Hot Desk." },
			{ HotDesks_CannotReserveForPastDate, "Reservation cannot be created for past date." },
			{ HotDesks_ReservationEndMustBeGraterThanStart, "Reservation end date must be greater than start date." },
			{ HotDesks_EndDateLimit30days, "EndDate date exceedes user's limit of 30 days." },
			{ HotDesks_EndDateLimit7days, "EndDate date exceedes user's limit of 7 days." },
			{ HotDesks_EmployeeHasReservation, "This employee has already a reservation within given dates." },
			{ HotDesks_CannotSetDisabledDesk, "Cannot set desk as hotdesk when desk is disabled." },
			{ HotDesks_StartDateGreaterThanEndDate, "StartDate is greater than EndDate." },
			{ HotDesks_ReservationCannotBeEmpty, "Reservation cannot be empty. Select at least one weekday." },
			{ HotDesks_CannotRemoveOtherReservation, "Cannot remove other user's reservations." },
			{ HotDesks_DeskNotAvailable, "Selected desk is not available anymore." },
			{ Equipments_DuplicateName, "Non-IT equipment with this name already exist." },
			{ Equipments_NameLengthExceeded, "Name cannot contain more than 100 characters." },
			{ HotDesks_StartDateOrEndDateIsEmpty, "Reservation dates cannot be empty."},
			{ InvalidArgument_IncorrectFreeDeskRangeValues, "Minimum value of free desks range cannot be greater than maximum"},
			{ InvalidArgument_IncorrectDeskCapacityRangeValues, "Minimum value of desks capacity range cannot be greater than maximum" },
			{ InvalidArgument_OccupiedDeskRangeValues, "Minimum value of occupied desks range cannot be greater than maximum" },
			{ InvalidArgument_InvalidWorkspaceType, "Workspace type has to be in correct range (0-office, 1-remote, 2-partially remote)"}
		};
	}
}
