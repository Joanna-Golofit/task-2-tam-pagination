import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { roomUrlPart } from './models';

const removeTeamFromRoomApiService = (roomId: string, projectId: string) => apiService(
  `${roomUrlPart}/${roomId}/RemoveTeamFromRoom/${projectId}`,
  'PUT', undefined, undefined,
)
  .pipe(
    map((data: AjaxResponse) => ({
      responseStatus: data.status,
    })),
  );

export default removeTeamFromRoomApiService;
