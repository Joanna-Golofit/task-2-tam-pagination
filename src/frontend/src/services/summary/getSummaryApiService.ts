import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { SummaryDto, summaryUrlPart } from './models';

const getSummaryApiService = () => apiService(`${summaryUrlPart}`, 'GET')
  .pipe(
    map((data: AjaxResponse) => (data.response as SummaryDto)),
  );

export default getSummaryApiService;
