import { ErrorCodes } from '../../services/common/models';
import {
  FETCH_USER_DETAILS,
  FETCH_USER_DETAILS_SUCCESS,
  IUserDetailsState,
  UserDetailsActionTypes,
  UPDATE_WORKSPACE_TYPE,
  REMOVE_HOTDESK_RESERVATION_USER_SUCCESS,
} from './types';
import { ReservationInfoDto, UserProjectDto } from '../../services/user/models';
import { EmployeeType, WorkspaceType } from '../../services/user/enums';

const initialState: IUserDetailsState = {
  selectedUserId: '',
  item: {
    id: '',
    email: '',
    employeeName: '',
    employeeSurname: '',
    employeeType: EmployeeType.Common,
    workspaceType: WorkspaceType.Office,
    locations: [] as {
      roomId: '',
      buildingName: '',
      floorNumber: 0,
      roomName: '',
      deskNumber: 0,
      locationName: ''
    }[],
    isExternal: false,
    projects: [] as UserProjectDto[],
    reservationInfo: [] as ReservationInfoDto[],
    ledProjectsReservationInfo: [] as ReservationInfoDto[],
    errorCode: ErrorCodes.NoError,
  },
};

export function userDetailsReducer(state = initialState, action: UserDetailsActionTypes): IUserDetailsState {
  switch (action.type) {
    case FETCH_USER_DETAILS:
      return { ...state };
    case FETCH_USER_DETAILS_SUCCESS: {
      const item = { ...action.payload };
      item.ledProjectsReservationInfo = item.ledProjectsReservationInfo
        .filter((ri: ReservationInfoDto) => !item.reservationInfo.some((eri: ReservationInfoDto) => eri.id === ri.id));
      return { ...state, item };
    }
    case UPDATE_WORKSPACE_TYPE: {
      const { item } = { ...state };
      item.workspaceType = action.updateEmployeeWorkspaceTypeDtos[0].workspaceType;
      if (action.updateEmployeeWorkspaceTypeDtos[0].workspaceType === 1) item.locations = [];
      return { ...state, item };
    }
    case REMOVE_HOTDESK_RESERVATION_USER_SUCCESS: {
      const { item } = { ...state };
      item.ledProjectsReservationInfo = item.ledProjectsReservationInfo.filter((dr: ReservationInfoDto) => dr.id !== action.reservationId);
      item.reservationInfo = item.reservationInfo.filter((dr: ReservationInfoDto) => dr.id !== action.reservationId);
      return { ...state, item };
    }
    default:
      return state;
  }
}
