import { createStore, combineReducers, applyMiddleware, Store, AnyAction } from 'redux';
import { createEpicMiddleware, combineEpics } from 'redux-observable';
import { catchError } from 'rxjs/operators';
import { roomsReducer } from './rooms/reducers';
import roomsEpics from './rooms/epics';
import { roomReducer } from './roomDetails/reducers';
import roomEpics from './roomDetails/epics';
import { projectsReducer } from './projects/reducers';
import projectsEpics from './projects/epics';
import projectDetailsEpics from './projectDetails/epics';
import { projectDetailsReducer } from './projectDetails/reducers';
import { floorsReducer } from './floors/reducers';
import floorsEpic from './floors/epics';
import { globalReducer } from './global/reducers';
import { showErrorCodeInNotifyAction } from './global/actions';
import globalEpics from './global/epics';
import employeesEpics from './employee/epics';
import { employeesReducer } from './employee/reducers';
import { userDetailsReducer } from './userDetails/reducers';
import userDetailsEpics from './userDetails/epics';
import { ErrorCodes } from '../services/common/models';
import { usersReducer } from './users/reducers';
import usersEpics from './users/epics';
import { adminReducer } from './admin/reducers';
import adminEpics from './admin/epics';
import summaryEpic from './summary/epics';
import { summaryReducer } from './summary/reducers';
import { hotDesksReducer } from './hotDesks/reducers';
import hotDesksEpic from './hotDesks/epics';
import { hotDeskDetailsReducer } from './hotDeskDetails/reducers';
import hotDeskDetailsEpic from './hotDeskDetails/epics';
import { darkModeReducer } from './darkMode/reducers';
import { equipmentsReducer } from './equipments/reducers';
import equipmentsEpics from './equipments/epics';
import equipmentEpic from './equipmentDetails/epics';
import { equipmentReducer } from './equipmentDetails/reducers';

export const rootReducer = combineReducers({
  global: globalReducer,
  rooms: roomsReducer,
  room: roomReducer,
  projects: projectsReducer,
  projectDetails: projectDetailsReducer,
  floors: floorsReducer,
  employees: employeesReducer,
  userDetails: userDetailsReducer,
  users: usersReducer,
  summary: summaryReducer,
  darkMode: darkModeReducer,
  admin: adminReducer,
  hotDesks: hotDesksReducer,
  hotDeskDetails: hotDeskDetailsReducer,
  equipments: equipmentsReducer,
  equipmentDetails: equipmentReducer,
});

export type AppState = ReturnType<typeof rootReducer>;

let store: Store<any, AnyAction>;

const greatEpic = (action$: any, store$: any, dependencies: any) => combineEpics(
  roomsEpics, roomEpics,
  projectsEpics, projectDetailsEpics,
  floorsEpic, globalEpics, employeesEpics,
  userDetailsEpics, usersEpics,
  summaryEpic, adminEpics, hotDesksEpic, hotDeskDetailsEpic,
  equipmentEpic, equipmentsEpics,
)(action$, store$, dependencies)
  .pipe(
    catchError((error: any, source: any) => {
      if (!error.status && (error.name || error.name === 'AjaxError')) {
        store.dispatch(showErrorCodeInNotifyAction(ErrorCodes.ServerIsNotAccessible));
      } else {
        const code = error.response?.code || 0;
        const status = error.response?.status;

        store.dispatch(showErrorCodeInNotifyAction(code, status));
      }
      return source;
    }),
  );

const epicMiddleware = createEpicMiddleware();

export default function configureStore() {
  store = createStore(rootReducer, applyMiddleware(epicMiddleware));

  epicMiddleware.run(greatEpic);

  return store;
}
