import { EditEquipmentDto, EquipmentDetails, ReservationEquipmentDto } from '../../services/equipments/models';
import {
  CLOSE_RESERVATION_EQUIPMENT_MODAL,
  OPEN_RESERVATION_EQUIPMENT_MODAL,
  IEditEquipmentDataSuccessAction,
  EDIT_EQUIPMENT_DATA_SUCCESS,
  CLEAR_EQUIPMENT_DETAILS,
  FETCH_EQUIPMENT_DETAILS_SUCCESS,
  FETCH_EQUIPMENT_DETAILS,
  IFetchEquipmentDetailsAction,
  IFetchEquipmentDetailsSuccessAction,
  IClearEquipmentDetailsAction,
  REMOVE_EQUIPMENT,
  IRemoveEquipmentAction,
  CLOSE_REMOVE_EQUIPMENT_MODAL,
  ICloseRemoveEquipmentModalAction,
  OPEN_REMOVE_EQUIPMENT_MODAL,
  IOpenRemoveEquipmentModalAction,
  IOpenEquipmentEditModalAction,
  OPEN_EDIT_EQUIPMENT_MODAL,
  ICloseEquipmentEditModalAction,
  CLOSE_EDIT_EQUIPMENT_MODAL,
  IReserveEquipmentDataAction,
  RESERVE_EQUIPMENT_DATA,
  EDIT_EQUIPMENT_DATA,
  IOpenReservationEquipmentModalAction,
  ICloseReservationEquipmentModalAction,
  IEditEquipmentDataAction,
  RESERVE_EQUIPMENT_DATA_SUCCESS,
  IReserveEquipmentDataSuccessAction,
} from './types';

export function fetchEquipmentDetailsAction(id: string): IFetchEquipmentDetailsAction {
  return {
    type: FETCH_EQUIPMENT_DETAILS,
    id,
  };
}

export function fetchEquipmentDetailsSuccessAction(equipmentDetails: EquipmentDetails): IFetchEquipmentDetailsSuccessAction {
  return {
    type: FETCH_EQUIPMENT_DETAILS_SUCCESS,
    payload: equipmentDetails,
  };
}

export function clearEquipmentDetailsAction(): IClearEquipmentDetailsAction {
  return {
    type: CLEAR_EQUIPMENT_DETAILS,
  };
}

export function openEditEquipmentModalAction(): IOpenEquipmentEditModalAction {
  return {
    type: OPEN_EDIT_EQUIPMENT_MODAL,
  };
}

export function closeEditEquipmentModalAction(): ICloseEquipmentEditModalAction {
  return {
    type: CLOSE_EDIT_EQUIPMENT_MODAL,
  };
}

export function editEquipmentAction(equipment: EditEquipmentDto): IEditEquipmentDataAction {
  return {
    type: EDIT_EQUIPMENT_DATA,
    equipment,
  };
}

export function editEquipmentSuccessAction(): IEditEquipmentDataSuccessAction {
  return {
    type: EDIT_EQUIPMENT_DATA_SUCCESS,
  };
}

export function openReservationEquipmentModalAction(): IOpenReservationEquipmentModalAction {
  return {
    type: OPEN_RESERVATION_EQUIPMENT_MODAL,
  };
}

export function closeReservationEquipmentModalAction(): ICloseReservationEquipmentModalAction {
  return {
    type: CLOSE_RESERVATION_EQUIPMENT_MODAL,
  };
}

export function reserveEquipmentDataAction(reservationEquipmentDto: ReservationEquipmentDto): IReserveEquipmentDataAction {
  return {
    type: RESERVE_EQUIPMENT_DATA,
    reservation: reservationEquipmentDto,
  };
}

export function reserveEquipmentDataSuccessAction(): IReserveEquipmentDataSuccessAction {
  return {
    type: RESERVE_EQUIPMENT_DATA_SUCCESS,
  };
}

export function openRemoveEquipmentModalAction(): IOpenRemoveEquipmentModalAction {
  return {
    type: OPEN_REMOVE_EQUIPMENT_MODAL,
  };
}

export function closeRemoveEquipmentModalAction(): ICloseRemoveEquipmentModalAction {
  return {
    type: CLOSE_REMOVE_EQUIPMENT_MODAL,
  };
}

export function removeEquipmentAction(id: string, navigate: () => void): IRemoveEquipmentAction {
  return {
    type: REMOVE_EQUIPMENT,
    id,
    navigate,
  };
}
