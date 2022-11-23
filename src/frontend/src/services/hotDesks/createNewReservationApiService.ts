import { AjaxResponse } from 'rxjs/ajax';
import { map } from 'rxjs/operators';
import dayjs from 'dayjs';
import { hotDesksUrlPart, HotDeskReservationDto } from './models';
import apiService from '../apiService';

const createNewReservationApiService = (reserveHotDeskDto: HotDeskReservationDto) => {
  const body = JSON.stringify({
    deskId: reserveHotDeskDto.deskId,
    reservationStart: dayjs(reserveHotDeskDto.reservationStart).format('YYYY-MM-DD'),
    reservationEnd: dayjs(reserveHotDeskDto.reservationEnd).format('YYYY-MM-DD'),
    reservingEmployee: reserveHotDeskDto.reservingEmployee,
  });

  return apiService(`${hotDesksUrlPart}`, 'POST', undefined, undefined, body)
    .pipe(
      map((data: AjaxResponse) => (data.response as string)),
    );
};

export default createNewReservationApiService;
