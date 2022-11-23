import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { employeeUrlPart, UpdateEmployeeWorkspaceTypeDto } from './models';

const updateEmployeesWorkspaceTypesApiService =
(updateEmployeeWorkspaceTypeDtos: UpdateEmployeeWorkspaceTypeDto[]) => apiService(
  `${employeeUrlPart}/UpdateEmployeesWorkspaceTypes`,
  'PUT', undefined, undefined, JSON.stringify(updateEmployeeWorkspaceTypeDtos),
).pipe(
  map((retData: AjaxResponse) => ({ responseStatus: retData.status })),
);

export default updateEmployeesWorkspaceTypesApiService;
