import { IRoomState, RoomActionTypes,
  FETCH_ROOM,
  FETCH_ROOM_SUCCESS,
  CLOSE_MODAL,
  OPEN_MODAL,
  SELECT_DESK,
  ADD_DESKS,
  OPEN_DESKS_MODAL,
  SET_ROOM_DETAILS_VIEW,
  CLEAR_ROOM_DETAILS,
  GET_EMPLOYEES_FOR_ROOM_DETAILS_SUCCESS,
  SET_HOT_DESK,
  SET_ROOM_HOT_DESK,
  GET_EMPLOYEES_FOR_ROOM_DETAILS,
  GET_DESK_FOR_ROOM_DETAILS_SUCCESS,
  TOGGLE_DESK_IS_ENABLED } from './types';
import { DeskForRoomDetailsDto, Building } from '../../services/room/models';
import { ErrorCodes } from '../../services/common/models';
import { arrIsNullOrEmpty } from '../../helpers/helpers';

const initialState: IRoomState = {
  isViewDisabled: false,
  projectEmployees: [],
  isOpenAddDesksModal: false,
  isEmployeeDropdownPending: false,
  item: {
    id: '',
    area: 0,
    building: {} as Building,
    capacity: 0,
    floor: 0,
    name: '',
    occupiedDesksCount: 0,
    areaMinLevelPerPerson: 0,
    desksInRoom: [],
    projectsInRoom: [],
    sasTokenForRoomPlans: '',
    errorCode: ErrorCodes.NoError,
    inactiveProjectsAssignedEmployeesCount: 0,
    freeDesksCount: 0,
  },
  modalState: {
    isModalOpen: false,
    isOkBtnOnly: true,
    text: '',
    yesOkFunction: undefined,
  },
  desksSelected: [] as Array<DeskForRoomDetailsDto>,
};

export function roomReducer(state = initialState, action: RoomActionTypes): IRoomState {
  switch (action.type) {
    case FETCH_ROOM:
      return { ...state };
    case FETCH_ROOM_SUCCESS:
      return { ...state,
        item: action.payload,
        desksSelected: [] as Array<DeskForRoomDetailsDto>,
        modalState: { ...state.modalState, isModalOpen: false } };
    case CLEAR_ROOM_DETAILS:
      return initialState;
    case ADD_DESKS:
      return { ...state, modalState: { ...state.modalState, isModalOpen: false } };
    case OPEN_MODAL:
      return { ...state,
        modalState: { ...state.modalState,
          isModalOpen: true,
          text: action.text,
          yesOkFunction: action.yesOkFunction,
          isOkBtnOnly: action.isOkBtnOnly } };
    case CLOSE_MODAL:
      return { ...state, modalState: { ...state.modalState, isModalOpen: false } };
    case SELECT_DESK:
    {
      let desksSelected: Array<DeskForRoomDetailsDto> = [];
      if (state.desksSelected.some((desk) => desk.id === action.desk.id)) {
        desksSelected = state.desksSelected.filter((desk) => desk.id !== action.desk.id);
      } else {
        desksSelected = [...state.desksSelected, action.desk];
      }
      return { ...state, desksSelected };
    }
    case SET_ROOM_DETAILS_VIEW:
      return { ...state, isViewDisabled: action.isDisabled };
    case OPEN_DESKS_MODAL:
      return { ...state, isOpenAddDesksModal: action.isOpen };
    case GET_EMPLOYEES_FOR_ROOM_DETAILS:
      return { ...state, isEmployeeDropdownPending: true };
    case GET_EMPLOYEES_FOR_ROOM_DETAILS_SUCCESS:
      return { ...state, projectEmployees: action.payload, isEmployeeDropdownPending: false };
    case GET_DESK_FOR_ROOM_DETAILS_SUCCESS:
    {
      const item = { ...state.item };
      const updateDesk = item.desksInRoom
        .find((desk: DeskForRoomDetailsDto) => desk.id === action.payload.id);
      if (updateDesk) {
        updateDesk.deskHistory = action.payload.deskHistory;
      }
      return { ...state, item };
    }
    case SET_HOT_DESK: {
      const item = { ...state.item };
      const { deskId, isHotDesk } = action.dto;
      const deskIndex = item.desksInRoom.findIndex((d) => d.id === deskId); // petla
      const { reservations } = item.desksInRoom[deskIndex];

      if (isHotDesk) {
        if (arrIsNullOrEmpty(item.desksInRoom[deskIndex].reservations)) {
          item.freeDesksCount -= 1;
        } else {
          item.occupiedDesksCount -= 1;

          reservations.map((r) => r.employee).forEach((employee) => {
            const desksRemovedEmployee = item.desksInRoom.filter((d) => !d.reservations.map((r) => r.employee.id).includes(employee?.id));
            employee?.projectsNames.forEach((pn) => {
              const otherEmployeeIsInProject = desksRemovedEmployee.some((d) => d.reservations.some((r) => r.employee?.projectsNames.includes(pn)));
              if (!otherEmployeeIsInProject) {
                item.projectsInRoom = item.projectsInRoom.filter((p) => p.name !== pn);
              }
            });
          });
        }
      } else {
        item.freeDesksCount += 1;
      }
      item.desksInRoom[deskIndex].reservations = [];
      item.desksInRoom[deskIndex].isHotDesk = isHotDesk;

      return { ...state, item };
    }

    case TOGGLE_DESK_IS_ENABLED: {
      const roomDetails = { ...state.item };
      const { desksIds, isEnabled } = action.dto;
      for (let i = 0; i < desksIds.length; i++) {
        const desk = roomDetails.desksInRoom.find((d) => d.id === desksIds[i]); // petla
        const { reservations } = desk!;
        if (!isEnabled) {
          if (arrIsNullOrEmpty(reservations)) {
            if (!desk!.isHotDesk) {
              roomDetails.freeDesksCount -= 1;
            }
          } else {
            roomDetails.occupiedDesksCount -= 1;

            reservations.map((r) => r.employee).forEach((employee) => {
              const desksRemovedEmployee = roomDetails.desksInRoom.filter((d) => !d.reservations.map((r) => r.employee.id).includes(employee?.id));
              employee?.projectsNames.forEach((pn) => {
                const otherEmployeeIsInProject = desksRemovedEmployee.some((d) => d.reservations.some((r) => r.employee?.projectsNames.includes(pn)));
                if (!otherEmployeeIsInProject) {
                  roomDetails.projectsInRoom = roomDetails.projectsInRoom.filter((p) => p.name !== pn);
                }
              });
            });
          }
          desk!.reservations = [];
          desk!.isHotDesk = false;
        } else {
          roomDetails.freeDesksCount += 1;
        }
        desk!.isEnabled = isEnabled;
      }

      return { ...state, item: roomDetails };
    }

    case SET_ROOM_HOT_DESK: {
      return { ...state };
    }

    default:
      return state;
  }
}
