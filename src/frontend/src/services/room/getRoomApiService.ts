import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { RoomDetailsDto, roomUrlPart } from './models';

const getRoomApiService = (roomId: string) => apiService(`${roomUrlPart}/${roomId}`, 'GET')
  .pipe(
    map((data: AjaxResponse) => (data.response as RoomDetailsDto)),
  );

export default getRoomApiService;
