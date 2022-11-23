import { EquipmentDetails, EditEquipmentDto, ReservationEquipmentDto } from '../../services/equipments/models';

export const FETCH_EQUIPMENT_DETAILS = 'FETCH_EQUIPMENT_DETAILS';
export const FETCH_EQUIPMENT_DETAILS_SUCCESS = 'FETCH_EQUIPMENT_DETAILS_SUCCESS';
export const CLEAR_EQUIPMENT_DETAILS = 'CLEAR_EQUIPMENT_DETAILS';
export const REMOVE_EQUIPMENT = 'REMOVE_EQUIPMENT';
export const REMOVE_EQUIPMENT_SUCCESS = 'REMOVE_EQUIPMENT_SUCCESS';
export const OPEN_REMOVE_EQUIPMENT_MODAL = 'OPEN_MODAL';
export const CLOSE_REMOVE_EQUIPMENT_MODAL = 'CLOSE_MODAL';
export const ADD_EMPLOYEE_TO_EQUIPMENT = 'ADD_EMPLOYEE_TO_EQUIPMENT';
export const OPEN_EDIT_EQUIPMENT_MODAL = 'OPEN_EDIT_EQUIPMENT_MODAL';
export const CLOSE_EDIT_EQUIPMENT_MODAL = 'CLOSE_EDIT_EQUIPMENT_MODAL';
export const EDIT_EQUIPMENT_DATA = 'EDIT_EQUIPMENT_DATA';
export const EDIT_EQUIPMENT_DATA_SUCCESS = 'EDIT_EQUIPMENT_DATA_SUCCESS';
export const OPEN_RESERVATION_EQUIPMENT_MODAL = 'OPEN_RESERVATION_EQUIPMENT_MODAL';
export const CLOSE_RESERVATION_EQUIPMENT_MODAL = 'CLOSE_RESERVATION_EQUIPMENT_MODAL';
export const RESERVE_EQUIPMENT_DATA = 'RESERVE_EQUIPMENT_DATA';
export const RESERVE_EQUIPMENT_DATA_SUCCESS = 'RESERVE_EQUIPMENT_DATA_SUCCESS';

export interface IEquipmentDetailState {
  equipmentDetailsResponse: EquipmentDetails;
  isRemoveEquipmentModalOpen: boolean;
  isEditEquipmentModalOpen: boolean;
  editRequestBody: EditEquipmentDto;
  isReserveEquipmentModalOpen: boolean;
  reserveRequestBody: ReservationEquipmentDto;
}

export interface IFetchEquipmentDetailsAction {
  type: typeof FETCH_EQUIPMENT_DETAILS;
  id: string;
}

export interface IFetchEquipmentDetailsSuccessAction {
  type: typeof FETCH_EQUIPMENT_DETAILS_SUCCESS;
  payload: EquipmentDetails;
}

export interface IOpenEquipmentEditModalAction {
  type: typeof OPEN_EDIT_EQUIPMENT_MODAL;
}

export interface ICloseEquipmentEditModalAction {
  type: typeof CLOSE_EDIT_EQUIPMENT_MODAL;
}

export interface IEditEquipmentDataAction {
  type: typeof EDIT_EQUIPMENT_DATA;
  equipment: EditEquipmentDto;
}

export interface IEditEquipmentDataSuccessAction {
  type: typeof EDIT_EQUIPMENT_DATA_SUCCESS;
}

export interface IOpenRemoveEquipmentModalAction {
  type: typeof OPEN_REMOVE_EQUIPMENT_MODAL;
}

export interface ICloseRemoveEquipmentModalAction {
  type: typeof CLOSE_REMOVE_EQUIPMENT_MODAL;
}

export interface IRemoveEquipmentAction {
  type: typeof REMOVE_EQUIPMENT;
  id: string;
  navigate: () => void;
}

export interface IClearEquipmentDetailsAction {
  type: typeof CLEAR_EQUIPMENT_DETAILS;
}

export interface IOpenReservationEquipmentModalAction {
  type: typeof OPEN_RESERVATION_EQUIPMENT_MODAL;
}

export interface ICloseReservationEquipmentModalAction {
  type: typeof CLOSE_RESERVATION_EQUIPMENT_MODAL;
}

export interface IReserveEquipmentDataAction {
  type: typeof RESERVE_EQUIPMENT_DATA;
  reservation: ReservationEquipmentDto;
}

export interface IReserveEquipmentDataSuccessAction {
  type: typeof RESERVE_EQUIPMENT_DATA_SUCCESS;
}

export type EquipmentDetailActionTypes = ICloseRemoveEquipmentModalAction
  | IOpenRemoveEquipmentModalAction
  | IRemoveEquipmentAction
  | IFetchEquipmentDetailsAction
  | IFetchEquipmentDetailsSuccessAction
  | IEditEquipmentDataAction
  | IEditEquipmentDataSuccessAction
  | IClearEquipmentDetailsAction
  | IOpenEquipmentEditModalAction
  | ICloseEquipmentEditModalAction
  | IOpenReservationEquipmentModalAction
  | ICloseReservationEquipmentModalAction
  | IReserveEquipmentDataAction
  | IReserveEquipmentDataSuccessAction;
