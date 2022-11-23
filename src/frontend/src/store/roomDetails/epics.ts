import { Action } from 'redux';
import { Epic, combineEpics, ofType } from 'redux-observable';
import { catchError, filter, map, switchMap } from 'rxjs/operators';
import { iif, of } from 'rxjs';

import {
  FETCH_ROOM,
  REMOVE_TEAM_FROM_ROOM,
  ALLOCATE_DESKS,
  DELETE_DESKS,
  ADD_DESKS,
  RESERVE_DESK,
  UPDATE_RESERVATION,
  GET_EMPLOYEES_FOR_ROOM_DETAILS,
  SET_HOT_DESK,
  SET_ROOM_HOT_DESK,
  TOGGLE_DESK_IS_ENABLED,
  RELEASE_DESK_EMPLOYEE,
} from './types';

import { fetchRoomSuccess, fetchRoom, getEmployeesForRoomDetailsSuccess, getEmployeesForRoomDetails, getDeskForRoomDetailsSuccess,
} from './actions';
import getRoomApiService from '../../services/room/getRoomApiService';
import removeTeamFromRoomApiService from '../../services/room/removeTeamFromRoomApiService';
import deleteDesksApiService from '../../services/room/deleteDesksApiService';
import allocateDesksApiService from '../../services/room/allocateDesksApiService';
import addDesksApiService from '../../services/room/addDesksApiService';
import { showErrorCodeInNotifyAction, setLoadingAction, showNotifyAction } from '../global/actions';
import reserveDeskApiService from '../../services/room/reserveDeskApiService';
import updateReservationApiService from '../../services/room/updateReservationApiService';
import getEmployeesForRoomDetailsApiService from '../../services/room/getProjectEmployeeForDeskCardService';
import setHotDeskApiService from '../../services/room/setHotDeskApiService';
import toggleDeskIsEnabledApiService from '../../services/room/toggleDeskIsEnabledApiService';
import setRoomHotDeskApiService from '../../services/room/setRoomHotDeskApiService';
import { strIsNullOrEmpty } from '../../helpers/helpers';
import releaseDeskEmployeeApiService from '../../services/room/releaseDeskEmployeeApiService';
import { ErrorCodes } from '../../services/common/models';

const handleApiError = (followUpActions: Action<any>[] | null = null) =>
  catchError((error: any) => {
    let notifyAction;

    if (error.name || error.name === 'AjaxError') {
      notifyAction = showNotifyAction(error.response.message, true, ' ');
    } else {
      notifyAction = showErrorCodeInNotifyAction(ErrorCodes.UnknownError);
    }

    if (followUpActions && !!followUpActions.length) {
      return of(notifyAction, ...followUpActions);
    }

    return of(notifyAction);
  });

const getRoomEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === FETCH_ROOM),
  switchMap(({ roomId }) => getRoomApiService(roomId)
    .pipe(
      switchMap((returnData) => of(fetchRoomSuccess(returnData), setLoadingAction(false))),
      catchError((error: any) => {
        if (error.name || error.name === 'AjaxError') {
          return of(showErrorCodeInNotifyAction(error.response.code, error.response.status), setLoadingAction(false));
        }
        return of(showErrorCodeInNotifyAction(ErrorCodes.UnknownError), setLoadingAction(false));
      }),
    )),
);

const removeTeamFromRoomEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === REMOVE_TEAM_FROM_ROOM),
  switchMap(({ roomId, projectId }) => removeTeamFromRoomApiService(roomId, projectId)
    .pipe(
      switchMap(() => iif(() => !strIsNullOrEmpty(projectId),
        of(fetchRoom(roomId), getEmployeesForRoomDetails(projectId, roomId)),
        of(fetchRoom(roomId)))),
    )),
);

const deleteDesksInRoomEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === DELETE_DESKS),
  switchMap(({ roomId, deskIds, projectId, fetchRoomAction }) => deleteDesksApiService(roomId, deskIds)
    .pipe(
      switchMap((returnData) => iif(() => !strIsNullOrEmpty(projectId),
        of(fetchRoomAction, getEmployeesForRoomDetails(projectId, returnData.roomId)),
        of(fetchRoomAction))),
    )),
);

const allocateDesksInRoomEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === ALLOCATE_DESKS),
  switchMap(({ allocateDesksDto }) => allocateDesksApiService(allocateDesksDto)
    .pipe(
      map((returnData) => fetchRoom(returnData.roomId)),
    )),
);

const reserveDeskInRoomEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === RESERVE_DESK),
  switchMap(({ reserveDeskDto, fetchRoomAction }) => reserveDeskApiService(reserveDeskDto)
    .pipe(
      switchMap(() => iif(() => fetchRoomAction,
        of(fetchRoomAction))),
    )),
);

const updateReservationEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === UPDATE_RESERVATION),
  switchMap(({ updateReservationDto, roomId, projectId, fetchRoomAction }) => updateReservationApiService(updateReservationDto)
    .pipe(
      switchMap(() => iif(() => !strIsNullOrEmpty(projectId) && fetchRoomAction,
        of(fetchRoomAction, getEmployeesForRoomDetails(projectId, roomId)),
        of(fetchRoomAction))),
      handleApiError([fetchRoomAction]),
    )),
);

const releaseDeskEmployeeEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === RELEASE_DESK_EMPLOYEE),
  switchMap(({ releaseDeskEmployeeDto, fetchRoomAction }) => releaseDeskEmployeeApiService(releaseDeskEmployeeDto)
    .pipe(
      switchMap(() => iif(() => fetchRoomAction,
        of(fetchRoomAction))),
      handleApiError([fetchRoomAction]),
    )),
);

const addDesksInRoomEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === ADD_DESKS),
  switchMap(({ addDesksDto }) => addDesksApiService(addDesksDto)
    .pipe(
      map((returnData) => fetchRoom(returnData.roomId)),
    )),
);

const getEmployeesForRoomDetailsEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === GET_EMPLOYEES_FOR_ROOM_DETAILS),
  switchMap(({ projectId, roomId }) => getEmployeesForRoomDetailsApiService(projectId, roomId)
    .pipe(
      map((returnData) => getEmployeesForRoomDetailsSuccess(returnData)),
    )),
);

const setHotDeskEpic: Epic = (action$) => action$.pipe(
  ofType(SET_HOT_DESK),
  switchMap(({ dto, projectId }) => setHotDeskApiService(dto)
    .pipe(
      switchMap((returnData) => of(getDeskForRoomDetailsSuccess(returnData), setLoadingAction(projectId === null))),
      catchError((error: any) => {
        if (error.name || error.name === 'AjaxError') {
          return of(showErrorCodeInNotifyAction(error.response.code, error.response.status), setLoadingAction(false));
        }
        return of(showErrorCodeInNotifyAction(ErrorCodes.UnknownError), setLoadingAction(false));
      }),
    )),
);

const toggleDeskIsEnabledEpic: Epic = (action$) => action$.pipe(
  ofType(TOGGLE_DESK_IS_ENABLED),
  switchMap(({ dto, projectId, roomId, fetchRoomAction }) => toggleDeskIsEnabledApiService(dto)
    .pipe(
      switchMap(() => iif(() => !strIsNullOrEmpty(projectId),
        of(getEmployeesForRoomDetails(projectId, roomId), fetchRoomAction),
        of(fetchRoomAction))),
      handleApiError(!strIsNullOrEmpty(projectId) ? [getEmployeesForRoomDetails(projectId, roomId), fetchRoomAction] : [fetchRoomAction]),
    )),
);

const setRoomHotDeskEpic: Epic = (action$) => action$.pipe(
  ofType(SET_ROOM_HOT_DESK),
  switchMap(({ dto, projectId }) => setRoomHotDeskApiService(dto)
    .pipe(
      switchMap(() => iif(() => !strIsNullOrEmpty(projectId),
        of(fetchRoom(dto.roomId), getEmployeesForRoomDetails(projectId, dto.roomId)),
        of(fetchRoom(dto.roomId)))),
      catchError((error: any) => {
        if (error.name || error.name === 'AjaxError') {
          return of(showErrorCodeInNotifyAction(error.response.code, error.response.status), setLoadingAction(false));
        }
        return of(showErrorCodeInNotifyAction(ErrorCodes.UnknownError), setLoadingAction(false));
      }),
    )),

);

const roomEpic = combineEpics(
  getRoomEpic,
  removeTeamFromRoomEpic,
  deleteDesksInRoomEpic,
  allocateDesksInRoomEpic,
  addDesksInRoomEpic,
  reserveDeskInRoomEpic,
  updateReservationEpic,
  releaseDeskEmployeeEpic,
  getEmployeesForRoomDetailsEpic,
  setHotDeskEpic,
  toggleDeskIsEnabledEpic,
  setRoomHotDeskEpic,
);

export default roomEpic;
