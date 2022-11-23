import {
  IFetchProjectDetailsAction,
  FETCH_PROJECT_DETAILS,
  IFetchProjectDetailsSuccessAction,
  FETCH_PROJECT_DETAILS_SUCCESS,
  IUpdateEmployeesWorkspaceTypesAction,
  UPDATE_EMPLOYEES_WORKSPACE_TYPES,
  IGetRoomDetails,
  IGetRoomDetailsSuccess,
  IClearProjectDetails,
  CLEAR_PROJECT_DETAILS,
  GET_ROOM_DETAILS_FOR_PROJECT,
  GET_ROOM_DETAILS_FOR_PROJECT_SUCCESS,
  REMOVE_EMPLOYEES_FROM_ROOM,
  ASSIGN_EMPLOYEES_TO_DESKS,
  IRemoveEmployeesFromRoomAction,
  IAssignEmployeesToDesksAction,
  IClearNewRoom,
  CLEAR_NEW_ROOM,
  IOpenRemoveProjectModalAction,
  ICloseRemoveProjectModalAction,
  CLOSE_REMOVE_PROJECT_MODAL,
  OPEN_REMOVE_PROJECT_MODAL,
  IRemoveProjectAction,
  REMOVE_PROJECT,
  REMOVE_EMPLOYEE_FROM_PROJECT,
  IRemoveEmployeeFromProjectAction,
  IAddEmployeeToProjectAction,
  ADD_EMPLOYEE_TO_PROJECT,
} from './types';
import { UpdateEmployeeWorkspaceTypeDto } from '../../services/employee/models';
import {
  AddEmployeeToProjectDto, ProjectDetails,
  RemoveEmployeeFromProjectDto,
  RoomForProjectDto,
} from '../../services/project/models';
import { ReserveDeskDto } from '../../services/room/models';

export function fetchProjectDetails(id: string): IFetchProjectDetailsAction {
  return {
    type: FETCH_PROJECT_DETAILS,
    id,
  };
}

export function fetchProjectsSuccess(projectDetails: ProjectDetails): IFetchProjectDetailsSuccessAction {
  return {
    type: FETCH_PROJECT_DETAILS_SUCCESS,
    payload: projectDetails,
  };
}

export function clearProjectDetails(): IClearProjectDetails {
  return {
    type: CLEAR_PROJECT_DETAILS,
  };
}

export function updateEmployeesWorkspaceTypesAction(
  updateEmployeeWorkspaceTypeDtos: UpdateEmployeeWorkspaceTypeDto[],
  projectId: string,
): IUpdateEmployeesWorkspaceTypesAction {
  return {
    type: UPDATE_EMPLOYEES_WORKSPACE_TYPES,
    updateEmployeeWorkspaceTypeDtos,
    projectId,
  };
}

export function getRoomDetailsForProject(roomId: string):
  IGetRoomDetails {
  return {
    type: GET_ROOM_DETAILS_FOR_PROJECT,
    roomId,
  };
}

export function getRoomDetailsForProjectSuccess(room: RoomForProjectDto):
  IGetRoomDetailsSuccess {
  return {
    type: GET_ROOM_DETAILS_FOR_PROJECT_SUCCESS,
    room,
  };
}

export function assignEmployeesToDesksAction(
  reserveDeskDto: ReserveDeskDto, projectId: string,
): IAssignEmployeesToDesksAction {
  return {
    type: ASSIGN_EMPLOYEES_TO_DESKS,
    reserveDeskDto,
    projectId,
  };
}

export function removeEmployeesFromRoomAction(roomId: string, projectId: string):
  IRemoveEmployeesFromRoomAction {
  return {
    type: REMOVE_EMPLOYEES_FROM_ROOM,
    roomId,
    projectId,
  };
}

export function clearNewRoomAction():
  IClearNewRoom {
  return {
    type: CLEAR_NEW_ROOM,
  };
}

export function addEmployeeToProject(payload: AddEmployeeToProjectDto): IAddEmployeeToProjectAction {
  return {
    type: ADD_EMPLOYEE_TO_PROJECT,
    payload,
  };
}

export function removeEmployeeFromProject(payload: RemoveEmployeeFromProjectDto): IRemoveEmployeeFromProjectAction {
  return {
    type: REMOVE_EMPLOYEE_FROM_PROJECT,
    payload,
  };
}

export function removeProjectAction(id: string, navigate: () => void): IRemoveProjectAction {
  return {
    type: REMOVE_PROJECT,
    id,
    navigate,
  };
}

export function openRemoveProjectModalAction(): IOpenRemoveProjectModalAction {
  return {
    type: OPEN_REMOVE_PROJECT_MODAL,
  };
}

export function closeRemoveProjectModalAction(): ICloseRemoveProjectModalAction {
  return {
    type: CLOSE_REMOVE_PROJECT_MODAL,
  };
}
