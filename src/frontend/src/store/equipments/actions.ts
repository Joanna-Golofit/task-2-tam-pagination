import { AddEquipmentDto, Equipment } from '../../services/equipments/models';
import {
  ADD_EQUIPMENT,
  ADD_EQUIPMENT_SUCCESS,
  CLOSE_ADD_EQUIPMENT_MODAL,
  CLOSE_EDIT_EQUIPMENT_MODAL,
  FETCH_EQUIPMENTS,
  FETCH_EQUIPMENTS_SUCCESS,
  IAddEquipmentAction,
  IAddEquipmentSuccessAction,
  ICloseAddEquipmentModalAction,
  ICloseEquipmentEditModalAction,
  IFetchEquipmentsAction,
  IFetchEquipmentsSuccessAction,
  IOpenAddEquipmentModalAction,
  IOpenEquipmentEditModalAction,
  OPEN_ADD_EQUIPMENT_MODAL,
  OPEN_EDIT_EQUIPMENT_MODAL,
} from './types';

export function fetchEquipments(): IFetchEquipmentsAction {
  return {
    type: FETCH_EQUIPMENTS,
  };
}

export function fetchEquipmentsSuccess(response: Equipment[]): IFetchEquipmentsSuccessAction {
  return {
    type: FETCH_EQUIPMENTS_SUCCESS,
    response,
  };
}
export function addEquipmentAction(addEquipmentDto: AddEquipmentDto): IAddEquipmentAction {
  return {
    type: ADD_EQUIPMENT,
    addEquipmentDto,
  };
}

export function openAddEquipmentModalAction(): IOpenAddEquipmentModalAction {
  return {
    type: OPEN_ADD_EQUIPMENT_MODAL,
  };
}

export function closeAddEquipmentModalAction(): ICloseAddEquipmentModalAction {
  return {
    type: CLOSE_ADD_EQUIPMENT_MODAL,
  };
}

export function addEquipmentSuccess(equipment: Equipment, equipmentId: string): IAddEquipmentSuccessAction {
  return {
    type: ADD_EQUIPMENT_SUCCESS,
    equipment,
    equipmentId,
  };
}

export function openEditEquipmentModalAction(equipmentId: string): IOpenEquipmentEditModalAction {
  return {
    type: OPEN_EDIT_EQUIPMENT_MODAL,
    equipmentId,
  };
}

export function closeReservationModalAction(): ICloseEquipmentEditModalAction {
  return {
    type: CLOSE_EDIT_EQUIPMENT_MODAL,
  };
}
