import { IToggleDarkModeAction, DARK_MODE }
  from './types';

export function toggleDarkMode(e: boolean): IToggleDarkModeAction {
  sessionStorage.setItem('darkmode', JSON.stringify(e));
  return {
    type: DARK_MODE,
    payload: e,
  };
}
