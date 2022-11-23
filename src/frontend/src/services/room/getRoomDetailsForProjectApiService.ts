import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { roomUrlPart } from './models';
import { RoomForProjectDto } from '../project/models';

const getRoomDetailsForProjectApiService = (roomId: string) =>
  apiService(`${roomUrlPart}/${roomId}/GetRoomDetailsForProject`, 'GET')
    .pipe(
      map((data: AjaxResponse) => (data.response as RoomForProjectDto)),
    );

export default getRoomDetailsForProjectApiService;
