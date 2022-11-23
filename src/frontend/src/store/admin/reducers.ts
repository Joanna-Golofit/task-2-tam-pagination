import { AdminActionTypes, IAdminState, IMPORT_BUBBLES, IMPORT_BUBBLES_SUCCESS } from './types';

const initialState: IAdminState = {
  isImportPending: false,
};

export function adminReducer(state = initialState, action: AdminActionTypes): IAdminState {
  switch (action.type) {
    case IMPORT_BUBBLES:
      return { ...state, isImportPending: true };
    case IMPORT_BUBBLES_SUCCESS:
      return { ...state, isImportPending: false };
    default:
      return state;
  }
}
