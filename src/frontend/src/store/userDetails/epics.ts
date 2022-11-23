import { Epic, combineEpics, ofType } from 'redux-observable';
import { filter, switchMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { setLoadingAction } from '../global/actions';
import { FETCH_USER_DETAILS, UPDATE_WORKSPACE_TYPE } from './types';
import { fetchUserDetailsSuccess } from './actions';
import getUserDetailsApiService from '../../services/user/getUserDetailsApiService';
import updateEmployeesWorkspaceTypesApiService from '../../services/employee/updateEmployeeWorkspaceTypesApiService';

const getUserDetailsEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === FETCH_USER_DETAILS),
  switchMap(({ userId }) =>
    getUserDetailsApiService(userId)
      .pipe(
        switchMap((userDetailsDto) =>
          of(
            fetchUserDetailsSuccess(userDetailsDto),
            setLoadingAction(false),
          )),
      )),
);

const updateWorkspaceTypesEpic: Epic = (action$) => action$.pipe(
  ofType(UPDATE_WORKSPACE_TYPE),
  switchMap(({ updateEmployeeWorkspaceTypeDtos }) =>
    updateEmployeesWorkspaceTypesApiService(updateEmployeeWorkspaceTypeDtos)
      .pipe(
        switchMap(() => of(setLoadingAction(false))),
      )),
);

const userDetailsEpics = combineEpics(getUserDetailsEpic, updateWorkspaceTypesEpic);

export default userDetailsEpics;
