import dayjs from 'dayjs';
import { UpdateEmployeeWorkspaceTypeDto } from '../../services/employee/models';
import { RoomForProjectDto } from '../../services/project/models';
import {
  IProjectDetailsState,
  ProjectsActionTypes,
  FETCH_PROJECT_DETAILS,
  FETCH_PROJECT_DETAILS_SUCCESS,
  UPDATE_EMPLOYEES_WORKSPACE_TYPES,
  CLEAR_PROJECT_DETAILS,
  GET_ROOM_DETAILS_FOR_PROJECT_SUCCESS,
  CLEAR_NEW_ROOM,
  ADD_EMPLOYEE_TO_PROJECT,
  REMOVE_EMPLOYEE_FROM_PROJECT,
  REMOVE_PROJECT,
  OPEN_REMOVE_PROJECT_MODAL,
  CLOSE_REMOVE_PROJECT_MODAL,
} from './types';

const initialState: IProjectDetailsState = {
  projectDetailsResponse: {
    assignedPeopleCount: 0,
    endDate: dayjs(),
    id: '',
    name: '',
    email: '',
    peopleCount: 0,
    rooms: [] as RoomForProjectDto[],
    teamLeaders: [],
    employees: [],
  },
  releasingDesks: [],
  newRoom: {} as RoomForProjectDto,
  isRemoveCompanyModalOpen: false,
};

export function projectDetailsReducer(state = initialState, action: ProjectsActionTypes): IProjectDetailsState {
  switch (action.type) {
    case FETCH_PROJECT_DETAILS:
      return { ...state };
    case FETCH_PROJECT_DETAILS_SUCCESS:
      return { ...state, projectDetailsResponse: action.payload };
    case CLEAR_PROJECT_DETAILS:
      return initialState;
    case UPDATE_EMPLOYEES_WORKSPACE_TYPES: {
      const dtos = action.updateEmployeeWorkspaceTypeDtos;
      const { employees } = state.projectDetailsResponse;

      dtos.forEach((ew: UpdateEmployeeWorkspaceTypeDto) => {
        const employeeIndex = employees.findIndex((e) => e.id === ew.employeeId);
        employees[employeeIndex].workmode = ew.workspaceType;

        if (ew.workspaceType === 1) {
          employees[employeeIndex].roomDeskDtos = [];
        }
      });

      return { ...state, projectDetailsResponse: { ...state.projectDetailsResponse, employees } };
    }
    case GET_ROOM_DETAILS_FOR_PROJECT_SUCCESS: {
      return { ...state, newRoom: action.room };
    }
    case CLEAR_NEW_ROOM: {
      return { ...state, newRoom: {} as RoomForProjectDto };
    }
    case ADD_EMPLOYEE_TO_PROJECT:
      return { ...state };
    case REMOVE_EMPLOYEE_FROM_PROJECT:
      return { ...state };
    case REMOVE_PROJECT:
      return { ...state };
    case OPEN_REMOVE_PROJECT_MODAL:
      return { ...state, isRemoveCompanyModalOpen: true };
    case CLOSE_REMOVE_PROJECT_MODAL:
      return { ...state, isRemoveCompanyModalOpen: false };

    default:
      return state;
  }
}
