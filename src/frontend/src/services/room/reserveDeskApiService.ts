import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { roomUrlPart, ReserveDeskDto } from './models';

const reserveDeskApiService = (reserveDeskDto: ReserveDeskDto) => apiService(
  `${roomUrlPart}/ReserveDesk`,
  'POST', undefined, undefined,
  JSON.stringify(reserveDeskDto),
)
  .pipe(
    map((data: AjaxResponse) => (data.response as ReserveDeskDto)),
  );

export default reserveDeskApiService;
