import { AjaxResponse } from 'rxjs/ajax';
import { map } from 'rxjs/operators';
import apiService from '../apiService';
import { ImportReportDto } from './models';

const executeImportFromBubblesApiService = () => apiService('import', 'POST')
  .pipe(
    map((data: AjaxResponse) => (data.response as ImportReportDto)),
  );

export default executeImportFromBubblesApiService;
