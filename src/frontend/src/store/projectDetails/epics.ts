import { Epic, ofType, combineEpics } from 'redux-observable';
import { filter, map, switchMap, catchError } from 'rxjs/operators';
import { iif, of } from 'rxjs';
import {
  ADD_EMPLOYEE_TO_PROJECT, ASSIGN_EMPLOYEES_TO_DESKS, FETCH_PROJECT_DETAILS,
  GET_ROOM_DETAILS_FOR_PROJECT,
  REMOVE_EMPLOYEES_FROM_ROOM,
  REMOVE_EMPLOYEE_FROM_PROJECT,
  REMOVE_PROJECT,
  UPDATE_EMPLOYEES_WORKSPACE_TYPES,
} from './types';
import {
  fetchProjectDetails, fetchProjectsSuccess, getRoomDetailsForProjectSuccess,
} from './actions';
import { setLoadingAction, showErrorCodeInNotifyAction } from '../global/actions';
import getProjectApiService from '../../services/project/getProjectApiService';
import updateEmployeesWorkspaceTypesApiService from '../../services/employee/updateEmployeeWorkspaceTypesApiService';
import getRoomDetailsForProjectApiService from '../../services/room/getRoomDetailsForProjectApiService';
import { updateUserWorkmode } from '../users/actions';
import addEmployeeToProjectApiService from '../../services/project/addEmployeeToProjectApiService';
import removeProjectApiService from '../../services/project/removeProjectApiService';
import removeEmployeeFromProjectApiService from '../../services/project/removeEmployeeFromProjectApiService';
import reserveDeskApiService from '../../services/room/reserveDeskApiService';
import { strIsNullOrEmpty } from '../../helpers/helpers';
import removeTeamFromRoomApiService from '../../services/room/removeTeamFromRoomApiService';
import { ErrorCodes } from '../../services/common/models';

const getProjectDetailsEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === FETCH_PROJECT_DETAILS),
  switchMap((action) => getProjectApiService(action.id).pipe(
    switchMap((project) => of(fetchProjectsSuccess(project), setLoadingAction(false))),
  )),
);

const updateEmployeesWorkspaceTypesEpic: Epic = (action$) => action$.pipe(
  ofType(UPDATE_EMPLOYEES_WORKSPACE_TYPES),
  switchMap(({ updateEmployeeWorkspaceTypeDtos, projectId }) => updateEmployeesWorkspaceTypesApiService(updateEmployeeWorkspaceTypeDtos)
    .pipe(
      switchMap(() => of(updateUserWorkmode(updateEmployeeWorkspaceTypeDtos), fetchProjectDetails(projectId))),
      catchError((error: any) => {
        if (error.name || error.name === 'AjaxError') {
          return of(showErrorCodeInNotifyAction(error.response.code, error.response.status), setLoadingAction(false));
        }
        return of(showErrorCodeInNotifyAction(ErrorCodes.UnknownError), setLoadingAction(false));
      }),
    )),
);

const getRoomDetailsForProjectEpic: Epic = (action$) => action$.pipe(
  ofType(GET_ROOM_DETAILS_FOR_PROJECT),
  switchMap(({ roomId }) => getRoomDetailsForProjectApiService(roomId)
    .pipe(
      switchMap((returnData) => of(getRoomDetailsForProjectSuccess(returnData), setLoadingAction(false))),
    )),
);

const assignEmployeesToDesksEpic: Epic = (action$) => action$.pipe(
  ofType(ASSIGN_EMPLOYEES_TO_DESKS),
  switchMap(({ reserveDeskDto, projectId }) => reserveDeskApiService(reserveDeskDto)
    .pipe(
      switchMap(() => iif(() => !strIsNullOrEmpty(projectId),
        of(fetchProjectDetails(projectId)),
        of(setLoadingAction(false)))),
    )),
);

const removeEmployeesFromRoomEpic: Epic = (action$) => action$.pipe(
  ofType(REMOVE_EMPLOYEES_FROM_ROOM),
  switchMap(({ roomId, projectId }) => removeTeamFromRoomApiService(roomId, projectId).pipe(
    switchMap(() => iif(() => !strIsNullOrEmpty(projectId),
      of(fetchProjectDetails(projectId)),
      of(setLoadingAction(false)))),
  )),
);

const addEmployeeToProjectEpic: Epic = (action$) => action$.pipe(
  ofType(ADD_EMPLOYEE_TO_PROJECT),
  switchMap((action) => addEmployeeToProjectApiService(action.payload)
    .pipe(
      switchMap(() => of(fetchProjectDetails(action.payload.companyId))),
    )),
);

const removeEmployeeFromProjectEpic: Epic = (action$) => action$.pipe(
  ofType(REMOVE_EMPLOYEE_FROM_PROJECT),
  switchMap((action) => removeEmployeeFromProjectApiService(action.payload.employeeId)
    .pipe(
      switchMap(() => of(fetchProjectDetails(action.payload.companyId))),
    )),
);

const removeProjectEpic: Epic = (action$) => action$.pipe(
  ofType(REMOVE_PROJECT),
  switchMap(({ id, navigate }) => removeProjectApiService(id)
    .pipe(
      map(() => {
        navigate();
        return setLoadingAction(false);
      }),
    )),
);

const projectDetailsEpics = combineEpics(
  getProjectDetailsEpic,
  updateEmployeesWorkspaceTypesEpic,
  getRoomDetailsForProjectEpic,
  assignEmployeesToDesksEpic,
  removeEmployeesFromRoomEpic,
  addEmployeeToProjectEpic,
  removeEmployeeFromProjectEpic,
  removeProjectEpic,
);

export default projectDetailsEpics;
