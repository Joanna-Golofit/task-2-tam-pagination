import { DarkModeActionTypes, IDarkModeState, DARK_MODE } from './types';

const dark = sessionStorage.getItem('darkmode');

const initialState: IDarkModeState = {
  darkMode: dark ? JSON.parse(dark) : false,
};

export function darkModeReducer(state = initialState, action: DarkModeActionTypes): IDarkModeState {
  switch (action.type) {
    case DARK_MODE:
      return { ...state, darkMode: !state.darkMode };
    default:
      return state;
  }
}
