import { IFetchRoomsAction, FETCH_ROOMS, IFetchRoomsSuccessAction, FETCH_ROOMS_SUCCESS } from './types';
import { RoomsDto } from '../../services/room/models';

export function fetchRooms(): IFetchRoomsAction {
  return {
    type: FETCH_ROOMS,
  };
}

export function fetchRoomsSuccess(rooms: RoomsDto): IFetchRoomsSuccessAction {
  return {
    type: FETCH_ROOMS_SUCCESS,
    payload: rooms,
  };
}
