import { UpdateEmployeeWorkspaceTypeDto } from '../../services/employee/models';
import { UserDetailsDto } from '../../services/user/models';

export const FETCH_USER_DETAILS = 'FETCH_USER_DETAILS';
export const FETCH_USER_DETAILS_SUCCESS = 'FETCH_USER_DETAILS_SUCCESS';
export const UPDATE_WORKSPACE_TYPE = 'UPDATE_WORKSPACE_TYPE';
export const REMOVE_HOTDESK_RESERVATION_USER_SUCCESS = 'REMOVE_HOTDESK_RESERVATION_USER_SUCCESS';

export interface IUserDetailsState {
    selectedUserId: string;
    item: UserDetailsDto;
  }

export interface IFetchUserDetailsAction {
    type: typeof FETCH_USER_DETAILS;
    userId: string
  }

export interface IFetchUserDetailsSuccessAction {
    type: typeof FETCH_USER_DETAILS_SUCCESS;
    payload: UserDetailsDto;
  }

export interface IUpdateWorkspaceType {
    type: typeof UPDATE_WORKSPACE_TYPE;
    updateEmployeeWorkspaceTypeDtos: UpdateEmployeeWorkspaceTypeDto[];
  }

export interface IRemoveHotDeskReservationUserSuccess {
    type: typeof REMOVE_HOTDESK_RESERVATION_USER_SUCCESS;
    reservationId: string;
  }

export type UserDetailsActionTypes = IFetchUserDetailsAction
                            | IFetchUserDetailsSuccessAction
                            | IUpdateWorkspaceType
                            | IRemoveHotDeskReservationUserSuccess;
