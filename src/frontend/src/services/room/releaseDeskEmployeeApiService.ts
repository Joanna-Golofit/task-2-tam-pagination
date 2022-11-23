import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { roomUrlPart, ReleaseDeskEmployeeDto } from './models';

const releaseDeskEmployeeApiService = (releaseDeskEmployeeDto: ReleaseDeskEmployeeDto) => apiService(
  `${roomUrlPart}/releaseDeskEmployee`,
  'POST', undefined, undefined,
  JSON.stringify(releaseDeskEmployeeDto),
)
  .pipe(
    map((data: AjaxResponse) => (data.response as ReleaseDeskEmployeeDto)),
  );

export default releaseDeskEmployeeApiService;
