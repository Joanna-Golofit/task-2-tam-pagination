import { AjaxResponse } from 'rxjs/ajax';
import { map } from 'rxjs/operators';
import apiService from '../apiService';
import { equipmentUrlPart, ReservationEquipmentDto } from './models';

const reserveEquipmentApiService = (reservationEquipmentDto: ReservationEquipmentDto) => apiService(
  `${equipmentUrlPart}/ReserveEquipment`,
  'PUT', undefined, undefined,
  JSON.stringify(reservationEquipmentDto),
).pipe(
  map((data: AjaxResponse) => (data.response as string[])),
);

export default reserveEquipmentApiService;
