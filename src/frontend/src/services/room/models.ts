import { ErrorCodes } from '../common/models';

export const roomUrlPart: string = 'Room';

export type RoomDetailsDto = {
  id: string;
  area: number;
  building: Building;
  capacity: number;
  floor: number;
  name: string;
  occupiedDesksCount: number;
  areaMinLevelPerPerson: number;
  sasTokenForRoomPlans: string;
  errorCode: ErrorCodes;
  inactiveProjectsAssignedEmployeesCount: number;
  desksInRoom: DeskForRoomDetailsDto[];
  projectsInRoom: ProjectForRoomDetailsDto[];
  freeDesksCount: number;
}

export type DeskForRoomDetailsDto = {
  id: string;
  number: number;
  isHotDesk: boolean;
  deskHistory: DeskHistoryDto[];
  reservations: DeskReservationDto[];
  isEnabled: boolean;
}

export type DeskReservationDto = {
  scheduledWeekdays: number[],
  reservationDate: Date,
  reservationEnd: Date,
  employee: EmployeeForRoomDetailsDto,
  id: string
}

export type EmployeeBasicInfoDto = {
  id: string,
  name: string,
  surname: string,
  email: string,
}

export type ProjectForRoomDetailsDto = {
  id: string;
  name: string;
  teamLeaders: TeamLeaderDto[];
}

export type AllocateDesksDto = {
  roomId: string;
  deskIds: Array<string>;
}

export type AddDesksDto = {
  roomId: string;
  firstDeskNumber: number;
  numberOfDesks: number;
}

export type RoomsDto = {
  buildings: BuildingDto[];
  maxFloor: number;
  areaMinLevelPerPerson: number;
  errorCode: ErrorCodes;
  rooms: RoomDto[]
}

export type RoomDto = {
  id: string;
  area: number;
  building: BuildingDto;
  capacity: number;
  floor: number;
  name: string;
  occupiedDesksCount: number;
  hotDesksCount: number;
  freeDesksCount: number;
  disabledDesksCount: number
}

export type BuildingDto = {
  id: string;
  name: string;
}

export type ReserveDeskDto = {
  deskId: string;
  employeeId: string;
  scheduledWeekdays: number[];
}

export type UpdateReservationDto = {
  reservationId: string;
  employeeId: string;
  scheduledWeekdays: number[];
}

export type ReleaseDeskEmployeeDto = {
  employeeId: string;
  deskId: string;
}

export type EmployeeForRoomDetailsDto = {
  id: string;
  name: string;
  surname: string;
  projectsNames: string[];
  roomDeskDtos: RoomDeskDto[]
}

export type TeamLeaderDto = {
  id: string;
  name: string;
  surname: string;
  email: string;
}

export type SetHotDeskDto = {
  deskId: string;
  roomId: string;
  isHotDesk: boolean;
}

export type ToggleDeskIsEnabledDto = {
  desksIds: string[];
  isEnabled: boolean;
}

export type SetRoomHotDeskDto = {
  roomId: string;
  isHotDesk: boolean;
}

export type SharedDeskDto = {
  label: string,
  value: number,
  selected: boolean,
  disabled: boolean
}

export type Building = {
  id: string;
  name: string;
}

export type RoomDeskDto = {
  roomId: string;
  roomName: string;
  deskId: string;
  deskNumber: number;
}

export type DeskHistoryDto = {
  id: string;
  deskId: string;
  employeeId: string;
  employeeName: string;
  employeeSurname: string;
  from: Date;
  to: Date;
}
