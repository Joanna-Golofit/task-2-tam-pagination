import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { projectUrlPart } from './models';
import { ErrorCodes } from '../common/models';

const removeProjectApiService = (id: string) => apiService(
  `${projectUrlPart}/${id}`,
  'DELETE', undefined, undefined,
  JSON.stringify({ companyId: id }),
)
  .pipe(
    map((data: AjaxResponse) => (data.response as ErrorCodes)),
  );

export default removeProjectApiService;
