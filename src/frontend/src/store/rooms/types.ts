import { RoomsDto } from '../../services/room/models';

export const FETCH_ROOMS = 'FETCH_ROOMS';
export const FETCH_ROOMS_SUCCESS = 'FETCH_ROOMS_SUCCESS';

export interface IRoomsState {
  isLoading: boolean;
  roomsResponse: RoomsDto;
}

export interface IFetchRoomsAction {
  type: typeof FETCH_ROOMS;
}

export interface IFetchRoomsSuccessAction {
  type: typeof FETCH_ROOMS_SUCCESS;
  payload: RoomsDto;
}

export type RoomsActionTypes = IFetchRoomsAction | IFetchRoomsSuccessAction;
