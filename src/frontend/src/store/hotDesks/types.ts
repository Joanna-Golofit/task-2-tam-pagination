import { HotDeskRespones, HotDesksDateFilters } from '../../services/hotDesks/models';

export const FETCH_HOT_DESKS = 'FETCH_HOT_DESKS';
export const FETCH_HOT_DESKS_SUCCESS = 'FETCH_HOT_DESKS_SUCCESS';

export const SET_DATE_FILTER = 'SET_DATE_FILTER';
export const RESET_DATE_FILTER = 'RESET_DATE_FILTER';

export interface IHotDesksState {
  response: HotDeskRespones;
  filters: HotDesksDateFilters,
  initFilters: HotDesksDateFilters,
  initNonStdFilters: HotDesksDateFilters,
}

export interface IFetchHotDesksAction {
  type: typeof FETCH_HOT_DESKS;
  query: HotDesksDateFilters;
}

export interface IFetchHotDesksSuccessAction {
  type: typeof FETCH_HOT_DESKS_SUCCESS;
  payload: HotDeskRespones;
}

export interface ISetDateFilterAction {
  type: typeof SET_DATE_FILTER;
  payload: HotDesksDateFilters;
}

export interface IResetDateFilterAction {
  type: typeof RESET_DATE_FILTER;
}

export type HotDesksActionTypes = IFetchHotDesksAction
                         | IFetchHotDesksSuccessAction
                         | ISetDateFilterAction
                         | IResetDateFilterAction;
