import { AddEquipmentDto, Equipment, EquipmentsFilterOptions } from '../../services/equipments/models';

export const FETCH_EQUIPMENTS = 'FETCH_EQUIPMENTS';
export const FETCH_EQUIPMENTS_SUCCESS = 'FETCH_EQUIPMENTS_SUCCESS';

export const ADD_EQUIPMENT = 'ADD_EQUIPMENT';

export const OPEN_ADD_EQUIPMENT_MODAL = 'OPEN_ADD_EQUIPMENT_MODAL';
export const CLOSE_ADD_EQUIPMENT_MODAL = 'CLOSE_ADD_EQUIPMENT_MODAL';

export const ADD_EQUIPMENT_SUCCESS = 'ADD_EQUIPMENT_SUCCESS';

export const OPEN_EDIT_EQUIPMENT_MODAL = 'OPEN_EDIT_EQUIPMENT_MODAL';
export const CLOSE_EDIT_EQUIPMENT_MODAL = 'CLOSE_EDIT_EQUIPMENT_MODAL';

export type Equipments = {
  equipments: Equipment[];
}
export interface IEquipmentsState {
  equipments: Equipment[];
  equipmentsCount: number;
  filters: EquipmentsFilterOptions;
  isAddEquipmentModalOpen: boolean;
  isEditEquipmentModalOpen: boolean;
  editEquipmentId: string;
  isRemoveEquipmentModalOpen: boolean;
}

export interface IFetchEquipmentsAction {
  type: typeof FETCH_EQUIPMENTS;
}

export interface IFetchEquipmentsSuccessAction {
  type: typeof FETCH_EQUIPMENTS_SUCCESS;
  response: Equipment[];
}

export interface IAddEquipmentAction {
  type: typeof ADD_EQUIPMENT;
  addEquipmentDto: AddEquipmentDto;
}

export interface IOpenAddEquipmentModalAction {
  type: typeof OPEN_ADD_EQUIPMENT_MODAL;
}

export interface ICloseAddEquipmentModalAction {
  type: typeof CLOSE_ADD_EQUIPMENT_MODAL;
}

export interface IAddEquipmentSuccessAction {
  type: typeof ADD_EQUIPMENT_SUCCESS;
  equipment: Equipment;
  equipmentId: string;
}

export interface IOpenEquipmentEditModalAction {
  type: typeof OPEN_EDIT_EQUIPMENT_MODAL;
  equipmentId: string;
}

export interface ICloseEquipmentEditModalAction {
  type: typeof CLOSE_EDIT_EQUIPMENT_MODAL;
}

export type EquipmentsActionTypes = IFetchEquipmentsAction
                                   | IFetchEquipmentsSuccessAction
                                   | IAddEquipmentAction
                                   | IOpenAddEquipmentModalAction
                                   | ICloseAddEquipmentModalAction
                                   | IAddEquipmentSuccessAction
                                   | ICloseEquipmentEditModalAction
                                   | IOpenEquipmentEditModalAction;
