import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { Action } from 'redux';
import {
  Button,
  DropdownItemProps,
  DropdownProps,
  Grid,
  Icon,
  Loader,
  Statistic,
  Table,
} from 'semantic-ui-react';
import {
  arrIsNullOrEmpty,
  stringFormat,
  strIsNullOrEmpty,
} from '../../helpers/helpers';
import {
  getEmployeeReservationId,
  getFreeWeekdays,
  getReservedWeekdaysByEmployee,
  getReservedWeekdaysByOtherEmployees,
} from '../../helpers/ReservationsHelpers';
import ModalWithContent from '../../layouts/components/ModalWithContent';
import SimpleModal from '../../layouts/components/SimpleModal';
import { EmployeeForProjectDetailsDto } from '../../services/project/models';
import {
  DeskForRoomDetailsDto,
  DeskReservationDto,
  EmployeeForRoomDetailsDto,
} from '../../services/room/models';
import { AppState } from '../../store';
import {
  setLoadingAction,
  showNotifyAction,
} from '../../store/global/actions';
import {
  deleteDesksAction,
  releaseDeskEmployeeAction,
  reserveDeskAction,
  setHotDesk,
  toggleDeskIsEnabled,
  updateReservationAction,
} from '../../store/roomDetails/actions';
import DeskHistoryPeek from '../DeskHistoryPeek/DeskHistoryPeek';
import WeekPopup from '../WeekPopup';
import styles from './DeskRow.module.scss';
import EmployeesDropdown from './EmployeeDropdown';
import EmployeeProjectsAccordion from './employeeProjectsAccordion';

type Props = {
  desk: DeskForRoomDetailsDto;
  disabledEmployees: string[];
  editMode: boolean;
  employees: (EmployeeForRoomDetailsDto | EmployeeForProjectDetailsDto)[];
  projectId: string;
  roomFetchAction: Action<any>;
  roomId: string;
};

