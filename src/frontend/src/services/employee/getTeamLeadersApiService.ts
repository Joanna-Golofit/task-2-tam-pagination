import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { employeeUrlPart, TeamLeader } from './models';

const getTeamLeadersApiService = () => apiService(`${employeeUrlPart}/GetTeamLeaders`, 'GET')
  .pipe(
    map((data: AjaxResponse) => (data.response as TeamLeader[])),
  );

export default getTeamLeadersApiService;
