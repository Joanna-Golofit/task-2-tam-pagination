import {
  IFetchFloorsAction,
  FETCH_FLOORS,
  IFetchFloorsSuccessAction,
  FETCH_FLOORS_SUCCESS,
  Floors,
} from './types';

export function fetchFloors(): IFetchFloorsAction {
  return {
    type: FETCH_FLOORS,
  };
}

export function fetchFloorsSuccess(floors: Floors): IFetchFloorsSuccessAction {
  return {
    type: FETCH_FLOORS_SUCCESS,
    payload: floors,
  };
}
