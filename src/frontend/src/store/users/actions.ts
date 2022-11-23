import { UsersForSearcherDto } from '../../services/user/models';
import { CLEAN_QUERY, ICleanQuery, ISetLoadingUsersSearcher, LOADING_USERS_SEARCHER, IStartSearch,
  START_SEARCH, IFinishSearch, FINISH_SEARCH, IUpdateSelection, UPDATE_SELECTION,
  GET_ALL_USERS_FOR_SEARCHER, GET_ALL_USERS_FOR_SEARCHER_SUCCESS,
  IGetAllUsersForSearcher, IGetAllUsersForSearcherSuccess, IUpdateUserWorkmode, UPDATE_USER_WORKMODE } from './types';

export function setLoadingUsersSearcher(isLoading: boolean): ISetLoadingUsersSearcher {
  return {
    type: LOADING_USERS_SEARCHER,
    isLoading,
  };
}

export function cleanQuery(): ICleanQuery {
  return {
    type: CLEAN_QUERY,
  };
}

export function startSearch(query: string): IStartSearch {
  return {
    type: START_SEARCH,
    query,
  };
}

export function finishSearch(results: any[]): IFinishSearch {
  return {
    type: FINISH_SEARCH,
    results,
  };
}

export function updateSelection(selection: any): IUpdateSelection {
  return {
    type: UPDATE_SELECTION,
    selection,
  };
}

export function getAllUsersForSearcher(): IGetAllUsersForSearcher {
  return {
    type: GET_ALL_USERS_FOR_SEARCHER,
  };
}

export function getAllUsersForSearcherSuccess(payload: UsersForSearcherDto): IGetAllUsersForSearcherSuccess {
  return {
    type: GET_ALL_USERS_FOR_SEARCHER_SUCCESS,
    payload,
  };
}

export function updateUserWorkmode(payload: any): IUpdateUserWorkmode {
  return {
    type: UPDATE_USER_WORKMODE,
    payload,
  };
}
