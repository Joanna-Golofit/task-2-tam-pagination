import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { projectUrlPart } from './models';

const setProjectsActiveApiService = (projectsIds: string[]) => apiService(`${projectUrlPart}/SetProjectsActive`,
  'PUT', undefined, undefined, JSON.stringify(projectsIds))
  .pipe(
    map((data: AjaxResponse) => (data.response as boolean)),
  );

export default setProjectsActiveApiService;
