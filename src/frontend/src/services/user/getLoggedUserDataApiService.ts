import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { LoggedUserDataDto, userUrlPart } from './models';

const getLoggedUserDataApiService = (loggedUserEmail: string) => {
  let uri = '/GetLoggedUserData';
  if (loggedUserEmail) {
    uri = `${uri}?loggedUserEmail=${loggedUserEmail}`;
  }
  return apiService(`${userUrlPart}${uri}`, 'GET')
    .pipe(
      map((data: AjaxResponse) => (data.response as LoggedUserDataDto)),
    );
};

export default getLoggedUserDataApiService;
