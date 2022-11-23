import { TeamLeader, TeamLeaderProjectForDropdownDto } from '../../services/employee/models';

export const GET_TEAM_LEADERS = 'GET_TEAM_LEADERS';
export const GET_TEAM_LEADERS_SUCCESS = 'GET_TEAM_LEADERS_SUCCESS';

export const GET_TEAM_LEADER_PROJECTS_FOR_DROPDOWN = 'GET_TEAM_LEADER_PROJECTS_FOR_DROPDOWN';
export const GET_TEAM_LEADER_PROJECTS_FOR_DROPDOWN_SUCCESS = 'GET_TEAM_LEADER_PROJECTS_FOR_DROPDOWN_SUCCESS';

export interface IEmployeesState {
  teamLeaders: TeamLeader[];
  teamLeaderProjects: TeamLeaderProjectForDropdownDto[];
}

export interface IGetTeamLeadersAction {
  type: typeof GET_TEAM_LEADERS;
}

export interface IGetTeamLeadersSuccessAction {
  type: typeof GET_TEAM_LEADERS_SUCCESS;
  payload: TeamLeader[]
}

export interface IGetTeamLeaderProjectsForDropdownAction {
  type: typeof GET_TEAM_LEADER_PROJECTS_FOR_DROPDOWN;
  teamLeaderId: string;
}

export interface IGetTeamLeaderProjectsForDropdownSuccessAction {
  type: typeof GET_TEAM_LEADER_PROJECTS_FOR_DROPDOWN_SUCCESS;
  payload: TeamLeaderProjectForDropdownDto[];
}

export type EmployeesActionTypes = IGetTeamLeadersAction
                         | IGetTeamLeadersSuccessAction
                         | IGetTeamLeaderProjectsForDropdownAction
                         | IGetTeamLeaderProjectsForDropdownSuccessAction;
