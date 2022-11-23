import {
  HotDeskDetailsActionTypes,
  IHotDeskReservationModalState,
  OPEN_RESERVATION_MODAL,
  CLOSE_RESERVATION_MODAL,
  RESERVE_HOTDESK_SUCCESS,
  GET_ACTIVE_RESERVATIONS_FOR_DESK_SUCCESS,
  GET_ACTIVE_RESERVATIONS_FOR_EMPLOYEE_SUCCESS,
  REMOVE_HOTDESK_RESERVATION_SUCCESS,
} from './types';

const initialState: IHotDeskReservationModalState = {
  deskReservations: [],
  employeeReservations: [],
  isOpen: false,
  deskId: null,
  deskNo: null,
};

export function hotDeskDetailsReducer(state = initialState, action: HotDeskDetailsActionTypes): IHotDeskReservationModalState {
  switch (action.type) {
    case OPEN_RESERVATION_MODAL:
      return { ...state, isOpen: true, deskId: action.deskId, deskNo: action.deskNo };

    case CLOSE_RESERVATION_MODAL:
      return { ...state, isOpen: false };

    case RESERVE_HOTDESK_SUCCESS:
      return { ...state, isOpen: false };

    case GET_ACTIVE_RESERVATIONS_FOR_DESK_SUCCESS:
      return { ...state, deskReservations: action.payload };

    case GET_ACTIVE_RESERVATIONS_FOR_EMPLOYEE_SUCCESS:
      return { ...state, employeeReservations: action.payload };

    case REMOVE_HOTDESK_RESERVATION_SUCCESS:
      return {
        ...state,
        employeeReservations: state.employeeReservations.filter((er) => er.id !== action.reservationId),
        deskReservations: state.deskReservations.filter((dr) => dr.id !== action.reservationId),
      };

    default:
      return state;
  }
}
