export const DARK_MODE = 'DARK_MODE';

export interface IDarkModeState {
  darkMode: boolean;
}

export interface IToggleDarkModeAction {
  type: typeof DARK_MODE;
  payload: boolean;
}

export type DarkModeActionTypes = IToggleDarkModeAction;
