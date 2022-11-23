import { map } from 'rxjs/operators';
import { AjaxResponse } from 'rxjs/ajax';
import apiService from '../apiService';
import { projectUrlPart, ProjectForDropdownDto } from './models';

const getAllProjectsForDropdownApiService = (search?: string) => {
  const query = search ? `?search=${search}` : '';
  return apiService(`${projectUrlPart}/GetAllProjectsForDropdown${query}`, 'GET')
    .pipe(
      map((data: AjaxResponse) => (data.response as Array<ProjectForDropdownDto>)),
    );
};

export default getAllProjectsForDropdownApiService;
