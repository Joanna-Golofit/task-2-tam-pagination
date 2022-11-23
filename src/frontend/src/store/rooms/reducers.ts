import { ErrorCodes } from '../../services/common/models';
import { IRoomsState, RoomsActionTypes, FETCH_ROOMS, FETCH_ROOMS_SUCCESS } from './types';

const initialState: IRoomsState = {
  isLoading: false,
  roomsResponse: { buildings: [], maxFloor: 0, rooms: [], areaMinLevelPerPerson: 0, errorCode: ErrorCodes.NoError },
};

export function roomsReducer(state = initialState, action: RoomsActionTypes): IRoomsState {
  switch (action.type) {
    case FETCH_ROOMS:
      return { ...state };
    case FETCH_ROOMS_SUCCESS:
      return { ...state, roomsResponse: action.payload };
    default:
      return state;
  }
}
