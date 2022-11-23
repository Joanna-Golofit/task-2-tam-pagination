import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { UsersForSearcherDto, userUrlPart } from './models';

const getAllUsersForSearcherApiService = () => apiService(`${userUrlPart}/getAllUsersForSearcher`, 'GET')
  .pipe(
    map((data: AjaxResponse) => (data.response as UsersForSearcherDto)),
  );

export default getAllUsersForSearcherApiService;
