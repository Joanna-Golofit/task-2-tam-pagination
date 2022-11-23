import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { roomUrlPart, AllocateDesksDto } from './models';

const allocateDesksApiService = (allocateDesksDto: AllocateDesksDto) => apiService(
  `${roomUrlPart}/AllocateDesks`,
  'PUT', undefined, undefined,
  JSON.stringify(allocateDesksDto),
)
  .pipe(
    map((data: AjaxResponse) => (data.response as AllocateDesksDto)),
  );

export default allocateDesksApiService;
