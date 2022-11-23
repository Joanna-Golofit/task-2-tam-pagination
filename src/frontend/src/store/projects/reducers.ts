import { IProjectsState, ProjectsActionTypes, FETCH_PROJECTS, FETCH_PROJECTS_SUCCESS,
  SET_PROJECTS_ACTIVE, SET_PROJECTS_INACTIVE, ADD_PROJECT, OPEN_ADD_PROJECT_MODAL, CLOSE_ADD_PROJECT_MODAL } from './types';

const initialState: IProjectsState = {
  projects: [],
  projectsCount: 0,
  filters: {
    teamLeaderIds: [],
    projectIds: [],
    showYourProjects: false,
    externalCompanies: false,
  },
  isAddProjectModalOpen: false,
};

export function projectsReducer(state = initialState, action: ProjectsActionTypes): IProjectsState {
  switch (action.type) {
    case SET_PROJECTS_ACTIVE:
      return { ...state };
    case SET_PROJECTS_INACTIVE:
      return { ...state };
    case FETCH_PROJECTS:
      return { ...state };
    case FETCH_PROJECTS_SUCCESS:
      return {
        ...state,
        projects: [...action.response],
        projectsCount: action.response?.length,
      };
    case ADD_PROJECT:
      return { ...state };
    case OPEN_ADD_PROJECT_MODAL:
      return { ...state, isAddProjectModalOpen: true };
    case CLOSE_ADD_PROJECT_MODAL:
      return { ...state, isAddProjectModalOpen: false };
    default:
      return state;
  }
}
