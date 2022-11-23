import { Epic, ofType, combineEpics } from 'redux-observable';
import { switchMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { GET_LOGGED_USER_DATA } from './types';
import { getLoggedUserDataSuccessAction, setLoadingAction } from './actions';
import getLoggedUserDataApiService from '../../services/user/getLoggedUserDataApiService';

const getLoggedUserDataEpic: Epic = (action$) => action$.pipe(
  ofType(GET_LOGGED_USER_DATA),
  switchMap(({ loggedUserEmail }) => getLoggedUserDataApiService(loggedUserEmail)
    .pipe(
      switchMap((retData) => of(
        getLoggedUserDataSuccessAction(retData),
        setLoadingAction(false),
      )),
    )),
);
const globalEpics = combineEpics(getLoggedUserDataEpic);

export default globalEpics;
