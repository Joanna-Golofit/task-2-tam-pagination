import { ErrorCodes } from '../../services/common/models';
import { IGlobalState, SHOW_NOTIFY, GlobalActionTypes, SET_LOADING, SHOW_ERROR_NOTIFY,
  DISPOSE_ERROR_NOTIFY,
  GET_LOGGED_USER_DATA,
  GET_LOGGED_USER_DATA_SUCCESS,
  LoggedUserData } from './types';

const initialState: IGlobalState = {
  isLoadingCounter: 0,
  notifyState: {
    errorCode: ErrorCodes.NoError,
    isOpen: false,
    title: '',
    text: '',
    negative: false,
    status: null,
  },
  loggedUserData: new LoggedUserData('', '', [], []),
};

export function globalReducer(state = initialState, action: GlobalActionTypes): IGlobalState {
  switch (action.type) {
    case SHOW_NOTIFY: {
      return { ...state,
        notifyState: {
          errorCode: ErrorCodes.NoError,
          text: action.text,
          title: action.title,
          isOpen: true,
          negative: action.negative } }; }
    case SHOW_ERROR_NOTIFY:
      return { ...state,
        notifyState: {
          errorCode: action.errorCode,
          text: '',
          title: '',
          isOpen: true,
          negative: true,
          status: action.status } };
    case DISPOSE_ERROR_NOTIFY:
      return { ...state,
        notifyState: {
          ...state.notifyState,
          isOpen: false },
        isLoadingCounter: 0,
      };
    case SET_LOADING: {
      if (state.isLoadingCounter + (action.isShow ? 1 : -1) < 0) {
        throw new Error(`isLoadingCounter is less than 0: ${state.isLoadingCounter + (action.isShow ? 1 : -1)}`);
      }
      return { ...state,
        isLoadingCounter: state.isLoadingCounter + (action.isShow ? 1 : -1),
        notifyState: {
          ...state.notifyState,
          isOpen: false } };
    }
    case GET_LOGGED_USER_DATA:
      return { ...state };
    case GET_LOGGED_USER_DATA_SUCCESS:
      return { ...state,
        loggedUserData: new LoggedUserData(action.payload.id, action.payload.email, action.payload.roles, action.payload.reservationInfo) };
    default:
      return state;
  }
}
