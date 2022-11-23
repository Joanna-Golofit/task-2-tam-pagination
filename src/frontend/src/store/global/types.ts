/* eslint-disable no-empty-function */
/* eslint-disable no-useless-constructor */
import { ErrorCodes } from '../../services/common/models';
import { UserRole } from '../../services/user/enums';
import { LoggedUserDataDto, ReservationInfoDto } from '../../services/user/models';

export const SHOW_NOTIFY = 'SHOW_NOTIFY';
export const SET_LOADING = 'SET_LOADING';
export const SHOW_ERROR_NOTIFY = 'SHOW_ERROR_NOTIFY';
export const DISPOSE_ERROR_NOTIFY = 'DISPOSE_ERROR_NOTIFY';

export const GET_LOGGED_USER_DATA = 'GET_LOGGED_USER_DATA';
export const GET_LOGGED_USER_DATA_SUCCESS = 'GET_LOGGED_USER_DATA_SUCCESS';

export interface IGlobalState {
  isLoadingCounter: number;
  notifyState: INotifyState;
  loggedUserData: LoggedUserData;
}

export class LoggedUserData {
  constructor(public id: string, public email: string, private roles: UserRole[], public reservationInfo: ReservationInfoDto[]) {}
  public isTeamLeader(): boolean {
    return this.roles.some((x) => x === UserRole.TeamLeader);
  }
  public isUserAdmin(): boolean {
    return this.roles.some((x) => x === UserRole.Admin);
  }
  public isStandardUser(): boolean {
    return !(this.isTeamLeader() || this.isUserAdmin());
  }
  public hasRole(role: UserRole): boolean {
    return this.roles.some((x) => x === role);
  }
}
export interface IGetLoggedUserDataAction {
  type: typeof GET_LOGGED_USER_DATA;
}

export interface IGetLoggedUserDataSuccessAction {
  type: typeof GET_LOGGED_USER_DATA_SUCCESS;
  payload: LoggedUserDataDto;
}

export interface INotifyState {
  isOpen: boolean;
  title?: string;
  text: string;
  negative: boolean;
  errorCode: ErrorCodes;
  status?: string | null;
}

export interface IShowNotifyAction {
  type: typeof SHOW_NOTIFY;
  title?: string;
  text: string;
  negative: boolean;
}

export interface IShowErrorCodeInNotifyAction {
  type: typeof SHOW_ERROR_NOTIFY;
  errorCode: ErrorCodes;
  status?: string | null;
}

export interface IDisposeErrorCodeInNotifyAction {
  type: typeof DISPOSE_ERROR_NOTIFY;
}

export interface ISetLoadingAction {
  type: typeof SET_LOADING;
  isShow: boolean;
}

export type GlobalActionTypes = IShowNotifyAction
                              | ISetLoadingAction
                              | IShowErrorCodeInNotifyAction
                              | IDisposeErrorCodeInNotifyAction
                              | IGetLoggedUserDataAction
                              | IGetLoggedUserDataSuccessAction;
