import { from, defer, of, throwError, Observable } from 'rxjs';
import { ajax, AjaxResponse } from 'rxjs/ajax';
import { map, catchError, mergeMapTo, switchMap } from 'rxjs/operators';
import { AccessTokenResponse } from 'react-aad-msal';
import { authProvider } from '../auth/authProvider';

type RequestMethods = 'GET' | 'POST' | 'PUT' | 'DELETE';

const baseUrl: string = `${process.env.REACT_APP_TAM_BACKEND_URL}/api/`;
const BACKEND_API_URL_FOR_TOKEN = process.env.REACT_APP_TAM_ENV_NAME === 'dev' ?
  'https://teamsallocationmanagerapi.azurewebsites.net' : process.env.REACT_APP_TAM_BACKEND_URL;
const SESSION_TOKEN_LOCAL_STORAGE = 'tamSessionToken';

const refreshToken = () => from(authProvider.getAccessToken()).pipe(
  switchMap((tokenData: AccessTokenResponse) => ajax({
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    responseType: 'json',
    url: `${BACKEND_API_URL_FOR_TOKEN}/.auth/login/aad`,
    body: { access_token: tokenData.accessToken },
  })),
  switchMap((response: AjaxResponse) => {
    localStorage.setItem(
      SESSION_TOKEN_LOCAL_STORAGE,
      response.response.authenticationToken,
    );
    return of(() => {});
  }),
);

const deferredAjax = (
  url: string,
  method: RequestMethods,
  headers?: object,
  responseType: string = 'json',
  body?: any,
) => defer(() => from(authProvider.getAccessToken()).pipe(
  switchMap((tokenData: AccessTokenResponse) => ajax({
    method,
    body,
    headers: {
      ...headers,
      'X-ZUMO-AUTH': localStorage.getItem(SESSION_TOKEN_LOCAL_STORAGE),
      'X-MS-TOKEN-AAD-ACCESS-TOKEN': tokenData.accessToken,
    },
    responseType,
    url: baseUrl + url,
  })),
));

const apiService = (
  url: string,
  method: RequestMethods,
  headers?: object,
  responseType: string = 'json',
  body?: any,
) => {
  if (!localStorage.getItem(SESSION_TOKEN_LOCAL_STORAGE)) {
    return refreshToken().pipe(
      mergeMapTo(deferredAjax(url, method, headers, responseType, body)),
    );
  }

  return deferredAjax(url, method, headers, responseType, body).pipe(
    map((response: AjaxResponse) => response),
    catchError((error: any, source: Observable<AjaxResponse>) => {
      if (error.status === 401) {
        return refreshToken().pipe(mergeMapTo(source));
      }
      return throwError(error);
    }),
  );
};

export default apiService;
