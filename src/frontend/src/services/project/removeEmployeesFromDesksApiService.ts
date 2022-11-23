import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { projectUrlPart, RemoveEmployeesFromDesksDto } from './models';
import { ErrorCodes } from '../common/models';

const removeEmployeesFromDesksApiService = (removeEmployeesFromDesksDto: RemoveEmployeesFromDesksDto) => apiService(
  `${projectUrlPart}/RemoveEmployeesFromDesks`,
  'PUT', undefined, undefined,
  JSON.stringify(removeEmployeesFromDesksDto),
)
  .pipe(
    map((data: AjaxResponse) => (data.response as ErrorCodes)),
  );

export default removeEmployeesFromDesksApiService;
