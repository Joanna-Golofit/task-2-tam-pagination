import React, { useState } from 'react';
import { Button, Table, TableCell } from 'semantic-ui-react';
import { useHistory } from 'react-router-dom';
import i18next from 'i18next';
import { useTranslation } from 'react-i18next';
import { useDispatch } from 'react-redux';
import dayjs from 'dayjs';
import { Routes } from '../../../Routes';
import styles from './userReservations.module.scss';
import SimpleModal from '../../../layouts/components/SimpleModal';
import { ReservationInfoDto } from '../../../services/user/models';
import { removeHotDeskReservationAction } from '../../../store/hotDeskDetails/actions';
import { stringFormat } from '../../../helpers/helpers';
import { setLoadingAction } from '../../../store/global/actions';

type ComponentProps = {
  reservationInfo: ReservationInfoDto[];
  email: string;
  forTeamLeader: boolean;
};

const UserReservation: React.FC<ComponentProps> = ({ reservationInfo, forTeamLeader = false }) => {
  const routerHistory = useHistory();
  const { t } = useTranslation();
  const [deleteModal, setDeleteModal] = useState({ isOpen: false, reservationId: '', yesFunction: () => {}, message: '' });
  const dispatch = useDispatch();

  const navigateToHotDeskDetails = (id: string) => {
    routerHistory.push(`${Routes.HotDesks}/${id}`);
  };
  const navigateToHotDesk = () => {
    routerHistory.push(`${Routes.HotDesks}`);
  };

  const formatReservationDate = (date: Date) =>
    dayjs(date).toDate().toLocaleString(i18next.language, { weekday: 'short', month: 'long', day: 'numeric', year: 'numeric' });

  const cancelReservationHandler = (reservation: ReservationInfoDto) => {
    const dateAsString = new Date(dayjs(reservation.reservationStart).toDate()).toLocaleString(i18next.language,
      { month: 'long', day: 'numeric', year: 'numeric' }) as string;
    setDeleteModal({
      isOpen: true,
      message: stringFormat(t('userDashboard.removeReservationModal'), dateAsString, `${reservation.desk.locationName}`, `${reservation.desk.deskNumber}`),
      reservationId: reservation.id,
      yesFunction: () => {
        dispatch(setLoadingAction(true));
        dispatch(removeHotDeskReservationAction(reservation.id, reservation.desk.roomId));
        setDeleteModal((prevState) => ({ ...prevState, isOpen: false }));
      } });
  };

  const rows = reservationInfo.map((reservation) => (
    <Table.Row key={reservation.id}>
      <Table.Cell width={5} className={styles.date}>
        {formatReservationDate(reservation.reservationStart)}
        {
          !dayjs(reservation.reservationStart).isSame(dayjs(reservation.reservationEnd), 'day') &&
            ` - ${formatReservationDate(reservation.reservationEnd)}`
                    }
      </Table.Cell>
      <Table.Cell width={3}> {forTeamLeader && reservation.employeeName} </Table.Cell>
      <Table.Cell width={2}> {reservation.desk.locationName} </Table.Cell>
      <Table.Cell width={2}> {reservation.desk.deskNumber} </Table.Cell>
      <Table.Cell className={styles.actionButtons} onClick={null}>
        <Button
          icon="trash"
          color="red"
          basic
          circular
          size="tiny"
          loading={deleteModal.isOpen && deleteModal.reservationId === reservation.id}
          onClick={() => cancelReservationHandler(reservation)}
        />
        <Button
          icon="arrow right"
          color="blue"
          basic
          circular
          size="tiny"
          onClick={() => navigateToHotDeskDetails(reservation.desk.roomId)}
        />
      </Table.Cell>
    </Table.Row>
  ));

  const tableBody = (() => (
    <Table.Body>
      {reservationInfo.length ?
        (rows) :
        (
          <Table.Row>
            <TableCell textAlign="center" colSpan="5">
              {t('userDashboard.noReservation')}
            </TableCell>
          </Table.Row>
        )}
    </Table.Body>
  ))();

  return (
    <>
      <div className={styles.reservationHeader}>
        <h3>{forTeamLeader ? t('userDashboard.reservationHeaderTeamLeader') : t('userDashboard.reservationHeader')}</h3>
        {!forTeamLeader && (<Button primary onClick={navigateToHotDesk}>{t('userDashboard.reservationBtn')}</Button>)}
      </div>
      <Table color="orange" unstackable>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell width={5}>{t('userDashboard.date')}</Table.HeaderCell>
            <Table.HeaderCell width={3}>{forTeamLeader && t('userDashboard.employeeName')}</Table.HeaderCell>
            <Table.HeaderCell width={2}>{t('userDashboard.location')}</Table.HeaderCell>
            <Table.HeaderCell width={2}>{t('userDashboard.deskNumber')}</Table.HeaderCell>
            <Table.HeaderCell width={2} />
          </Table.Row>
        </Table.Header>
        {tableBody}
      </Table>
      <SimpleModal
        isOpen={deleteModal.isOpen}
        text={deleteModal.message}
        closeFunction={() => {
          setDeleteModal((prevState) => ({ ...prevState, isOpen: false }));
        }}
        yesOkFunction={() => {
          deleteModal.yesFunction();
        }}
        isOkBtnOnly={false}
      />
    </>
  );
};

export default UserReservation;
