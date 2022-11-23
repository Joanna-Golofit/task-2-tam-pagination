import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { userUrlPart } from './models';

const getIsUserAdminApiService = () => apiService(
  `${userUrlPart}/GetIsUserAdmin`, 'GET',
)
  .pipe(
    map((data: AjaxResponse) => (data.response as boolean)),
  );

export default getIsUserAdminApiService;
