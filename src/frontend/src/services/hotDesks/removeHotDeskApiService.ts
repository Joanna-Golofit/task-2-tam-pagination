import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { hotDesksUrlPart } from './models';

const removeHotDeskApiService = (reservationId: string) => apiService(
  `${hotDesksUrlPart}/${reservationId}`,
  'DELETE', undefined, undefined,
  JSON.stringify({}),
)
  .pipe(
    map((data: AjaxResponse) => (data.response as any)),
  );

export default removeHotDeskApiService;
