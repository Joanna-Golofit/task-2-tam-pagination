import { TeamLeader, TeamLeaderProjectForDropdownDto } from '../../services/employee/models';
import { IEmployeesState, EmployeesActionTypes,
  GET_TEAM_LEADERS, GET_TEAM_LEADERS_SUCCESS,

  GET_TEAM_LEADER_PROJECTS_FOR_DROPDOWN_SUCCESS } from './types';

const initialState: IEmployeesState = {
  teamLeaders: [] as TeamLeader[],
  teamLeaderProjects: [] as TeamLeaderProjectForDropdownDto[],
};

export function employeesReducer(state = initialState, action: EmployeesActionTypes): IEmployeesState {
  switch (action.type) {
    case GET_TEAM_LEADERS:
      return { ...state };
    case GET_TEAM_LEADERS_SUCCESS:
      return { ...state,
        teamLeaders: action.payload };
    case GET_TEAM_LEADER_PROJECTS_FOR_DROPDOWN_SUCCESS:
      return { ...state, teamLeaderProjects: action.payload };
    default:
      return state;
  }
}
