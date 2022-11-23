import {
  HotDeskReservationDto,
  HotDeskReservationInfoDto,
} from '../../services/hotDesks/models';

export const OPEN_RESERVATION_MODAL = 'OPEN_RESERVATION_MODAL';
export const CLOSE_RESERVATION_MODAL = 'CLOSE_RESERVATION_MODAL';

export const RESERVE_HOTDESK = 'RESERVE_HOTDESK';
export const RESERVE_HOTDESK_SUCCESS = 'RESERVE_HOTDESK_SUCCESS';

export const GET_ACTIVE_RESERVATIONS_FOR_DESK = 'GET_ACTIVE_RESERVATIONS_FOR_DESK';
export const GET_ACTIVE_RESERVATIONS_FOR_DESK_SUCCESS = 'GET_ACTIVE_RESERVATIONS_FOR_DESK_SUCCESS';

export const GET_ACTIVE_RESERVATIONS_FOR_EMPLOYEE = 'GET_ACTIVE_RESERVATIONS_FOR_EMPLOYEE';
export const GET_ACTIVE_RESERVATIONS_FOR_EMPLOYEE_SUCCESS = 'GET_ACTIVE_RESERVATIONS_FOR_EMPLOYEE_SUCCESS';

export const REMOVE_HOTDESK_RESERVATION = 'REMOVE_HOTDESK_RESERVATION';
export const REMOVE_HOTDESK_RESERVATION_SUCCESS = 'REMOVE_HOTDESK_RESERVATION_SUCCESS';

export interface IHotDeskReservationModalState {
    employeeReservations: HotDeskReservationInfoDto[];
    deskReservations: HotDeskReservationInfoDto[];
    isOpen: boolean;
    deskId: string | null;
    deskNo: number | null;
  }

export interface IOpenReservationModalAction {
    type: typeof OPEN_RESERVATION_MODAL;
    deskId: string;
    deskNo: number;
  }

export interface ICloseReservationModalAction {
    type: typeof CLOSE_RESERVATION_MODAL;
  }

export interface IReserveHotDesk {
    type: typeof RESERVE_HOTDESK;
    payload: HotDeskReservationDto;
    roomId: string,
  }

export interface IReserveHotDeskSuccess {
    type: typeof RESERVE_HOTDESK_SUCCESS;
    reservationId: string;
  }

export interface IGetActiveReservationsForDeskAction {
    type: typeof GET_ACTIVE_RESERVATIONS_FOR_DESK;
    deskId: string;
  }

export interface IGetActiveReservationsForDeskSuccessAction {
    type: typeof GET_ACTIVE_RESERVATIONS_FOR_DESK_SUCCESS;
    payload: HotDeskReservationInfoDto [];
  }

export interface IGetActiveReservationsForEmployeeAction {
    type: typeof GET_ACTIVE_RESERVATIONS_FOR_EMPLOYEE;
    employeeId: string;
  }

export interface IGetActiveReservationsForEmployeeSuccessAction {
    type: typeof GET_ACTIVE_RESERVATIONS_FOR_EMPLOYEE_SUCCESS;
    payload: HotDeskReservationInfoDto [];
  }

export interface IRemoveHotDeskReservation {
    type: typeof REMOVE_HOTDESK_RESERVATION;
    reservationId: string;
    roomId: string;
  }

export interface IRemoveHotDeskReservationSuccess {
    type: typeof REMOVE_HOTDESK_RESERVATION_SUCCESS;
    reservationId: string;
  }

export type HotDeskDetailsActionTypes = IOpenReservationModalAction
                           | ICloseReservationModalAction
                           | IReserveHotDesk
                           | IReserveHotDeskSuccess
                           | IGetActiveReservationsForDeskAction
                           | IGetActiveReservationsForDeskSuccessAction
                           | IGetActiveReservationsForEmployeeSuccessAction
                           | IRemoveHotDeskReservation
                           | IRemoveHotDeskReservationSuccess;
