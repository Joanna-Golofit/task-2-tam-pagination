import { IImportBubblesAction, IImportBubblesSuccessAction, IMPORT_BUBBLES, IMPORT_BUBBLES_SUCCESS }
  from './types';

export function importBubblesAction(): IImportBubblesAction {
  return {
    type: IMPORT_BUBBLES,
  };
}

export function importBubblesSuccessAction(): IImportBubblesSuccessAction {
  return {
    type: IMPORT_BUBBLES_SUCCESS,
  };
}
