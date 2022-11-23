import { combineEpics, Epic } from 'redux-observable';
import { of } from 'rxjs';
import { filter, switchMap, catchError } from 'rxjs/operators';
import i18n from 'i18next';
import createNewReservationApiService from '../../services/hotDesks/createNewReservationApiService';
import getActiveReservationsForDeskApiService from '../../services/hotDesks/getActiveReservationsForDeskApiService';
import getActiveReservationsForEmployeeApiService from '../../services/hotDesks/getActiveReservationsForEmployeeApiService';
import removeHotDeskApiService from '../../services/hotDesks/removeHotDeskApiService';
import { setLoadingAction, showErrorCodeInNotifyAction, showNotifyAction } from '../global/actions';
import { fetchRoom } from '../roomDetails/actions';
import {
  reserveHotdeskSuccess,
  getActiveReservationsForDeskSuccessAction,
  getActiveReservationsForEmployeeSuccessAction,
  removeHotDeskReservationSuccessAction,
} from './actions';
import {
  RESERVE_HOTDESK,
  GET_ACTIVE_RESERVATIONS_FOR_DESK,
  GET_ACTIVE_RESERVATIONS_FOR_EMPLOYEE,
  REMOVE_HOTDESK_RESERVATION,
} from './types';
import { ErrorCodes } from '../../services/common/models';
import { removeHotDeskReservationSuccessUserAction } from '../userDetails/actions';

const handleApiErrorAndStopLoader = () =>
  catchError((error: any) => {
    if (error.name || error.name === 'AjaxError') {
      const errorMessage = error.response.translationKey ? i18n.t(error.response.translationKey) : error.response.message;
      return of(showNotifyAction(errorMessage, true, ' '), setLoadingAction(false));
    }
    return of(showErrorCodeInNotifyAction(ErrorCodes.UnknownError), setLoadingAction(false));
  });

const reserveHotDeskEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === RESERVE_HOTDESK),
  switchMap(({ payload, roomId }) => createNewReservationApiService(payload)
    .pipe(
      switchMap((id) => of(
        fetchRoom(roomId),
        reserveHotdeskSuccess(id),
        showNotifyAction(`${i18n.t('hotDesks.reservationSuccessNotification')}`),
      )),
      handleApiErrorAndStopLoader(),
    )),
);

const getActiveReservationsForDeskEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === GET_ACTIVE_RESERVATIONS_FOR_DESK),
  switchMap(({ deskId }) => getActiveReservationsForDeskApiService(deskId)
    .pipe(
      switchMap((response) => of(
        getActiveReservationsForDeskSuccessAction(response),
        setLoadingAction(false),
      )),
      handleApiErrorAndStopLoader(),
    )),
);

const getActiveReservationsForEmployeeEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === GET_ACTIVE_RESERVATIONS_FOR_EMPLOYEE),
  switchMap(({ employeeId }) => getActiveReservationsForEmployeeApiService(employeeId)
    .pipe(
      switchMap((response) => of(
        getActiveReservationsForEmployeeSuccessAction(response),
        setLoadingAction(false),
      )),
      handleApiErrorAndStopLoader(),
    )),
);

const removeHotDeskReservationEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === REMOVE_HOTDESK_RESERVATION),
  switchMap(({ reservationId, roomId }) => removeHotDeskApiService(reservationId)
    .pipe(
      switchMap(() => of(
        fetchRoom(roomId),
        removeHotDeskReservationSuccessAction(reservationId),
        removeHotDeskReservationSuccessUserAction(reservationId),
      )),
      handleApiErrorAndStopLoader(),
    )),
);

const hotDeskDetailsEpic = combineEpics(
  reserveHotDeskEpic,
  getActiveReservationsForDeskEpic,
  getActiveReservationsForEmployeeEpic,
  removeHotDeskReservationEpic,
);

export default hotDeskDetailsEpic;
