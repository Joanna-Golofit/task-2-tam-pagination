import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { roomUrlPart } from './models';

const deleteDesksApiService = (roomId: string, deskIdsToDelete: Array<string>) => apiService(
  `${roomUrlPart}/${roomId}/DeleteDesks`,
  'DELETE', undefined, undefined,
  JSON.stringify(deskIdsToDelete),
)
  .pipe(
    map((data: AjaxResponse) => ({ roomId: data.response.roomId as string,
      deskIds: data.response.deskIds as Array<string>,
      responseStatus: data.status })),
  );

export default deleteDesksApiService;
