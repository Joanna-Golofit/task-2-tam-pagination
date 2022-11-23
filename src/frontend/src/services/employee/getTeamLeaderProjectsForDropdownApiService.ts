import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { employeeUrlPart, TeamLeaderProjectForDropdownDto } from './models';

const getTeamLeaderProjectsForDropdownApiService = (teamLeaderId: string) => apiService(
  `${employeeUrlPart}/GetTeamLeaderProjectsForDropdown/${teamLeaderId}`,
  'GET',
)
  .pipe(
    map((data: AjaxResponse) => (data.response as TeamLeaderProjectForDropdownDto[])),
  );

export default getTeamLeaderProjectsForDropdownApiService;
