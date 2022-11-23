import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { hotDesksUrlPart } from './models';

const getActiveReservationsForDeskApiService = (deskId: string) => apiService(
  `${hotDesksUrlPart}/GetActiveReservationsForDesk/${deskId}`,
  'GET',
)
  .pipe(
    map((data: AjaxResponse) => (data.response as any)),
  );

export default getActiveReservationsForDeskApiService;