const DeskRow: React.FC<Props> = ({
  desk,
  disabledEmployees,
  editMode = false,
  employees,
  projectId,
  roomId,
  roomFetchAction,
}) => {
  const dispatch = useDispatch();
  const { t } = useTranslation();

  const { loggedUserData } = useSelector((state: AppState) => state.global);

  const [deleteModal, setDeleteModal] = useState({ isOpen: false, yesFunction: () => { }, message: '', body: '' });
  const [employeeOptions, setEmployeeOptions] = useState([] as DropdownItemProps[]);
  const [isLoaderActive, setIsLoaderActive] = useState(false);
  const [selectedEmployeeId, setSelectedEmployeeId] = useState('');

  useEffect(() => {
    setIsLoaderActive(false);
    handleEmployeeOptions();
  }, [desk.reservations]);

  useEffect(() => {
    handleEmployeeOptions();
  }, [employees]);

  const setCalendarOnClose = (reservationId: string, scheduledWeekdays: number[]) => {
    const deskReservation = desk.reservations.find((dr) => dr.id === reservationId);
    const scheduledWeekdaysChanged = JSON.stringify(deskReservation?.scheduledWeekdays.sort()) !== JSON.stringify(scheduledWeekdays.sort());

    if (!reservationId || !scheduledWeekdaysChanged) {
      return;
    }

    dispatch(setLoadingAction(true));
    dispatch(updateReservationAction({ reservationId, employeeId: deskReservation?.employee.id || '', scheduledWeekdays }, roomId, '', roomFetchAction));
  };

  function handleEmployeeOptions(): void {
    if (!strIsNullOrEmpty(projectId) && arrIsNullOrEmpty(employees)) {
      dispatch(showNotifyAction(t('roomDetails.noEmployees'), false));
      setEmployeeOptions([] as EmployeeForRoomDetailsDto[]);
    } else {
      setEmployeeOptions(employees.map((e) => ({
        key: e.id,
        text: `${e.name} ${e.surname}`,
        value: e.id,
        disabled: disabledEmployees?.some((de) => de === e.id),
      })));
    }
  }

  const selectEmployeeHandler = (
    _: React.SyntheticEvent<HTMLElement, Event>,
    data: DropdownProps,
    deskReservation: DeskReservationDto | null,
    dispatchAction: Function,
  ) => {
    setIsLoaderActive(true);
    const selectedEmployee = employees.find((e) => e.id === data.value as string);
    if (selectedEmployee?.roomDeskDtos.length === 0) {
      dispatch(setLoadingAction(true));
      dispatchAction(selectedEmployee, deskReservation);
      setSelectedEmployeeId('');
    } else {
      setDeleteModal({
        isOpen: true,
        body: '',
        message: t('projectDetails.assignmentExistsMessage',
          { location: selectedEmployee?.roomDeskDtos.map((dto) => `${dto.roomName} b. ${dto.deskNumber}`) })
          .replace(new RegExp(',', 'g'), ', '),
        yesFunction: () => {
          dispatch(setLoadingAction(true));
          dispatchAction(selectedEmployee, deskReservation);
          setDeleteModal((prevState) => ({ ...prevState, isOpen: false }));
          setSelectedEmployeeId('');
        },
      });
    }
  };

  const dispatchReserveDeskAction = (selectedEmployee: EmployeeForRoomDetailsDto) =>
    dispatch(reserveDeskAction({
      deskId: desk.id,
      employeeId: selectedEmployee.id,
      scheduledWeekdays: getFreeWeekdays(desk.reservations),
    }, roomFetchAction));

  const removeEmployeeHandler = (deskReservation: DeskReservationDto) => {
    setDeleteModal({
      isOpen: true,
      body: '',
      message: stringFormat(t('roomDetails.removeEmployeeFromDesk'), `${deskReservation.employee.name} ${deskReservation.employee.surname}`, `${desk.number}`),
      yesFunction: () => {
        setIsLoaderActive(true);
        dispatch(setLoadingAction(true));
        dispatch(releaseDeskEmployeeAction({ employeeId: deskReservation.employee.id, deskId: desk.id }, roomFetchAction));
        setDeleteModal((prevState) => ({ ...prevState, isOpen: false }));
      },
    });
  };

  const removeDeskHandler = () => {
    setIsLoaderActive(true);
    setDeleteModal({
      isOpen: true,
      body: (desk.reservations?.length > 0) ? t('roomDetails.areYouSureToChangePlans') : '',
      message: (desk.reservations?.length > 0) ? t('roomDetails.removeDeskMessage') : t('roomDetails.areYouSureToChangePlans'),
      yesFunction: () => {
        dispatch(setLoadingAction(true));
        dispatch(deleteDesksAction(roomId, [desk.id], projectId, roomFetchAction));
        setDeleteModal((prevState) => ({ ...prevState, isOpen: false }));
      },
    });
  };

  const setHotDeskToggleHandler = () => {
    if (desk.reservations.length) {
      setDeleteModal({
        isOpen: true,
        body: '',
        // check based on new state
        message: !desk.isHotDesk ? t('roomDetails.setHotDeskMessage') : t('roomDetails.unsetHotDeskMessage'),
        yesFunction: () => {
          dispatch(setLoadingAction(true));
          dispatch(setHotDesk({ roomId, deskId: desk.id, isHotDesk: !desk.isHotDesk }, projectId));
          setDeleteModal((prevState) => ({ ...prevState, isOpen: false }));
        },
      });
    } else {
      dispatch(setLoadingAction(true));
      dispatch(setHotDesk({ roomId, deskId: desk.id, isHotDesk: !desk.isHotDesk }, projectId));
    }
  };

  const toggleDeskIsEnabledHandler = () => {
    const desksIds = [desk.id];
    if (desk.reservations.length && desk.isEnabled) {
      setDeleteModal({
        isOpen: true,
        body: '',
        message: t('roomDetails.setDisabledDeskMessage'),
        yesFunction: () => {
          dispatch(setLoadingAction(true));
          dispatch(toggleDeskIsEnabled({ desksIds, isEnabled: !desk.isEnabled }, projectId, roomId, roomFetchAction));
          setDeleteModal((prevState) => ({ ...prevState, isOpen: false }));
        },
      });
    } else {
      dispatch(setLoadingAction(true));
      dispatch(toggleDeskIsEnabled({ desksIds, isEnabled: !desk.isEnabled }, projectId, roomId, roomFetchAction));
    }
  };
  const getEmployeeProjectComponent = (employee: EmployeeForRoomDetailsDto) => (
    employee.projectsNames.length === 1 ?
      <p className={styles.employeeProject}>{employee.projectsNames[0]}</p> : (
        <EmployeeProjectsAccordion employee={employee} />
      )
  );
  return (
    <>
      <Table.Row className={styles.statistic}>
        <Table.Cell>
          <Statistic size="tiny" color="black">
            <Statistic.Value>{desk.number}</Statistic.Value>
          </Statistic>
        </Table.Cell>
        {isLoaderActive && (
          <Table.Cell textAlign="left">
            <Loader active inline />
          </Table.Cell>
        )}
        {!isLoaderActive && (
          <Table.Cell textAlign="left">
            {desk.isHotDesk && <span><b><i>{t('roomDetails.hotDesk')}</i></b></span>}
            {!desk.isEnabled && <span><b><i>{t('roomDetails.disabledDesk')}</i></b></span>}
            {!desk.isHotDesk && desk.isEnabled && (
              <>
                <Grid columns="equal">
                  {(loggedUserData.isStandardUser() || editMode) && (
                    <>
                      {!desk.reservations.length && (
                        <Grid.Row>
                          <Grid.Column width={12}>
                            <p className={styles.emptyDesk}>{t('roomDetails.emptyDesk')}</p>
                          </Grid.Column>
                        </Grid.Row>
                      )}
                      {!!desk.reservations.length && desk.reservations.map((dr) => (
                        <Grid.Row key={dr.id} verticalAlign="middle">
                          <Grid.Column width={14}>
                            <p>
                              {`${dr.employee.name} ${dr.employee.surname}`}
                            </p>
                            {getEmployeeProjectComponent(dr.employee)}
                          </Grid.Column>
                          {!editMode && (
                            <Grid.Column width={1}>
                              <WeekPopup
                                disabled={false}
                                selectedDays={getReservedWeekdaysByEmployee(desk.reservations, dr.employee.id)}
                                disabledDays={getReservedWeekdaysByOtherEmployees(desk.reservations, dr.employee.id)}
                                onCloseAction={() => { }}
                              />
                            </Grid.Column>
                          )}
                        </Grid.Row>
                      ))}
                    </>
                  )}
                  {!loggedUserData.isStandardUser() && !editMode && (
                    <>
                      {desk.reservations.map((dr) => (
                        <Grid.Row key={dr.id} verticalAlign="middle">
                          <Grid.Column>
                            <p>
                              {`${dr.employee.name} ${dr.employee.surname}`}
                            </p>
                            {getEmployeeProjectComponent(dr.employee)}
                          </Grid.Column>
                          <Grid.Column width={5} textAlign="right" className={styles.buttons}>
                            <WeekPopup
                              disabled={false}
                              selectedDays={getReservedWeekdaysByEmployee(desk.reservations, dr.employee.id)}
                              disabledDays={getReservedWeekdaysByOtherEmployees(desk.reservations, dr.employee.id)}
                              onCloseAction={(scheduledWeekdays) => {
                                setCalendarOnClose(getEmployeeReservationId(desk.reservations, dr.employee.id), scheduledWeekdays);
                              }}
                            />
                            <Button
                              icon="user delete"
                              color="red"
                              basic
                              circular
                              size="tiny"
                              title={t('roomDetails.removeUser')}
                              onClick={() => removeEmployeeHandler(dr)}
                            />
                          </Grid.Column>
                        </Grid.Row>
                      ))}
                      {!!projectId && (!desk.reservations.length || !!getFreeWeekdays(desk.reservations).length) && (
                        <Grid.Row>
                          <Grid.Column width={11}>
                            <EmployeesDropdown
                              selectedEmployee={selectedEmployeeId}
                              employeeOptions={employeeOptions}
                              onEmployeeChange={(event, data) => selectEmployeeHandler(event, data, null, dispatchReserveDeskAction)}
                              disabled={arrIsNullOrEmpty(employeeOptions)}
                              styles={styles.employeesDropdown}
                            />
                          </Grid.Column>
                        </Grid.Row>
                      )}
                      {!projectId && !desk.reservations.length && (
                        <Grid.Row>
                          <Grid.Column width={12}>
                            <p className={styles.emptyDesk}>{t('roomDetails.emptyDesk')}</p>
                          </Grid.Column>
                        </Grid.Row>
                      )}
                    </>
                  )}
                </Grid>
              </>
            )}
          </Table.Cell>
        )}
        {!editMode ? (
          <>
            {!loggedUserData.isStandardUser() && (
              <Table.Cell>
                <DeskHistoryPeek
                  deskHistory={desk.deskHistory}
                  deskNumber={desk.number}
                  employeeDropdown={!desk.isHotDesk && desk.reservations.length > 0 && !!projectId &&
                    (!desk.reservations.length || !!getFreeWeekdays(desk.reservations).length)}
                />
              </Table.Cell>
            )}
            {loggedUserData.isStandardUser() && (
              <Table.Cell />
            )}
          </>
        ) : (
          <>
            <Table.Cell>
              <Icon
                color="blue"
                className={styles.toggle}
                name={desk.isHotDesk ? 'toggle on' : 'toggle off'}
                size="big"
                disabled={!desk.isEnabled}
                onClick={setHotDeskToggleHandler}
              />
            </Table.Cell>
            <Table.Cell>
              <Icon
                color={desk.isEnabled ? 'green' : 'red'}
                className={styles.toggle}
                name={desk.isEnabled ? 'toggle off' : 'toggle on'}
                size="big"
                onClick={toggleDeskIsEnabledHandler}
              />
            </Table.Cell>
            {loggedUserData.isUserAdmin() === true && (
              <Table.Cell>
                <Button
                  icon="trash"
                  color="red"
                  basic
                  circular
                  size="tiny"
                  onClick={removeDeskHandler}
                />
              </Table.Cell>
            )}
          </>
        )}
      </Table.Row>
      {deleteModal.body ? (
        <ModalWithContent
          isOpen={deleteModal.isOpen}
          title={deleteModal.message}
          body={deleteModal.body}
          closeFunction={() => {
            setDeleteModal((prevState) => ({ ...prevState, isOpen: false }));
            setIsLoaderActive(false);
          }}
          yesOkFunction={() => {
            deleteModal.yesFunction();
          }}
          isOkBtnOnly={false}
        />
      ) : (
        <SimpleModal
          isOpen={deleteModal.isOpen}
          text={deleteModal.message}
          closeFunction={() => {
            setDeleteModal((prevState) => ({ ...prevState, isOpen: false }));
            setIsLoaderActive(false);
          }}
          yesOkFunction={() => {
            deleteModal.yesFunction();
          }}
          isOkBtnOnly={false}
        />
      )}
    </>
  );
};

export default DeskRow;
