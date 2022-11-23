import { DeskReservationDto } from '../services/room/models';

export { getReservedWeekdaysByOtherEmployees, getReservedWeekdaysByEmployee, getFreeWeekdays, getEmployeeReservationId };

function getReservedWeekdaysByOtherEmployees(deskReservations: DeskReservationDto[], employeeId: string) {
  return deskReservations
    .filter((dr) => dr.employee.id !== employeeId)
    .map((dr) => dr.scheduledWeekdays)
    .flat();
}

function getReservedWeekdaysByEmployee(deskReservations: DeskReservationDto[], employeId: string) {
  return deskReservations
    .filter((dr) => dr.employee.id === employeId)
    .map((dr) => dr.scheduledWeekdays)
    .flat();
}

function getFreeWeekdays(deskReservations: DeskReservationDto[]) {
  const reservedWeekDays = deskReservations
    .map((dr) => dr.scheduledWeekdays)
    .flat();

  return [1, 2, 3, 4, 5].filter((day) => !reservedWeekDays.includes(day));
}

function getEmployeeReservationId(deskReservations: DeskReservationDto[], employeeId: string): string {
  return deskReservations.find((dr) => dr.employee.id === employeeId)?.id || '';
}
