import { EmployeeBasicInfoDto } from '../room/models';

export const hotDesksUrlPart: string = 'HotDesk';

export type Room = {
  id: string;
  freeHotDeskCount: number;
  hotDesksCount: number;
  area: number;
  building: Building;
  floor: number;
  name: string;
}

export type Building = {
  id: string;
  name: string,
}

export type HotDeskRespones = {
  rooms: Room[];
  buildings: Room[];
  maxFloor: number;
  areaMinLevelPerPerson: number;
}

export type HotDesksDateFilters = {
  startDate: Date;
  endDate: Date;
}

export type HotDeskFilters = {
  room?: string,
  buildingIds?: string[],
  floors?: number[],
  freeHotDeskMin?: number,
  freeHotDeskMax?: number,
}

export type HotDeskReservationDto = {
  deskId: string,
  reservationStart: Date,
  reservationEnd: Date,
  reservingEmployee?: string,
}

export type DeskLocationDto = {
 id: string,
 roomId: string,
 locationName: string,
 deskNumber: number,
}

export type HotDeskReservationInfoDto = {
  id: string,
  desk: DeskLocationDto,
  employee: EmployeeBasicInfoDto,
  createdBy: EmployeeBasicInfoDto,
  reservationStart: Date,
  reservationEnd: Date,
}
