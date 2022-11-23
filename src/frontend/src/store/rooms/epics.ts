import { Epic } from 'redux-observable';
import { filter, switchMap } from 'rxjs/operators';
import { iif, of } from 'rxjs';
import { FETCH_ROOMS } from './types';
import { fetchRoomsSuccess } from './actions';
import getAllRoomsApiService from '../../services/room/getAllRoomsApiService';
import { showErrorCodeInNotifyAction, setLoadingAction } from '../global/actions';

const roomsEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === FETCH_ROOMS),
  switchMap(() => getAllRoomsApiService().pipe(
    switchMap((returnData) => iif(() => returnData.errorCode === 0,
      of(fetchRoomsSuccess(returnData), setLoadingAction(false)),
      of(showErrorCodeInNotifyAction(returnData.errorCode)))),
  )),
);

export default roomsEpic;
