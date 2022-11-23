import { UpdateEmployeeWorkspaceTypeDto } from '../../services/employee/models';
import {
  AddEmployeeToProjectDto, ProjectDetails,
  RemoveEmployeeFromProjectDto,
  RoomForProjectDto,
} from '../../services/project/models';
import { DeskForRoomDetailsDto, ReserveDeskDto } from '../../services/room/models';

export const FETCH_PROJECT_DETAILS = 'FETCH_PROJECT_DETAILS';
export const FETCH_PROJECT_DETAILS_SUCCESS = 'FETCH_PROJECT_DETAILS_SUCCESS';

export const UPDATE_EMPLOYEES_WORKSPACE_TYPES = 'UPDATE_EMPLOYEES_WORKSPACE_TYPES';

export const GET_IS_USER_ADMIN = 'GET_IS_USER_ADMIN';
export const GET_IS_USER_ADMIN_SUCCESS = 'GET_IS_USER_ADMIN_SUCCESS';

export const SET_ASSIGNED_DESKS_COUNT = 'SET_ASSIGNED_DESKS_COUNT';
export const CLEAR_ASSIGNED_DESKS_COUNT = 'CLEAR_ASSIGNED_DESKS_COUNT';

export const SET_IS_LOADING_PROJECT = 'SET_IS_LOADING_PROJECT';

export const GET_ROOM_DETAILS_FOR_PROJECT = 'GET_ROOM_DETAILS_FOR_PROJECT';
export const GET_ROOM_DETAILS_FOR_PROJECT_SUCCESS = 'GET_ROOM_DETAILS_FOR_PROJECT_SUCCESS';

export const CLEAR_PROJECT_DETAILS = 'CLEAR_PROJECT_DETAILS';

export const ASSIGN_EMPLOYEES_TO_DESKS = 'ASSIGN_EMPLOYEE_TO_DESK';
export const REMOVE_EMPLOYEES_FROM_ROOM = 'REMOVE_EMPLOYEE_FROM_ROOM';

export const REMOVE_PROJECT_FROM_ROOM = 'REMOVE_PROJECT_FROM_ROOM';

export const CLEAR_NEW_ROOM = 'CLEAR_SELECTED_ROOM';

export const ADD_EMPLOYEE_TO_PROJECT = 'ADD_EMPLOYEE_TO_PROJECT';

export const REMOVE_EMPLOYEE_FROM_PROJECT = 'REMOVE_EMPLOYEE_FROM_PROJECT';

export const REMOVE_PROJECT = 'REMOVE_PROJECT';

export const OPEN_REMOVE_PROJECT_MODAL = 'OPEN_MODAL';
export const CLOSE_REMOVE_PROJECT_MODAL = 'CLOSE_MODAL';

export interface ReleasingDeskState {
  roomId: string;
  deskId: string;
}

export interface DesksInRoom {
  roomId: string;
  desks: DeskForRoomDetailsDto[];
}

export interface IProjectDetailsState {
  projectDetailsResponse: ProjectDetails;
  releasingDesks: ReleasingDeskState[];
  newRoom: RoomForProjectDto;
  isRemoveCompanyModalOpen: boolean;
}

export interface IFetchProjectDetailsAction {
  type: typeof FETCH_PROJECT_DETAILS;
  id: string;
}

export interface IFetchProjectDetailsSuccessAction {
  type: typeof FETCH_PROJECT_DETAILS_SUCCESS;
  payload: ProjectDetails;
}

export interface IUpdateEmployeesWorkspaceTypesAction {
  type: typeof UPDATE_EMPLOYEES_WORKSPACE_TYPES;
  updateEmployeeWorkspaceTypeDtos: UpdateEmployeeWorkspaceTypeDto[];
  projectId: string;
}

export interface IGetRoomDetails {
  type: typeof GET_ROOM_DETAILS_FOR_PROJECT;
  roomId: string,
}

export interface IGetRoomDetailsSuccess {
  type: typeof GET_ROOM_DETAILS_FOR_PROJECT_SUCCESS;
  room: RoomForProjectDto,
}

export interface IClearProjectDetails {
  type: typeof CLEAR_PROJECT_DETAILS;
}

export interface IAssignEmployeesToDesksAction {
  type: typeof ASSIGN_EMPLOYEES_TO_DESKS;
  reserveDeskDto: ReserveDeskDto;
  projectId: string;
}

export interface IRemoveEmployeesFromRoomAction {
  type: typeof REMOVE_EMPLOYEES_FROM_ROOM;
  roomId: string;
  projectId: string;
}

export interface IClearNewRoom {
  type: typeof CLEAR_NEW_ROOM;
}

export interface IAddEmployeeToProjectAction {
  type: typeof ADD_EMPLOYEE_TO_PROJECT;
  payload: AddEmployeeToProjectDto;
}

export interface IRemoveEmployeeFromProjectAction {
  type: typeof REMOVE_EMPLOYEE_FROM_PROJECT;
  payload: RemoveEmployeeFromProjectDto;
}

export interface IRemoveProjectAction {
  type: typeof REMOVE_PROJECT;
  id: string;
  navigate: () => void;
}

export interface IOpenRemoveProjectModalAction {
  type: typeof OPEN_REMOVE_PROJECT_MODAL;
}

export interface ICloseRemoveProjectModalAction {
  type: typeof CLOSE_REMOVE_PROJECT_MODAL;
}

export type ProjectsActionTypes =
  IFetchProjectDetailsAction |
  IFetchProjectDetailsSuccessAction |
  IUpdateEmployeesWorkspaceTypesAction |
  IGetRoomDetails |
  IGetRoomDetailsSuccess |
  IClearProjectDetails |
  IAssignEmployeesToDesksAction |
  IRemoveEmployeesFromRoomAction |
  IClearNewRoom |
  IAddEmployeeToProjectAction |
  IRemoveEmployeeFromProjectAction |
  IRemoveProjectAction |
  IOpenRemoveProjectModalAction |
  ICloseRemoveProjectModalAction;
