import { ErrorCodes } from '../../services/common/models';
import { LoggedUserDataDto } from '../../services/user/models';
import { SHOW_NOTIFY, IShowNotifyAction, ISetLoadingAction,
  SET_LOADING, SHOW_ERROR_NOTIFY, IShowErrorCodeInNotifyAction, IDisposeErrorCodeInNotifyAction,
  DISPOSE_ERROR_NOTIFY,
  IGetLoggedUserDataAction,
  IGetLoggedUserDataSuccessAction,
  GET_LOGGED_USER_DATA,
  GET_LOGGED_USER_DATA_SUCCESS } from './types';

export function showNotifyAction(text: string, negative: boolean = false, title?: string): IShowNotifyAction {
  return {
    type: SHOW_NOTIFY,
    title,
    text,
    negative,
  };
}

export function showErrorCodeInNotifyAction(errorCode: ErrorCodes, status?: string | null): IShowErrorCodeInNotifyAction {
  return {
    type: SHOW_ERROR_NOTIFY,
    errorCode,
    status,
  };
}

export function disposeErrorCodeInNotifyAction(): IDisposeErrorCodeInNotifyAction {
  return {
    type: DISPOSE_ERROR_NOTIFY,
  };
}

export function setLoadingAction(isShow: boolean): ISetLoadingAction {
  return {
    type: SET_LOADING,
    isShow,
  };
}

export function getLoggedUserDataAction(): IGetLoggedUserDataAction {
  return {
    type: GET_LOGGED_USER_DATA,
  };
}

export function getLoggedUserDataSuccessAction(loggedUserDataDto: LoggedUserDataDto)
: IGetLoggedUserDataSuccessAction {
  return {
    type: GET_LOGGED_USER_DATA_SUCCESS,
    payload: loggedUserDataDto,
  };
}
