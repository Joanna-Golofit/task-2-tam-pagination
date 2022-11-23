import React, { useEffect, useState } from 'react';
import dayjs from 'dayjs';
import i18next from 'i18next';
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { Button, Modal, Table } from 'semantic-ui-react';
import { AppState } from '../../../store';
import { setLoadingAction } from '../../../store/global/actions';
import {
  closeReservationModalAction,
  reserveHotdesk,
  getActiveReservationsForDeskAction,
  getActiveReservationsForEmployeeAction,
  removeHotDeskReservationAction,
} from '../../../store/hotDeskDetails/actions';
import styles from './reservationModal.module.scss';
import EmployeesSearch from './employeesSearch';
import Datepicker from '../../../components/Datepicker';
import { stringFormat } from '../../../helpers/helpers';

type ComponentProps = {
  roomId: string;
};

const ReservationModal: React.FC<ComponentProps> = ({ roomId }) => {
  const { t } = useTranslation();
  const dispatch = useDispatch();

  const { loggedUserData } = useSelector((state: AppState) => state.global);
  const { isOpen, deskId, deskNo, deskReservations, employeeReservations } = useSelector((state: AppState) => state.hotDeskDetails);
  const employeeDeskReservations = employeeReservations.filter((er) => er.desk.id === deskId);

  const [reservationRange, setReservationRange] = useState([] as Date[]);
  const [employeeId, setEmployeeId] = useState('' as string);

  useEffect(() => {
    if (!deskId || !isOpen) {
      return;
    }

    if (canReserveForOthers()) {
      setEmployeeId('');
    } else {
      setEmployeeId(loggedUserData.id);
    }

    setReservationRange([]);
    dispatch(setLoadingAction(true));
    dispatch(getActiveReservationsForDeskAction(deskId));
  }, [isOpen]);

  useEffect(() => {
    if (employeeId) {
      dispatch(setLoadingAction(true));
      dispatch(getActiveReservationsForEmployeeAction(employeeId));
    }
  }, [employeeId, isOpen]);

  const canReserveForOthers = () => loggedUserData.isUserAdmin() || loggedUserData.isTeamLeader();

  const reserveHotdeskHandler = () => {
    dispatch(setLoadingAction(true));
    dispatch(reserveHotdesk({
      reservingEmployee: employeeId,
      reservationStart: reservationRange[0],
      reservationEnd: reservationRange[1] || reservationRange[0],
      deskId: deskId!,
    }, roomId));
  };

  const removeReservation = (reservationId: string) => {
    dispatch(setLoadingAction(true));
    dispatch(removeHotDeskReservationAction(reservationId, roomId));
  };

  const onClose = () => {
    dispatch(closeReservationModalAction());
  };

  const getMaxReservationDate = () => {
    if (canReserveForOthers()) {
      return dayjs().add(30, 'days').toDate();
    }

    return dayjs().add(7, 'days').toDate();
  };

  const dateIsWithinAnyDeskReservation = (date: Date) =>
    deskReservations.some((dr) => date >= dayjs(dr.reservationStart).toDate() && date <= dayjs(dr.reservationEnd).toDate());

  const dateIsWithinAnyEmployeeReservation = (date: Date) =>
    employeeReservations.some((er) => date >= dayjs(er.reservationStart).toDate() && date <= dayjs(er.reservationEnd).toDate());

  const disableReservedDates = (date: Date) =>
    !(dateIsWithinAnyDeskReservation(date) || dateIsWithinAnyEmployeeReservation(date));

  const formatReservationDate = (date: Date) =>
    dayjs(date).toDate().toLocaleString(i18next.language, { weekday: 'short', month: 'long', day: 'numeric', year: 'numeric' });

  return (
    <>
      <Modal open={isOpen} onClose={onClose} size="tiny">
        <Modal.Header>
          {stringFormat(t('hotDesks.reservationHeader'), deskNo?.toString() || '')}
        </Modal.Header>
        <Modal.Content>
          <Table color="brown" unstackable>
            <Table.Body>
              {canReserveForOthers() && (
                <Table.Row>
                  <Table.Cell>
                    {t('hotDesks.selectEmployeeForReservation')}
                  </Table.Cell>
                  <Table.Cell>
                    <EmployeesSearch employeeSelectedCallback={setEmployeeId} />
                  </Table.Cell>
                </Table.Row>
              )}
              <Table.Row>
                <Table.Cell>
                  {t('hotDesks.reservationDate')}
                </Table.Cell>
                <Table.Cell className={styles.pickerHolder}>
                  <Datepicker
                    name="reservationDate"
                    value={reservationRange}
                    onChange={(_, data) => setReservationRange(data.value as Date[])}
                    type="range"
                    clearable
                    clearOnSameDateClick={false}
                    datePickerOnly
                    minDate={dayjs().subtract(1, 'd').toDate()}
                    maxDate={getMaxReservationDate()}
                    filterDate={disableReservedDates}
                    placeholder="YYYY-MM-DD - YYYY-MM-DD"
                  />
                </Table.Cell>
              </Table.Row>
            </Table.Body>
          </Table>
          <Table color="brown" unstackable>
            <Table.Header>
              <Table.Row>
                <Table.HeaderCell>{t('hotDesks.activeReservations')}</Table.HeaderCell>
                <Table.HeaderCell width={1} />
              </Table.Row>
            </Table.Header>
            <Table.Body>
              {(!employeeId && canReserveForOthers()) && (
                <Table.Row>
                  <Table.Cell textAlign="center" colSpan="5">
                    {t('common.noResultsFilters')}
                  </Table.Cell>
                </Table.Row>
              )}
              {(!!employeeId && !employeeDeskReservations.length) && (
                <Table.Row>
                  <Table.Cell textAlign="center" colSpan="5">
                    {t('common.noResultsFilters')}
                  </Table.Cell>
                </Table.Row>
              )}
              {(!!employeeId && !!employeeDeskReservations.length) && employeeDeskReservations.map((reservation) => (
                <Table.Row key={reservation.id}>
                  <Table.Cell className={styles.date}>
                    {formatReservationDate(reservation.reservationStart)}
                    {
                      !dayjs(reservation.reservationStart).isSame(dayjs(reservation.reservationEnd), 'day') &&
                      ` - ${formatReservationDate(reservation.reservationEnd)}`
                    }
                  </Table.Cell>
                  <Table.Cell className={styles.actionButtons} onClick={null}>
                    <Button
                      icon="trash"
                      color="red"
                      basic
                      circular
                      size="tiny"
                      onClick={() => removeReservation(reservation.id)}
                    />
                  </Table.Cell>
                </Table.Row>
              ))}
            </Table.Body>
          </Table>
        </Modal.Content>
        <Modal.Actions>
          <Button
            className={styles.btn}
            size="tiny"
            disabled={!reservationRange || !reservationRange.length || !employeeId}
            onClick={reserveHotdeskHandler}
          >
            {t('hotDesks.reservationBtn')}
          </Button>
        </Modal.Actions>
      </Modal>
    </>
  );
};

export default ReservationModal;
