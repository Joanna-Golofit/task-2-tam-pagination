import { IFloorsState, FloorsActionTypes, FETCH_FLOORS, FETCH_FLOORS_SUCCESS } from './types';

const initialState: IFloorsState = {
  isLoading: false,
  floorsResponse: { buildings: [], floors: [], maxFloor: 0 },
};

export function floorsReducer(state = initialState, action: FloorsActionTypes): IFloorsState {
  switch (action.type) {
    case FETCH_FLOORS:
      return { ...state, isLoading: true };
    case FETCH_FLOORS_SUCCESS:
      return { ...state, isLoading: false, floorsResponse: action.payload };
    default:
      return state;
  }
}
