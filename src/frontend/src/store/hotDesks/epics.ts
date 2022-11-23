import { Epic } from 'redux-observable';
import { filter, switchMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { FETCH_HOT_DESKS } from './types';
import { fetchHotDesksSuccessAction } from './actions';
import { setLoadingAction } from '../global/actions';
import getAllHotDesksApiService from '../../services/hotDesks/getAllHotDesksApiService';

const getHotDesksEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === FETCH_HOT_DESKS),
  switchMap(({ query }) => getAllHotDesksApiService(query).pipe(
    switchMap((returnData) =>
      of(fetchHotDesksSuccessAction(returnData), setLoadingAction(false))),
  )),
);

export default getHotDesksEpic;
