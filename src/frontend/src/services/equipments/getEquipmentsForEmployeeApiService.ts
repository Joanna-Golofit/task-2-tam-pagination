import { AjaxResponse } from 'rxjs/ajax';
import { map } from 'rxjs/operators';
import apiService from '../apiService';
import { EmployeeEquipmentDetailDto, equipmentUrlPart } from './models';

const getEquipmentsForEmployeeApiService = (employeeId: string) => apiService(`${equipmentUrlPart}/GetForEmployee?employeeId=${employeeId}`, 'GET')
  .pipe(
    map((data: AjaxResponse) => (data.response as EmployeeEquipmentDetailDto[])),
  );

export default getEquipmentsForEmployeeApiService;
