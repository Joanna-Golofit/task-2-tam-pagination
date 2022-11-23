import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { EmployeeForRoomDetailsDto, roomUrlPart } from './models';

const getEmployeesForRoomDetailsApiService = (projectId: string, roomId: string) => apiService(
  `${roomUrlPart}/${roomId}/GetEmployeesForRoomDetails/${projectId}`, 'GET',
)
  .pipe(
    map((data: AjaxResponse) => (data.response as Array<EmployeeForRoomDetailsDto>)),
  );

export default getEmployeesForRoomDetailsApiService;
