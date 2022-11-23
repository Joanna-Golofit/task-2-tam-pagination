import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { ErrorCodes } from '../common/models';
import { AssignEmployeesToDesksDto, projectUrlPart } from './models';

const assignEmployeesToDesksApiService = (assignEmployeesToDesksDto: AssignEmployeesToDesksDto) => apiService(
  `${projectUrlPart}/AssignEmployeesToDesks`,
  'PUT', undefined, undefined,
  JSON.stringify(assignEmployeesToDesksDto),
)
  .pipe(
    map((data: AjaxResponse) => (data.response as ErrorCodes)),
  );

export default assignEmployeesToDesksApiService;
