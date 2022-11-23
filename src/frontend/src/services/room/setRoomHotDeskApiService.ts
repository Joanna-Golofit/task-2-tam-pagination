import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { roomUrlPart, SetHotDeskDto } from './models';
import { ErrorCodes } from '../common/models';

const setRoomHotDeskApiService = (dto: SetHotDeskDto) => apiService(
  `${roomUrlPart}/SetRoomAsHotDesk`,
  'PUT', undefined, undefined,
  JSON.stringify(dto),
)
  .pipe(
    map((data: AjaxResponse) => (data.response as ErrorCodes)),
  );

export default setRoomHotDeskApiService;
