import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { projectUrlPart, AddProjectDto } from './models';

const addProjectApiService = (addExternalCompanyDto: AddProjectDto) => apiService(
  `${projectUrlPart}/NewExternalCompany`,
  'POST', undefined, undefined,
  JSON.stringify(addExternalCompanyDto),
)
  .pipe(
    map((data: AjaxResponse) => (data.response as any)),
  );

export default addProjectApiService;
