import { Epic } from 'redux-observable';
import { filter, switchMap } from 'rxjs/operators';
import { iif, of } from 'rxjs';
import { GET_SUMMARY } from './types';
import { getSummarySuccess } from './actions';
import { showErrorCodeInNotifyAction, setLoadingAction } from '../global/actions';
import getSummaryApiService from '../../services/summary/getSummaryApiService';
import { setRoomDetailsViewAction } from '../roomDetails/actions';

const summaryEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === GET_SUMMARY),
  switchMap(() => getSummaryApiService().pipe(
    switchMap((returnData) =>
      iif(() => returnData.errorCode === 0,
        of(getSummarySuccess(returnData), setLoadingAction(false)),
        of(showErrorCodeInNotifyAction(returnData.errorCode), setRoomDetailsViewAction(true)))),
  )),
);

export default summaryEpic;
