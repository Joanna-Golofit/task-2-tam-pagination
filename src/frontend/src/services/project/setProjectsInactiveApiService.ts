import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { projectUrlPart } from './models';

const setProjectsInactiveApiService = (projectsIds: string[]) => apiService(
  `${projectUrlPart}/SetProjectsInactive`,
  'PUT', undefined, undefined, JSON.stringify(projectsIds),
)
  .pipe(
    map((data: AjaxResponse) => (data.response as boolean)),
  );

export default setProjectsInactiveApiService;
