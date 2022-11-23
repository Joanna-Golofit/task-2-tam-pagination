import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { roomUrlPart, AllocateDesksDto, AddDesksDto } from './models';

const addDesksApiService = (addDesksDto: AddDesksDto) => apiService(
  `${roomUrlPart}/AddDesks`,
  'POST', undefined, undefined,
  JSON.stringify(addDesksDto),
)
  .pipe(
    map((data: AjaxResponse) => (data.response as AllocateDesksDto)),
  );

export default addDesksApiService;
