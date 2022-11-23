import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { roomUrlPart, UpdateReservationDto } from './models';

const updateReservationApiService = (updateReservationDto: UpdateReservationDto) => apiService(
  `${roomUrlPart}/UpdateReservation`,
  'PUT', undefined, undefined,
  JSON.stringify(updateReservationDto),
)
  .pipe(
    map((data: AjaxResponse) => (data.response as UpdateReservationDto)),
  );

export default updateReservationApiService;
