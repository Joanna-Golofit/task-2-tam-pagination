export const IMPORT_BUBBLES = 'IMPORT_BUBBLES';
export const IMPORT_BUBBLES_SUCCESS = 'IMPORT_BUBBLES_SUCCESS';

export interface IAdminState {
    isImportPending: boolean;
  }

export interface IImportBubblesAction {
    type: typeof IMPORT_BUBBLES;
  }

export interface IImportBubblesSuccessAction {
    type: typeof IMPORT_BUBBLES_SUCCESS;
  }

export type AdminActionTypes = IImportBubblesAction
                            | IImportBubblesSuccessAction;
