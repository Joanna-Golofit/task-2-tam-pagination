import { AjaxResponse } from 'rxjs/ajax';
import { map } from 'rxjs/operators';
import apiService from '../apiService';
import { equipmentUrlPart, EditEquipmentDto } from './models';

const editEquipmentApiService = (editEquipmentDto: EditEquipmentDto) => apiService(
  `${equipmentUrlPart}/EditEquipment`,
  'PUT', undefined, undefined,
  JSON.stringify(editEquipmentDto),
).pipe(
  map((data: AjaxResponse) => (data.response as string[])),
);

export default editEquipmentApiService;
