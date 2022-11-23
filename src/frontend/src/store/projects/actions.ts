import { AddProjectDto, Project } from '../../services/project/models';
import {
  IFetchProjectsAction,
  FETCH_PROJECTS,
  IFetchProjectsSuccessAction,
  FETCH_PROJECTS_SUCCESS,
  IAddProjectAction,
  ADD_PROJECT,
  IOpenAddProjectModalAction,
  OPEN_ADD_PROJECT_MODAL,
  CLOSE_ADD_PROJECT_MODAL,
  ICloseAddProjectModalAction,
} from './types';

export function fetchProjects(): IFetchProjectsAction {
  return {
    type: FETCH_PROJECTS,
  };
}

export function fetchProjectsSuccess(response: Project[]): IFetchProjectsSuccessAction {
  return {
    type: FETCH_PROJECTS_SUCCESS,
    response,
  };
}

export function addProjectAction(addDesksDto: AddProjectDto, navigate: (id: string) => void): IAddProjectAction {
  return {
    type: ADD_PROJECT,
    addDesksDto,
    navigate,
  };
}

export function openAddProjectModalAction(): IOpenAddProjectModalAction {
  return {
    type: OPEN_ADD_PROJECT_MODAL,
  };
}

export function closeAddProjectModalAction(): ICloseAddProjectModalAction {
  return {
    type: CLOSE_ADD_PROJECT_MODAL,
  };
}
