import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { equipmentUrlPart } from './models';
import { ErrorCodes } from '../common/models';

const removeEquipmentApiService = (id: string) => apiService(
  `${equipmentUrlPart}/${id}`,
  'DELETE', undefined, undefined,
)
  .pipe(
    map((data: AjaxResponse) => (data.response as ErrorCodes)),
  );

export default removeEquipmentApiService;
