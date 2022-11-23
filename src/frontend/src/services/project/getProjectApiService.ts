import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { projectUrlPart, ProjectDetails } from './models';

const getProjectApiService = (projectId: string) => apiService(`${projectUrlPart}/${projectId}`, 'GET')
  .pipe(
    map((data: AjaxResponse) => (data.response as ProjectDetails)),
  );

export default getProjectApiService;
