import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { AddEmployeeToProjectDto, projectUrlPart } from './models';
import { ErrorCodes } from '../common/models';

const addEmployeeToProjectApiService = (addEployeeToCompanyDto: AddEmployeeToProjectDto) => apiService(
  `${projectUrlPart}/${addEployeeToCompanyDto.companyId}/AddEmployees?employeeCount=${addEployeeToCompanyDto.employeeCount}`,
  'POST', undefined, undefined,
  JSON.stringify(addEployeeToCompanyDto),
)
  .pipe(
    map((data: AjaxResponse) => (data.response as ErrorCodes)),
  );

export default addEmployeeToProjectApiService;
