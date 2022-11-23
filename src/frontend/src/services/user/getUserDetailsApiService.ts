import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { UserDetailsDto, userUrlPart } from './models';

const getUserDetailsApiService = (userId: string) => apiService(`${userUrlPart}/${userId}`, 'GET')
  .pipe(
    map((data: AjaxResponse) => (data.response as UserDetailsDto)),
  );

export default getUserDetailsApiService;
