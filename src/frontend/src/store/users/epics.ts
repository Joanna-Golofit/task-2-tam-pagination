import { Epic, combineEpics } from 'redux-observable';
import { filter, switchMap } from 'rxjs/operators';
import { of } from 'rxjs';

import getAllUsersForSearcherApiService from '../../services/user/getAllUsersForSearcherApiService';
import { GET_ALL_USERS_FOR_SEARCHER } from './types';
import { getAllUsersForSearcherSuccess, setLoadingUsersSearcher } from './actions';

const getAllUsersForSearcherEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === GET_ALL_USERS_FOR_SEARCHER),
  switchMap(() => getAllUsersForSearcherApiService()
    .pipe(
      switchMap((userDetailsDto) => of(getAllUsersForSearcherSuccess(userDetailsDto), setLoadingUsersSearcher(false))),
    )),
);

const usersEpics = combineEpics(getAllUsersForSearcherEpic);

export default usersEpics;
