import {
  CLEAR_EQUIPMENT_DETAILS,
  CLOSE_REMOVE_EQUIPMENT_MODAL,
  FETCH_EQUIPMENT_DETAILS,
  FETCH_EQUIPMENT_DETAILS_SUCCESS,
  IEquipmentDetailState,
  OPEN_REMOVE_EQUIPMENT_MODAL,
  REMOVE_EQUIPMENT,
  EquipmentDetailActionTypes,
  OPEN_EDIT_EQUIPMENT_MODAL,
  CLOSE_EDIT_EQUIPMENT_MODAL,
  EDIT_EQUIPMENT_DATA_SUCCESS,
  EDIT_EQUIPMENT_DATA,
  CLOSE_RESERVATION_EQUIPMENT_MODAL,
  OPEN_RESERVATION_EQUIPMENT_MODAL,
  RESERVE_EQUIPMENT_DATA,
  RESERVE_EQUIPMENT_DATA_SUCCESS,
} from './types';

const initialState: IEquipmentDetailState = {
  equipmentDetailsResponse: {
    additionalInfo: '',
    assignedPeopleCount: 0,
    count: 0,
    employees: [],
    id: '',
    name: '',
    reservationsHistory: [],
  },
  isRemoveEquipmentModalOpen: false,
  isEditEquipmentModalOpen: false,
  editRequestBody: {
    equipmentId: '',
    name: '',
    count: 0,
  },
  isReserveEquipmentModalOpen: false,
  reserveRequestBody: {
    equipmentId: '',
    employeeReservations: undefined,
    dateFrom: new Date(),
  },
};

export function equipmentReducer(state = initialState, action: EquipmentDetailActionTypes): IEquipmentDetailState {
  switch (action.type) {
    case FETCH_EQUIPMENT_DETAILS:
      return { ...state };
    case FETCH_EQUIPMENT_DETAILS_SUCCESS:
      return { ...state, equipmentDetailsResponse: action.payload };
    case CLEAR_EQUIPMENT_DETAILS:
      return initialState;
    case REMOVE_EQUIPMENT:
      return { ...state };
    case OPEN_REMOVE_EQUIPMENT_MODAL:
      return { ...state, isRemoveEquipmentModalOpen: true };
    case CLOSE_REMOVE_EQUIPMENT_MODAL:
      return { ...state, isRemoveEquipmentModalOpen: false };
    case OPEN_EDIT_EQUIPMENT_MODAL:
      return { ...state, isEditEquipmentModalOpen: true };
    case CLOSE_EDIT_EQUIPMENT_MODAL:
      return { ...state, isEditEquipmentModalOpen: false };
    case EDIT_EQUIPMENT_DATA:
      return { ...state, editRequestBody: { ...action.equipment } };
    case EDIT_EQUIPMENT_DATA_SUCCESS:
      return { ...state, isEditEquipmentModalOpen: false };
    case OPEN_RESERVATION_EQUIPMENT_MODAL:
      return { ...state, isReserveEquipmentModalOpen: true };
    case CLOSE_RESERVATION_EQUIPMENT_MODAL:
      return { ...state, isReserveEquipmentModalOpen: false };
    case RESERVE_EQUIPMENT_DATA:
      return { ...state, reserveRequestBody: { ...action.reservation } };
    case RESERVE_EQUIPMENT_DATA_SUCCESS:
      return { ...state, isReserveEquipmentModalOpen: false };
    default:
      return state;
  }
}
