export type Floors = {
  buildings: Building[];
  maxFloor: number;
  floors: Floor[];
}

export type Floor = {
  id: string;
  building: Building;
  floor: number;
  roomCount: number;
  capacity: number;
  occupiedDesks: number;
  area: number;
}

export type Building = {
  id: string;
  name: string;
}

export const FETCH_FLOORS = 'FETCH_FLOORS';
export const FETCH_FLOORS_SUCCESS = 'FETCH_FLOORS_SUCCESS';

export interface IFloorsState {
  isLoading: boolean;
  floorsResponse: Floors;
}

export interface IFetchFloorsAction {
  type: typeof FETCH_FLOORS;
}

export interface IFetchFloorsSuccessAction {
  type: typeof FETCH_FLOORS_SUCCESS;
  payload: Floors;
}

export type FloorsActionTypes = IFetchFloorsAction | IFetchFloorsSuccessAction;
