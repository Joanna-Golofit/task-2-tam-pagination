import dayjs from 'dayjs';
import { TeamLeader } from '../employee/models';
import { DeskReservationDto, Building, DeskHistoryDto, RoomDeskDto } from '../room/models';

export const projectUrlPart: string = 'Project';
export const employeeUrlPart: string = 'Employee';

export type ProjectForDropdownDto = {
  id: string;
  name: string;
}

export type AssignEmployeesToDesksDto = {
  roomId: string;
  deskEmployeeDtos: DeskEmployeeDto[]
}

export type DeskEmployeeDto = {
  deskId: string,
  employeeId: string,
}

export type RemoveEmployeesFromDesksDto = {
  roomId: string;
  deskIds: string[];
}

export type Project = {
  id: string;
  name: string;
  teamLeaders: TeamLeader[];
  peopleCount: number;
  assignedPeopleCount: number;
  officeEmployeesCount: number;
  remoteEmployeesCount: number;
  hybridEmployeesCount: number;
  unassignedMembersCount: number;
  notSetMembersCount: number;
}

export type ProjectsFilterOptions = {
  teamLeaderIds?: string[],
  projectIds?: string[],
  unassignedPeopleMin?: number,
  unassignedPeopleMax?: number,
  peopleCountMin?: number,
  peopleCountMax?: number,
  showYourProjects?: boolean
  externalCompanies: boolean;
}

export type Employee = {
  id: string;
  name: string;
  surname: string;
  workmode: number;
}

export type ProjectDetails = {
  rooms: RoomForProjectDto[];
  endDate: dayjs.ConfigType;
  id: string;
  name: string;
  email: string;
  teamLeaders: TeamLeader[];
  peopleCount: number;
  assignedPeopleCount: number;
  employees: EmployeeForProjectDetailsDto[];
}

export type RoomForProjectDto = {
  id: string;
  building: Building;
  area: number;
  name: string;
  floor: number;
  capacity: number;
  occupiedDesksCount: number;
  desksInRoom: DeskForProjectDetailsDto[];
  sasTokenForRoomPlans: string;
  freeDesksCount: number;
  hotDesksCount: number;
}

export type DeskForProjectDetailsDto = {
  id: string;
  number: number;
  isHotDesk: boolean;
  deskHistory: DeskHistoryDto[];
  reservations: DeskReservationDto[];
  isEnabled: boolean;
}

export type EmployeeForProjectDetailsDto = {
  id: string;
  name: string;
  surname: string;
  workmode: number;
  projectsNames: string[];
  roomDeskDtos: RoomDeskDto[];
}

export type AddEmployeeToProjectDto = {
  companyId: string;
  employeeCount: number;
}

export type RemoveEmployeeFromProjectDto = {
  companyId: string;
  employeeId: string;
}

export type AddProjectDto = {
  name: string;
  email: string;
  initialEmployeeCount: number;
}
