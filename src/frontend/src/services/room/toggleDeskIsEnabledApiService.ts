import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { roomUrlPart, ToggleDeskIsEnabledDto } from './models';
import { ErrorCodes } from '../common/models';

const toggleDeskIsEnabledApiService = (dto: ToggleDeskIsEnabledDto) => apiService(
  `${roomUrlPart}/ToggleDesksIsEnabled`,
  'PUT', undefined, undefined,
  JSON.stringify(dto),
)
  .pipe(
    map((data: AjaxResponse) => (data.response as ErrorCodes)),
  );

export default toggleDeskIsEnabledApiService;
