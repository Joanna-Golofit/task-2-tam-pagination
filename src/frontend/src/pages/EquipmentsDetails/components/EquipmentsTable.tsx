import dayjs from 'dayjs';
import i18next from 'i18next';
import React from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { NavLink } from 'react-router-dom';
import { Table, Form, Responsive } from 'semantic-ui-react';
import { LAPTOP_MEDIA_WIDTH } from '../../../globalConstants';
import { Routes } from '../../../Routes';
import { EquipmentReservationHistoryDto } from '../../../services/equipments/models';
import { AppState } from '../../../store';

const EquipmentsTable: React.FC = () => {
  const { t } = useTranslation();
  const externalCompanyDetails = useSelector((state: AppState) => state.equipmentDetails);
  const { equipmentDetailsResponse: equipment } = externalCompanyDetails;
  const { employees: reservations } = equipment;
  const { reservationsHistory } = externalCompanyDetails.equipmentDetailsResponse;

  const formatReservationDate = (date: Date) =>
    dayjs(date).toDate().toLocaleString(i18next.language, { weekday: 'short', month: 'long', day: 'numeric', year: 'numeric' });

  const rows = (
    reservationsHistory?.length !== 0 && reservationsHistory ?
      reservationsHistory.filter((history: EquipmentReservationHistoryDto) => !history.reservationEnd)
        .map((({ employeeId, employeeName, employeeSurname, reservationStart, reservationEnd, count }) => (
          <Table.Row key={employeeId}>
            <Table.Cell width={7}>
              <NavLink to={`${Routes.Users}/${employeeId}`} exact><b>{`${employeeName} ${employeeSurname}`}</b></NavLink>
            </Table.Cell>
            <Table.Cell width={7}>
              {formatReservationDate(reservationStart)}
              { !dayjs(reservationStart).isSame(dayjs(reservationEnd), 'day') }
            </Table.Cell>
            <Table.Cell>
              {count}
            </Table.Cell>
          </Table.Row>
        ))) :
      (
        <Table.Row>
          <Table.Cell colSpan={4} textAlign="center"><i>{t('common.noResultsFilters')}</i></Table.Cell>
        </Table.Row>
      )
  );

  const employeeHistoryRows = (
    reservationsHistory?.length !== 0 && reservationsHistory ?
      reservationsHistory.filter((history: EquipmentReservationHistoryDto) => history.reservationEnd !== null)
        .map((({ employeeId, employeeName, employeeSurname, reservationStart, reservationEnd, count }) => (
          <Table.Row key={employeeId}>
            <Table.Cell width={7}>
              <NavLink to={`${Routes.Users}/${employeeId}`} exact><b>{`${employeeName} ${employeeSurname}`}</b></NavLink>
            </Table.Cell>
            <Table.Cell width={7}>
              {formatReservationDate(reservationStart)}
              { !dayjs(reservationStart).isSame(dayjs(reservationEnd), 'day') && ` - ${formatReservationDate(reservationEnd)}` }
            </Table.Cell>
            <Table.Cell>
              {count}
            </Table.Cell>
          </Table.Row>
        ))) :
      (
        <Table.Row>
          <Table.Cell colSpan={4} textAlign="center"><i>{t('common.noResultsFilters')}</i></Table.Cell>
        </Table.Row>
      )
  );
  return (
    <>
      <Form>
        <Table compact color="orange" unstackable>
          <Table.Header>
            <Responsive as={Table.Row} minWidth={LAPTOP_MEDIA_WIDTH}>
              <Table.HeaderCell width={7}>{t('equipmentDetails.employee')}</Table.HeaderCell>
              <Table.HeaderCell width={7}>{t('equipmentDetails.from')}</Table.HeaderCell>
              <Table.HeaderCell>{t('equipmentDetails.count')}</Table.HeaderCell>

            </Responsive>
            <Responsive as={Table.Row} maxWidth={LAPTOP_MEDIA_WIDTH - 1}>
              <Table.HeaderCell>{t('equipmentDetails.employee')}</Table.HeaderCell>
              <Table.HeaderCell>{t('equipmentDetails.from')}</Table.HeaderCell>
              <Table.HeaderCell>{t('equipmentDetails.count')}</Table.HeaderCell>
            </Responsive>
          </Table.Header>
          <Table.Body>
            {(reservations?.length || 0) === 0 ? (
              <Table.Row>
                <Table.Cell colSpan={4} textAlign="center"><i>{t('common.noResultsFilters')}</i></Table.Cell>
              </Table.Row>
            ) : rows}
          </Table.Body>
        </Table>
        <Table compact color="orange" unstackable>
          <Table.Header>
            <Responsive as={Table.Row} minWidth={LAPTOP_MEDIA_WIDTH}>
              <Table.HeaderCell width={7}>{t('equipmentDetails.historyHeader')}</Table.HeaderCell>
              <Table.HeaderCell width={7}>{t('equipmentDetails.timeHeader')}</Table.HeaderCell>
              <Table.HeaderCell>{t('equipmentDetails.count')}</Table.HeaderCell>
            </Responsive>
            <Responsive as={Table.Row} maxWidth={LAPTOP_MEDIA_WIDTH - 1}>
              <Table.HeaderCell>{t('equipmentDetails.historyHeader')}</Table.HeaderCell>
              <Table.HeaderCell>{t('equipmentDetails.timeHeader')}</Table.HeaderCell>
              <Table.HeaderCell>{t('equipmentDetails.count')}</Table.HeaderCell>
            </Responsive>
          </Table.Header>
          <Table.Body>
            {(reservationsHistory?.length || 0) === 0 ? (
              <Table.Row>
                <Table.Cell colSpan={4} textAlign="center"><i>{t('common.noResultsFilters')}</i></Table.Cell>
              </Table.Row>
            ) : employeeHistoryRows}
          </Table.Body>
        </Table>
      </Form>
    </>
  );
};

export default EquipmentsTable;
