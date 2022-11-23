import dayjs from 'dayjs';
import { FETCH_HOT_DESKS, FETCH_HOT_DESKS_SUCCESS, HotDesksActionTypes, IHotDesksState, SET_DATE_FILTER, RESET_DATE_FILTER } from './types';

const initialState: IHotDesksState = {
  response: {
    rooms: [],
    buildings: [],
    maxFloor: 0,
    areaMinLevelPerPerson: 0,
  },
  filters: {
    startDate: new Date(),
    endDate: dayjs(new Date()).add(6, 'days').toDate(),
  },
  initFilters: {
    startDate: new Date(new Date().setHours(0, 0, 0, 0)),
    endDate: dayjs(new Date()).add(3650, 'days').toDate(),
  },
  initNonStdFilters: {
    startDate: new Date(new Date().setHours(0, 0, 0, 0)),
    endDate: dayjs(new Date()).add(3650, 'days').toDate(),
  },
};

export function hotDesksReducer(state = initialState, action: HotDesksActionTypes): IHotDesksState {
  switch (action.type) {
    case FETCH_HOT_DESKS:
      return { ...state };
    case FETCH_HOT_DESKS_SUCCESS:
      return { ...state, response: action.payload };
    case SET_DATE_FILTER:
      return { ...state, filters: action.payload };
    case RESET_DATE_FILTER:
      return {
        ...state,
        filters: {
          startDate: new Date(),
          endDate: dayjs(new Date()).add(6, 'days').toDate(),
        },
      };
    default:
      return state;
  }
}
