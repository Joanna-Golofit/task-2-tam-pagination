import { AjaxResponse } from 'rxjs/ajax';
import { map } from 'rxjs/operators';
import apiService from '../apiService';
import { AddEquipmentDto, equipmentUrlPart } from './models';

const addEquipmentApiService = (addEquipmentDto: AddEquipmentDto) => apiService(
  `${equipmentUrlPart}/AddEquipment`,
  'POST', undefined, undefined,
  JSON.stringify(addEquipmentDto),
)
  .pipe(
    map((data: AjaxResponse) => (data.response as any)),
  );

export default addEquipmentApiService;
