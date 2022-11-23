import { getWorkspaceTypeDisplayName } from '../../services/user/enums';
import { CLEAN_QUERY, FINISH_SEARCH,
  GET_ALL_USERS_FOR_SEARCHER_SUCCESS,
  IUsersState, LOADING_USERS_SEARCHER, START_SEARCH,
  UPDATE_SELECTION, UPDATE_USER_WORKMODE, UsersActionTypes } from './types';

const initialState: IUsersState = {
  loading: false,
  results: [],
  value: '',
  users: [],
};

export function usersReducer(state = initialState, action: UsersActionTypes): IUsersState {
  switch (action.type) {
    case GET_ALL_USERS_FOR_SEARCHER_SUCCESS: {
      const users = action.payload.users.map((user) => ({
        title: user.displayName,
        description: user.email,
        workspace: getWorkspaceTypeDisplayName(user.workspaceType),
        id: user.id,
        key: user.id,
        projects: user.projectsNames,
      }));
      return { ...state, users };
    }
    case LOADING_USERS_SEARCHER:
      return { ...state, loading: action.isLoading };
    case CLEAN_QUERY:
      return initialState;
    case START_SEARCH:
      return { ...state, loading: true, value: action.query };
    case FINISH_SEARCH:
      return { ...state, loading: false, results: action.results };
    case UPDATE_SELECTION:
      return { ...state, value: action.selection };
    case UPDATE_USER_WORKMODE: {
      const dtos = action.payload;
      const users = [...state.users];
      dtos.forEach((dto) => {
        const index = users.findIndex((u) => u.id === dto.employeeId);
        if (index >= 0) {
          users[index].workspace = getWorkspaceTypeDisplayName(dto.workspaceType);
        }
      });

      return { ...state, users };
    }
    default:
      return state;
  }
}
