import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { projectUrlPart, Project } from './models';

const getAllProjectsApiService =
  () => apiService(`${projectUrlPart}`, 'GET')
    .pipe(
      map((data: AjaxResponse) => (data.response as Project[])),
    );

export default getAllProjectsApiService;
