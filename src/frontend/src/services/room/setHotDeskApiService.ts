import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { DeskForRoomDetailsDto, roomUrlPart, SetHotDeskDto } from './models';

const setHotDeskApiService = (dto: SetHotDeskDto) => apiService(
  `${roomUrlPart}/SetHotDesk`,
  'PUT', undefined, undefined,
  JSON.stringify(dto),
)
  .pipe(
    map((data: AjaxResponse) => (data.response as DeskForRoomDetailsDto)),
  );

export default setHotDeskApiService;
