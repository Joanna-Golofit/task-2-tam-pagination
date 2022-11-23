import { Action } from 'redux';
import { IFetchRoomAction,
  FETCH_ROOM,
  IFetchRoomSuccessAction,
  FETCH_ROOM_SUCCESS,
  ICloseModalAction,
  CLOSE_MODAL,
  IRemoveTeamFromRoomAction,
  REMOVE_TEAM_FROM_ROOM,
  OPEN_MODAL,
  IOpenModalAction,
  ISelectDeskAction,
  SELECT_DESK,
  DELETE_DESKS,
  IDeleteDesksAction,
  IAllocateDesksAction,
  ALLOCATE_DESKS,
  IAddDesksAction,
  ADD_DESKS,
  IOpenAddDesksModalAction,
  OPEN_DESKS_MODAL,
  IReserveDeskAction,
  RESERVE_DESK,
  IUpdateReservationAction,
  UPDATE_RESERVATION,
  IUpdateDeskPersonSuccessAction,
  UPDATE_DESK_PERSON_SUCCESS,
  ISetRoomDetailsViewAction,
  SET_ROOM_DETAILS_VIEW,
  IClearRoomDetails,
  CLEAR_ROOM_DETAILS,
  IGetEmployeesForRoomDetails,
  GET_EMPLOYEES_FOR_ROOM_DETAILS,
  GET_EMPLOYEES_FOR_ROOM_DETAILS_SUCCESS,
  IGetEmployeesForRoomDetailsSuccess,
  ISetHotDesk,
  SET_HOT_DESK,
  ISetRoomHotDeskAction,
  SET_ROOM_HOT_DESK,
  TOGGLE_DESK_IS_ENABLED,
  IToggleDeskIsEnabledDto,
  RELEASE_DESK_EMPLOYEE,
  IReleaseDeskEmployeeAction,
  GET_DESK_FOR_ROOM_DETAILS_SUCCESS,
  IGetDeskForRoomDetailsSuccess,
} from './types';

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
} from '../../services/room/models';

export function fetchRoom(roomId: string): IFetchRoomAction {
  return {
    type: FETCH_ROOM,
    roomId,
  };
}

export function fetchRoomSuccess(room: RoomDetailsDto): IFetchRoomSuccessAction {
  return {
    type: FETCH_ROOM_SUCCESS,
    payload: room,
  };
}

export function clearRoomDetails(): IClearRoomDetails {
  return {
    type: CLEAR_ROOM_DETAILS,
  };
}

export function openModalAction(text: string,
  yesOkFunction: (()=>void) | undefined = undefined,
  isOkBtnOnly: boolean = false):
  IOpenModalAction {
  return {
    type: OPEN_MODAL,
    yesOkFunction,
    text,
    isOkBtnOnly,
  };
}

export function closeModalAction(): ICloseModalAction {
  return {
    type: CLOSE_MODAL,
  };
}

export function removeTeamFromRoomAction(roomId: string, projectId: string): IRemoveTeamFromRoomAction {
  return {
    type: REMOVE_TEAM_FROM_ROOM,
    roomId,
    projectId,
  };
}

export function releaseDeskEmployeeAction(
  releaseDeskEmployeeDto: ReleaseDeskEmployeeDto,
  fetchRoomAction: Action<any>,
) : IReleaseDeskEmployeeAction {
  return {
    type: RELEASE_DESK_EMPLOYEE,
    releaseDeskEmployeeDto,
    fetchRoomAction,
  };
}

export function deleteDesksAction(roomId: string, deskIds: Array<string>, projectId: string, fetchRoomAction: Action<any>): IDeleteDesksAction {
  return {
    type: DELETE_DESKS,
    roomId,
    deskIds,
    projectId,
    fetchRoomAction,
  };
}

export function selectDeskAction(desk: DeskForRoomDetailsDto): ISelectDeskAction {
  return {
    type: SELECT_DESK,
    desk,
  };
}

export function allocateDesksAction(allocateDesksDto: AllocateDesksDto): IAllocateDesksAction {
  return {
    type: ALLOCATE_DESKS,
    allocateDesksDto,
  };
}

export function reserveDeskAction(reserveDeskDto: ReserveDeskDto, fetchRoomAction: Action<any>): IReserveDeskAction {
  return {
    type: RESERVE_DESK,
    reserveDeskDto,
    fetchRoomAction,
  };
}

export function updateReservationAction(
  updateReservationDto: UpdateReservationDto, roomId: string, projectId: string, fetchRoomAction: Action<any>,
): IUpdateReservationAction {
  return {
    type: UPDATE_RESERVATION,
    updateReservationDto,
    roomId,
    projectId,
    fetchRoomAction,
  };
}

export function updateDeskPersonSuccessAction(): IUpdateDeskPersonSuccessAction {
  return {
    type: UPDATE_DESK_PERSON_SUCCESS,
  };
}

export function addDesksAction(addDesksDto: AddDesksDto): IAddDesksAction {
  return {
    type: ADD_DESKS,
    addDesksDto,
  };
}

export function setRoomDetailsViewAction(isDisabled: boolean): ISetRoomDetailsViewAction {
  return {
    type: SET_ROOM_DETAILS_VIEW,
    isDisabled,
  };
}

export function openAddDesksModalAction(isOpen: boolean): IOpenAddDesksModalAction {
  return {
    type: OPEN_DESKS_MODAL,
    isOpen,
  };
}

export function getEmployeesForRoomDetails(projectId: string, roomId: string): IGetEmployeesForRoomDetails {
  return {
    type: GET_EMPLOYEES_FOR_ROOM_DETAILS,
    projectId,
    roomId,
  };
}

export function getEmployeesForRoomDetailsSuccess(employees: EmployeeForRoomDetailsDto[]):
IGetEmployeesForRoomDetailsSuccess {
  return {
    type: GET_EMPLOYEES_FOR_ROOM_DETAILS_SUCCESS,
    payload: employees,
  };
}

export function setHotDesk(dto: SetHotDeskDto, projectId: string): ISetHotDesk {
  return {
    type: SET_HOT_DESK,
    dto,
    projectId,
  };
}

export function toggleDeskIsEnabled(dto: ToggleDeskIsEnabledDto, projectId: string, roomId: string, fetchRoomAction: Action<any>): IToggleDeskIsEnabledDto {
  return {
    type: TOGGLE_DESK_IS_ENABLED,
    dto,
    projectId,
    roomId,
    fetchRoomAction,
  };
}

export function setRoomHotDeskAction(dto: SetRoomHotDeskDto, projectId: string): ISetRoomHotDeskAction {
  return {
    type: SET_ROOM_HOT_DESK,
    dto,
    projectId,
  };
}

export function getDeskForRoomDetailsSuccess(payload: DeskForRoomDetailsDto): IGetDeskForRoomDetailsSuccess {
  return {
    type: GET_DESK_FOR_ROOM_DETAILS_SUCCESS,
    payload,
  };
}
