import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import dayjs from 'dayjs';
import apiService from '../apiService';
import { hotDesksUrlPart, HotDesksDateFilters, HotDeskRespones } from './models';
import { queryString } from '../../providers/queryStringProvider';

const getAllHotDesksApiService =
  (query: HotDesksDateFilters) => {
    const startDate = dayjs(query.startDate).format('YYYY-MM-DD');
    const endDate = dayjs(query.endDate).format('YYYY-MM-DD');

    return apiService(`${hotDesksUrlPart}${queryString({ startDate, endDate })}`, 'GET')
      .pipe(
        map((data: AjaxResponse) => (data.response as HotDeskRespones)),
      );
  };

export default getAllHotDesksApiService;
