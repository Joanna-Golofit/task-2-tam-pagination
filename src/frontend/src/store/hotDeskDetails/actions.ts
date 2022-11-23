import { HotDeskReservationDto, HotDeskReservationInfoDto } from '../../services/hotDesks/models';
import {
  CLOSE_RESERVATION_MODAL,
  ICloseReservationModalAction,

  OPEN_RESERVATION_MODAL,
  IOpenReservationModalAction,

  RESERVE_HOTDESK,
  IReserveHotDesk,

  RESERVE_HOTDESK_SUCCESS,
  IReserveHotDeskSuccess,

  GET_ACTIVE_RESERVATIONS_FOR_DESK,
  IGetActiveReservationsForDeskAction,

  GET_ACTIVE_RESERVATIONS_FOR_DESK_SUCCESS,
  IGetActiveReservationsForDeskSuccessAction,

  GET_ACTIVE_RESERVATIONS_FOR_EMPLOYEE,
  IGetActiveReservationsForEmployeeAction,

  GET_ACTIVE_RESERVATIONS_FOR_EMPLOYEE_SUCCESS,
  IGetActiveReservationsForEmployeeSuccessAction,

  REMOVE_HOTDESK_RESERVATION,
  IRemoveHotDeskReservation,

  REMOVE_HOTDESK_RESERVATION_SUCCESS,
  IRemoveHotDeskReservationSuccess,
} from './types';

export function openReservationModalAction(deskId: string, deskNo: number): IOpenReservationModalAction {
  return {
    type: OPEN_RESERVATION_MODAL,
    deskId,
    deskNo,
  };
}

export function closeReservationModalAction(): ICloseReservationModalAction {
  return {
    type: CLOSE_RESERVATION_MODAL,
  };
}

export function reserveHotdesk(payload: HotDeskReservationDto, roomId: string): IReserveHotDesk {
  return {
    type: RESERVE_HOTDESK,
    payload,
    roomId,
  };
}

export function reserveHotdeskSuccess(reservationId: string): IReserveHotDeskSuccess {
  return {
    type: RESERVE_HOTDESK_SUCCESS,
    reservationId,
  };
}

export function getActiveReservationsForDeskAction(deskId: string) : IGetActiveReservationsForDeskAction {
  return {
    type: GET_ACTIVE_RESERVATIONS_FOR_DESK,
    deskId,
  };
}

export function getActiveReservationsForDeskSuccessAction(payload: HotDeskReservationInfoDto[]) : IGetActiveReservationsForDeskSuccessAction {
  return {
    type: GET_ACTIVE_RESERVATIONS_FOR_DESK_SUCCESS,
    payload,
  };
}

export function getActiveReservationsForEmployeeAction(employeeId: string) : IGetActiveReservationsForEmployeeAction {
  return {
    type: GET_ACTIVE_RESERVATIONS_FOR_EMPLOYEE,
    employeeId,
  };
}

export function getActiveReservationsForEmployeeSuccessAction(payload: HotDeskReservationInfoDto[]) : IGetActiveReservationsForEmployeeSuccessAction {
  return {
    type: GET_ACTIVE_RESERVATIONS_FOR_EMPLOYEE_SUCCESS,
    payload,
  };
}

export function removeHotDeskReservationAction(reservationId: string, roomId: string): IRemoveHotDeskReservation {
  return {
    type: REMOVE_HOTDESK_RESERVATION,
    reservationId,
    roomId,
  };
}

export function removeHotDeskReservationSuccessAction(reservationId: string): IRemoveHotDeskReservationSuccess {
  return {
    type: REMOVE_HOTDESK_RESERVATION_SUCCESS,
    reservationId,
  };
}
