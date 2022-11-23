import { Epic, ofType, combineEpics } from 'redux-observable';
import { switchMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { GET_TEAM_LEADERS,
  GET_TEAM_LEADER_PROJECTS_FOR_DROPDOWN } from './types';
import { setLoadingAction } from '../global/actions';
import getTeamLeadersApiService from '../../services/employee/getTeamLeadersApiService';
import { getTeamLeaderProjectsForDropdownSuccessAction, getTeamLeadersSuccessAction } from './actions';
import getTeamLeaderProjectsForDropdownApiService
  from '../../services/employee/getTeamLeaderProjectsForDropdownApiService';

const getTeamLeadersEpic: Epic = (action$) => action$.pipe(
  ofType(GET_TEAM_LEADERS),
  switchMap(() => getTeamLeadersApiService().pipe(
    switchMap((response) => of(getTeamLeadersSuccessAction(response), setLoadingAction(false))),
  )),
);

const getTeamLeaderProjectsForDropdownEpic: Epic = (action$) => action$.pipe(
  ofType(GET_TEAM_LEADER_PROJECTS_FOR_DROPDOWN),
  switchMap(({ teamLeaderId }) => getTeamLeaderProjectsForDropdownApiService(teamLeaderId).pipe(
    switchMap((response) => of(getTeamLeaderProjectsForDropdownSuccessAction(response), setLoadingAction(false))),
  )),
);

const employeesEpics = combineEpics(getTeamLeadersEpic,
  getTeamLeaderProjectsForDropdownEpic);
export default employeesEpics;
