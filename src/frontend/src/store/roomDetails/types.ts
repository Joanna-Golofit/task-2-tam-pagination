import { Action } from 'redux';
import {
  RoomDetailsDto,
  DeskForRoomDetailsDto,
  AllocateDesksDto,
  AddDesksDto,
  ReserveDeskDto,
  UpdateReservationDto,
  EmployeeForRoomDetailsDto,
  SetHotDeskDto,
  SetRoomHotDeskDto,
  ToggleDeskIsEnabledDto,
  ReleaseDeskEmployeeDto,
}
  from '../../services/room/models';

export const FETCH_ROOM = 'FETCH_ROOM';
export const FETCH_ROOM_SUCCESS = 'FETCH_ROOM_SUCCESS';
export const REMOVE_TEAM_FROM_ROOM = 'REMOVE_TEAM_FROM_ROOM';
export const RELEASE_DESK_EMPLOYEE = 'RELEASE_DESK_EMPLOYEE';
export const DELETE_DESKS = 'DELETE_DESKS';
export const OPEN_MODAL = 'OPEN_MODAL';
export const CLOSE_MODAL = 'CLOSE_MODAL';
export const SELECT_DESK = 'SELECT_DESK';
export const ALLOCATE_DESKS = 'ALLOCATE_DESKS';
export const RESERVE_DESK = 'RESERVE_DESK';
export const UPDATE_RESERVATION = 'UPDATE_RESERVATION';
export const ADD_DESKS = 'ADD_DESKS';
export const SET_ROOM_DETAILS_VIEW = 'SET_ROOM_DETAILS_VIEW';
export const OPEN_DESKS_MODAL = 'OPEN_DESKS_MODAL';
export const SET_ROOM_HOT_DESK = 'SET_ROOM_HOT_DESK';
export const UPDATE_DESK_PERSON_SUCCESS = 'UPDATE_DESK_PERSON_SUCCESS';
export const CLEAR_ROOM_DETAILS = 'CLEAR_ROOM_DETAILS';
export const GET_EMPLOYEES_FOR_ROOM_DETAILS = 'GET_EMPLOYEES_FOR_ROOM_DETAILS';
export const GET_EMPLOYEES_FOR_ROOM_DETAILS_SUCCESS = 'GET_EMPLOYEES_FOR_ROOM_DETAILS_SUCCESS';
export const GET_DESK_FOR_ROOM_DETAILS_SUCCESS = 'GET_DESK_FOR_ROOM_DETAILS_SUCCESS';
export const SET_HOT_DESK = 'SET_HOT_DESK';
export const TOGGLE_DESK_IS_ENABLED = 'TOGGLE_DESK_IS_ENABLED';

export interface IRoomState {
    isViewDisabled: boolean;
    isEmployeeDropdownPending: boolean;
    projectEmployees: EmployeeForRoomDetailsDto[];
    isOpenAddDesksModal: boolean;
    modalState: IModalState;
    desksSelected: Array<DeskForRoomDetailsDto>;
    item: RoomDetailsDto;
  }

export interface IModalState {
    isModalOpen: boolean;
    isOkBtnOnly: boolean;
    text: string;
    yesOkFunction: (() => void) | undefined;
  }

export interface IFetchRoomAction {
    type: typeof FETCH_ROOM;
    roomId: string
  }

export interface IFetchRoomSuccessAction {
    type: typeof FETCH_ROOM_SUCCESS;
    payload: RoomDetailsDto;
  }

export interface IClearRoomDetails {
    type: typeof CLEAR_ROOM_DETAILS;
  }

export interface IOpenModalAction {
    type: typeof OPEN_MODAL;
    yesOkFunction: any;
    text: string;
    isOkBtnOnly: boolean;
  }

export interface ICloseModalAction {
    type: typeof CLOSE_MODAL;
  }

export interface IRemoveTeamFromRoomAction {
  type: typeof REMOVE_TEAM_FROM_ROOM;
  roomId: string;
  projectId: string;
}

export interface IReleaseDeskEmployeeAction {
  type: typeof RELEASE_DESK_EMPLOYEE;
  releaseDeskEmployeeDto: ReleaseDeskEmployeeDto;
  fetchRoomAction: Action<any>;
}

export interface IDeleteDesksAction {
  type: typeof DELETE_DESKS;
  roomId: string;
  deskIds: Array<string>;
  projectId: string;
  fetchRoomAction: Action<any>;
}

export interface ISelectDeskAction {
  type: typeof SELECT_DESK;
  desk: DeskForRoomDetailsDto;
}

export interface IAllocateDesksAction {
  type: typeof ALLOCATE_DESKS;
  allocateDesksDto: AllocateDesksDto;
}

export interface IReserveDeskAction {
  type: typeof RESERVE_DESK;
  reserveDeskDto: ReserveDeskDto;
  fetchRoomAction: Action<any>;
}

export interface IUpdateReservationAction {
  type: typeof UPDATE_RESERVATION;
  updateReservationDto: UpdateReservationDto;
  roomId: string;
  projectId: string;
  fetchRoomAction: Action<any>;
}

export interface IUpdateDeskPersonSuccessAction {
  type: typeof UPDATE_DESK_PERSON_SUCCESS;
}

export interface IAddDesksAction {
  type: typeof ADD_DESKS;
  addDesksDto: AddDesksDto;
}

export interface ISetRoomDetailsViewAction {
  type: typeof SET_ROOM_DETAILS_VIEW;
  isDisabled: boolean;
}

export interface IOpenAddDesksModalAction {
  type: typeof OPEN_DESKS_MODAL;
  isOpen: boolean;
}

export interface ISetRoomHotDeskAction {
  type: typeof SET_ROOM_HOT_DESK;
  dto: SetRoomHotDeskDto;
  projectId: string;
}

export interface IGetEmployeesForRoomDetails {
  type: typeof GET_EMPLOYEES_FOR_ROOM_DETAILS;
  projectId: string;
  roomId: string;
}

export interface IGetEmployeesForRoomDetailsSuccess {
  type: typeof GET_EMPLOYEES_FOR_ROOM_DETAILS_SUCCESS;
  payload: EmployeeForRoomDetailsDto[];
}

export interface ISetHotDesk {
  type: typeof SET_HOT_DESK;
  dto: SetHotDeskDto;
  projectId: string;
}

export interface IToggleDeskIsEnabledDto {
  type: typeof TOGGLE_DESK_IS_ENABLED;
  dto: ToggleDeskIsEnabledDto;
  projectId: string;
  roomId: string;
  fetchRoomAction: Action<any>;
}

export interface IGetDeskForRoomDetailsSuccess {
  type: typeof GET_DESK_FOR_ROOM_DETAILS_SUCCESS;
  payload: DeskForRoomDetailsDto;
}

export type RoomActionTypes = IFetchRoomAction
                            | IFetchRoomSuccessAction
                            | ICloseModalAction
                            | IOpenModalAction
                            | IRemoveTeamFromRoomAction
                            | IDeleteDesksAction
                            | ISelectDeskAction
                            | IAllocateDesksAction
                            | IAddDesksAction
                            | ISetRoomDetailsViewAction
                            | IOpenAddDesksModalAction
                            | IReserveDeskAction
                            | IUpdateReservationAction
                            | IUpdateDeskPersonSuccessAction
                            | IClearRoomDetails
                            | IGetEmployeesForRoomDetails
                            | IGetEmployeesForRoomDetailsSuccess
                            | ISetHotDesk
                            | IGetDeskForRoomDetailsSuccess
                            | ISetRoomHotDeskAction
                            | IToggleDeskIsEnabledDto;
