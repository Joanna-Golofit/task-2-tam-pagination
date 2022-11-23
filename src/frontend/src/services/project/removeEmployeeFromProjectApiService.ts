import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { employeeUrlPart } from './models';
import { ErrorCodes } from '../common/models';

const removeEmployeeFromProjectApiService = (id: string) => apiService(
  `${employeeUrlPart}/${id}`,
  'DELETE', undefined, undefined,
  JSON.stringify({ employeeId: id }),
)
  .pipe(
    map((data: AjaxResponse) => (data.response as ErrorCodes)),
  );

export default removeEmployeeFromProjectApiService;
