import {
  ADD_EQUIPMENT,
  ADD_EQUIPMENT_SUCCESS,
  CLOSE_ADD_EQUIPMENT_MODAL,
  CLOSE_EDIT_EQUIPMENT_MODAL,
  EquipmentsActionTypes,
  FETCH_EQUIPMENTS,
  FETCH_EQUIPMENTS_SUCCESS,
  IEquipmentsState,
  OPEN_ADD_EQUIPMENT_MODAL,
  OPEN_EDIT_EQUIPMENT_MODAL,
} from './types';

const initialState: IEquipmentsState = {
  equipments: [],
  equipmentsCount: 0,
  filters: {
    name: '',
  },
  isAddEquipmentModalOpen: false,
  isEditEquipmentModalOpen: false,
  editEquipmentId: '',
  isRemoveEquipmentModalOpen: false,
};

export function equipmentsReducer(state = initialState, action: EquipmentsActionTypes): IEquipmentsState {
  switch (action.type) {
    case FETCH_EQUIPMENTS:
      return { ...state };
    case FETCH_EQUIPMENTS_SUCCESS:
      return {
        ...state,
        equipments: action.response,
      };
    case ADD_EQUIPMENT:
      return { ...state, isAddEquipmentModalOpen: false };
    case OPEN_ADD_EQUIPMENT_MODAL:
      return { ...state, isAddEquipmentModalOpen: true };
    case CLOSE_ADD_EQUIPMENT_MODAL:
      return { ...state, isAddEquipmentModalOpen: false };
    case ADD_EQUIPMENT_SUCCESS: {
      const newEquipment = action.equipment;
      newEquipment.id = action.equipmentId;
      newEquipment.assignedPeopleCount = 0;
      const newCount = state.equipmentsCount + 1;
      return {
        ...state,
        equipments: [...state.equipments, newEquipment],
        isAddEquipmentModalOpen: false,
        equipmentsCount: newCount,
      };
    }
    case OPEN_EDIT_EQUIPMENT_MODAL:
      return { ...state, isEditEquipmentModalOpen: true, editEquipmentId: action.equipmentId };
    case CLOSE_EDIT_EQUIPMENT_MODAL:
      return { ...state, isEditEquipmentModalOpen: false, editEquipmentId: '' };
    default:
      return state;
  }
}
