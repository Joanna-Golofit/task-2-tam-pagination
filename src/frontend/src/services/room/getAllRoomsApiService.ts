import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { roomUrlPart, RoomsDto } from './models';

const getAllRoomsApiService = () => apiService(`${roomUrlPart}`, 'GET')
  .pipe(
    map((data: AjaxResponse) => (data.response as RoomsDto)),
  );

export default getAllRoomsApiService;
