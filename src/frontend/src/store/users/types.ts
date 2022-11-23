import { UpdateEmployeeWorkspaceTypeDto } from '../../services/employee/models';
import { User, UsersForSearcherDto } from '../../services/user/models';

export const LOADING_USERS_SEARCHER = 'LOADING_USERS_SEARCHER';
export const CLEAN_QUERY = 'CLEAN_QUERY';
export const START_SEARCH = 'START_SEARCH';
export const FINISH_SEARCH = 'FINISH_SEARCH';
export const UPDATE_SELECTION = 'UPDATE_SELECTION';
export const GET_ALL_USERS_FOR_SEARCHER = 'GET_ALL_USERS_FOR_SEARCHER';
export const GET_ALL_USERS_FOR_SEARCHER_SUCCESS = 'GET_ALL_USERS_FOR_SEARCHER_SUCCESS';
export const UPDATE_USER_WORKMODE = 'UPDATE_USER_WORKMODE';

export interface IUsersState {
  loading: boolean,
  results: any[],
  value: string,
  users: User[];
}

export interface ISetLoadingUsersSearcher {
    type: typeof LOADING_USERS_SEARCHER;
    isLoading: boolean
}

export interface ICleanQuery {
    type: typeof CLEAN_QUERY;
}

export interface IStartSearch {
    type: typeof START_SEARCH;
    query: string
}

export interface IFinishSearch {
    type: typeof FINISH_SEARCH;
    results: any[]
}

export interface IUpdateSelection {
    type: typeof UPDATE_SELECTION;
    selection: any
}

export interface IGetAllUsersForSearcher {
    type: typeof GET_ALL_USERS_FOR_SEARCHER;
}

export interface IGetAllUsersForSearcherSuccess {
  type: typeof GET_ALL_USERS_FOR_SEARCHER_SUCCESS;
  payload: UsersForSearcherDto
}

export interface IUpdateUserWorkmode {
  type: typeof UPDATE_USER_WORKMODE;
  payload: UpdateEmployeeWorkspaceTypeDto[];
}

export type UsersActionTypes = ISetLoadingUsersSearcher
                            | ICleanQuery
                            | IStartSearch
                            | IFinishSearch
                            | IUpdateSelection
                            | IGetAllUsersForSearcher
                            | IGetAllUsersForSearcherSuccess
                            | IUpdateUserWorkmode;
