import { Epic } from 'redux-observable';
import { filter, switchMap, catchError } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import { of } from 'rxjs';
import apiService from '../../services/apiService';
import { FETCH_FLOORS, Floors } from './types';
import { fetchFloorsSuccess } from './actions';
import { showErrorCodeInNotifyAction, setLoadingAction } from '../global/actions';
import { ErrorCodes } from '../../services/common/models';

const floorsEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === FETCH_FLOORS),
  switchMap(() => apiService('Floor', 'GET').pipe(
    switchMap((data: AjaxResponse) => {
      const floors = data.response as Floors;
      return of(fetchFloorsSuccess(floors), setLoadingAction(false));
    }),
    catchError((error: any) => {
      if (error.name || error.name === 'AjaxError') {
        return of(showErrorCodeInNotifyAction(ErrorCodes.ServerIsNotAccessible));
      }
      return of(showErrorCodeInNotifyAction(0));
    }),
  )),
);

export default floorsEpic;
