import { Epic, combineEpics } from 'redux-observable';
import { filter, switchMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { ADD_PROJECT, FETCH_PROJECTS } from './types';
import { closeAddProjectModalAction, fetchProjectsSuccess } from './actions';
import { setLoadingAction } from '../global/actions';
import getAllProjectsApiService from '../../services/project/getAllProjectsApiService';
import addProjectApiService from '../../services/project/addProjectApiService';

const projectsEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === FETCH_PROJECTS),
  switchMap(() => getAllProjectsApiService().pipe(
    switchMap((response) => of(fetchProjectsSuccess(response), setLoadingAction(false))),
  )),
);

const addProjectEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === ADD_PROJECT),
  switchMap(({ addDesksDto, navigate }) => addProjectApiService(addDesksDto)
    .pipe(
      switchMap((returnData) => {
        navigate(returnData);
        return of(closeAddProjectModalAction(), setLoadingAction(false));
      }),
    )),
);

const projectsEpics = combineEpics(projectsEpic, addProjectEpic);
export default projectsEpics;
