import { AddProjectDto, Project, ProjectsFilterOptions } from '../../services/project/models';

export const FETCH_PROJECTS = 'FETCH_PROJECTS';
export const FETCH_PROJECTS_SUCCESS = 'FETCH_PROJECTS_SUCCESS';
export const SET_PROJECTS_ACTIVE = 'SET_PROJECTS_ACTIVE';
export const SET_PROJECTS_INACTIVE = 'SET_PROJECTS_INACTIVE';
export const SET_PROJECTS_STATE_CHANGED = 'SET_PROJECTS_STATE_CHANGED';

export const ADD_PROJECT = 'ADD_PROJECT';

export const OPEN_ADD_PROJECT_MODAL = 'OPEN_ADD_PROJECT_MODAL';
export const CLOSE_ADD_PROJECT_MODAL = 'CLOSE_ADD_PROJECT_MODAL';

export interface IProjectsState {
  projects: Project[];
  projectsCount: number;
  filters: ProjectsFilterOptions;
  isAddProjectModalOpen: boolean;
}

export interface IFetchProjectsAction {
  type: typeof FETCH_PROJECTS;
}

export interface IFetchProjectsSuccessAction {
  type: typeof FETCH_PROJECTS_SUCCESS;
  response: Project[];
}

export interface ISetProjectsActiveAction {
  type: typeof SET_PROJECTS_ACTIVE;
  projectsIds: string[];
  query: any;
}

export interface ISetProjectsStateChangedAction {
  type: typeof SET_PROJECTS_STATE_CHANGED;
  wasChanged: boolean;
}

export interface ISetProjectsInactiveAction {
  type: typeof SET_PROJECTS_INACTIVE;
  projectsIds: string[];
  query: any;
}

export interface IAddProjectAction {
  type: typeof ADD_PROJECT;
  addDesksDto: AddProjectDto;
  navigate: (id: string) => void;
}

export interface IOpenAddProjectModalAction {
  type: typeof OPEN_ADD_PROJECT_MODAL;
}

export interface ICloseAddProjectModalAction {
  type: typeof CLOSE_ADD_PROJECT_MODAL;
}

export type ProjectsActionTypes = IFetchProjectsAction
                         | IFetchProjectsSuccessAction
                            | ISetProjectsActiveAction
                          | ISetProjectsInactiveAction
                      | ISetProjectsStateChangedAction
                                   | IAddProjectAction
                          | IOpenAddProjectModalAction
                        | ICloseAddProjectModalAction;
