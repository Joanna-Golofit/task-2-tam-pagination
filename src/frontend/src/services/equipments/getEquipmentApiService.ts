import { AjaxResponse } from 'rxjs/ajax';
import { map } from 'rxjs/operators';
import apiService from '../apiService';
import { EquipmentDetails, equipmentUrlPart } from './models';

const getEquipmentApiService = (equipmentId: string) => apiService(`${equipmentUrlPart}/${equipmentId}`, 'GET')
  .pipe(
    map((data: AjaxResponse) => (data.response as EquipmentDetails)),
  );

export default getEquipmentApiService;
