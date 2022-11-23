import { HotDeskRespones, HotDesksDateFilters } from '../../services/hotDesks/models';
import {
  FETCH_HOT_DESKS,
  FETCH_HOT_DESKS_SUCCESS,
  IFetchHotDesksAction,
  IFetchHotDesksSuccessAction,
  IResetDateFilterAction,
  ISetDateFilterAction,
  RESET_DATE_FILTER,
  SET_DATE_FILTER,
} from './types';

export function fetchHotDesksAction(query: HotDesksDateFilters): IFetchHotDesksAction {
  return {
    type: FETCH_HOT_DESKS,
    query,
  };
}

export function fetchHotDesksSuccessAction(payload: HotDeskRespones): IFetchHotDesksSuccessAction {
  return {
    type: FETCH_HOT_DESKS_SUCCESS,
    payload,
  };
}

export function setDateFilterAction(payload: HotDesksDateFilters): ISetDateFilterAction {
  return {
    type: SET_DATE_FILTER,
    payload,
  };
}

export function resetDateFilterAction(): IResetDateFilterAction {
  return {
    type: RESET_DATE_FILTER,
  };
}
