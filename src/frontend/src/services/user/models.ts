import { ErrorCodes } from '../common/models';
import { EmployeeType, UserRole, WorkspaceType } from './enums';

export const userUrlPart: string = 'User';

export type UserDetailsDto = {
  id: string;
  email: string;
  employeeName: string;
  employeeSurname: string;
  employeeType: EmployeeType;
  workspaceType: WorkspaceType;
  isExternal?: boolean;
  locations: UserLocationDto[];
  projects: UserProjectDto[];
  reservationInfo: ReservationInfoDto[];
  ledProjectsReservationInfo: ReservationInfoDto[];
  errorCode: ErrorCodes;
}

export type UserLocationDto = {
  roomId: string;
  buildingName: string;
  floorNumber: number;
  roomName: string;
  deskNumber: number;
  locationName: string;
}

export type UserProjectDto = {
  id: string;
  name: string;
  teamLeadersNames: string[];
}

export type UsersForSearcherDto = {
  users: UserForSearcherDto[];
}

export type UserForSearcherDto = {
  id: string;
  displayName: string;
  email: string;
  workspaceType: WorkspaceType | null;
  projectsNames: string[];
}

export type User = {
  title: string,
  description: string,
  workspace: string,
  id: string,
  key: string,
  projects: string[]
}

export type LoggedUserDataDto = {
  id: string,
  email: string,
  roles: UserRole[]
}

export type ReservationInfoDto = {
  id: string,
  desk: UserLocationDto,
  employeeName: string,
  reservationStart: Date,
  reservationEnd: Date,
}
