import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { hotDesksUrlPart } from './models';

const getActiveReservationsForEmployeeApiService = (employeeId: string) => apiService(
  `${hotDesksUrlPart}/GetActiveReservationsForEmployee/${employeeId}`,
  'GET',
)
  .pipe(
    map((data: AjaxResponse) => (data.response as any)),
  );

export default getActiveReservationsForEmployeeApiService;
