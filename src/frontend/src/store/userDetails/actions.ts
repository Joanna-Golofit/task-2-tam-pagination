import { UpdateEmployeeWorkspaceTypeDto } from '../../services/employee/models';
import { UserDetailsDto } from '../../services/user/models';
import {
  FETCH_USER_DETAILS,
  IFetchUserDetailsAction,

  FETCH_USER_DETAILS_SUCCESS,
  IFetchUserDetailsSuccessAction,

  UPDATE_WORKSPACE_TYPE,
  IUpdateWorkspaceType,

  REMOVE_HOTDESK_RESERVATION_USER_SUCCESS,
  IRemoveHotDeskReservationUserSuccess,
} from './types';

export function fetchUserDetails(userId: string): IFetchUserDetailsAction {
  return {
    type: FETCH_USER_DETAILS,
    userId,
  };
}

export function fetchUserDetailsSuccess(dto: UserDetailsDto): IFetchUserDetailsSuccessAction {
  return {
    type: FETCH_USER_DETAILS_SUCCESS,
    payload: dto,
  };
}

export function updateWorkspaceType(updateEmployeeWorkspaceTypeDtos: UpdateEmployeeWorkspaceTypeDto[])
: IUpdateWorkspaceType {
  return {
    type: UPDATE_WORKSPACE_TYPE,
    updateEmployeeWorkspaceTypeDtos,
  };
}

export function removeHotDeskReservationSuccessUserAction(reservationId: string): IRemoveHotDeskReservationUserSuccess {
  return {
    type: REMOVE_HOTDESK_RESERVATION_USER_SUCCESS,
    reservationId,
  };
}
