import { AjaxResponse } from 'rxjs/ajax';
import { map } from 'rxjs/operators';
import apiService from '../apiService';
import { Equipment, equipmentUrlPart } from './models';

const getAllEquipmentsApiService =
  () => apiService(`${equipmentUrlPart}`, 'GET')
    .pipe(
      map((data: AjaxResponse) => (data.response as Equipment[])),
    );

export default getAllEquipmentsApiService;
