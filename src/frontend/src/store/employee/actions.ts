import {
  TeamLeader, TeamLeaderProjectForDropdownDto } from '../../services/employee/models';
import {
  GET_TEAM_LEADERS,
  GET_TEAM_LEADERS_SUCCESS,
  GET_TEAM_LEADER_PROJECTS_FOR_DROPDOWN,
  GET_TEAM_LEADER_PROJECTS_FOR_DROPDOWN_SUCCESS,
  IGetTeamLeaderProjectsForDropdownAction,
  IGetTeamLeaderProjectsForDropdownSuccessAction,
  IGetTeamLeadersAction,
  IGetTeamLeadersSuccessAction,
} from './types';

export function getTeamLeadersAction(): IGetTeamLeadersAction {
  return {
    type: GET_TEAM_LEADERS,
  };
}

export function getTeamLeadersSuccessAction(teamLeaders: TeamLeader[]): IGetTeamLeadersSuccessAction {
  return {
    type: GET_TEAM_LEADERS_SUCCESS,
    payload: teamLeaders,
  };
}

export function getTeamLeaderProjectsForDropdownAction(teamLeaderId: string):
IGetTeamLeaderProjectsForDropdownAction {
  return {
    type: GET_TEAM_LEADER_PROJECTS_FOR_DROPDOWN,
    teamLeaderId,
  };
}

export function getTeamLeaderProjectsForDropdownSuccessAction(teamLeaderProjects: TeamLeaderProjectForDropdownDto[]):
IGetTeamLeaderProjectsForDropdownSuccessAction {
  return {
    type: GET_TEAM_LEADER_PROJECTS_FOR_DROPDOWN_SUCCESS,
    payload: teamLeaderProjects,
  };
}
