import React from 'react';
import { Table, Form, Responsive, Button } from 'semantic-ui-react';
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { NavLink } from 'react-router-dom';
import { AppState } from '../../../store';
import { Routes } from '../../../Routes';
import { LAPTOP_MEDIA_WIDTH } from '../../../globalConstants';
import { setLoadingAction } from '../../../store/global/actions';
import { removeEmployeeFromProject } from '../../../store/projectDetails/actions';

const EmployeesTable: React.FC = () => {
  const { t } = useTranslation();
  const dispatch = useDispatch();

  const externalCompanyDetails = useSelector((state: AppState) => state.projectDetails);
  const { projectDetailsResponse: externalCompany } = externalCompanyDetails;
  const { employees } = externalCompany;

  const roomsDesks = externalCompany.rooms.flatMap((r) => r.desksInRoom.map((d) =>
    ({
      roomName: `${r.building.name}-${r.name}`,
      desk: d.number,
      deskId: d.id,
      employeesIds: d.reservations.map((dr) => dr.employee?.id),
    })));

  const handleRemove = (employeeId : string) => {
    dispatch(setLoadingAction(true));
    dispatch(removeEmployeeFromProject({
      employeeId,
      companyId: externalCompany.id,
    }));
  };

  const rows = employees.map(({ id, name, surname }) => {
    const employeeRoomDesk = roomsDesks?.filter((rd) => rd.employeesIds.includes(id));
    return (
      <Table.Row key={id}>
        <Table.Cell>
          <NavLink to={`${Routes.Users}/${id}`} exact><b>{`${name} ${surname}`}</b></NavLink>
        </Table.Cell>
        <Table.Cell>
          {employeeRoomDesk.length ? employeeRoomDesk.map((erd) => (
            <p key={`${erd?.roomName}_${erd?.desk}`}>
              {erd?.roomName} <span>{t('projectDetails.desk')} {erd?.desk}</span>
            </p>
          )) : '-'}
        </Table.Cell>
        <Table.Cell>
          <Button
            icon="trash"
            color="red"
            basic
            circular
            size="tiny"
            onClick={() => handleRemove(id)}
          />
        </Table.Cell>
      </Table.Row>
    );
  });

  return (
    <>
      <Form>
        <Table compact color="orange" unstackable>
          <Table.Header>
            <Responsive as={Table.Row} minWidth={LAPTOP_MEDIA_WIDTH}>
              <Table.HeaderCell>{t('projectDetails.teamMember')}</Table.HeaderCell>
              <Table.HeaderCell> {t('projectDetails.location')}</Table.HeaderCell>
              <Table.HeaderCell width={1} />
            </Responsive>
            <Responsive as={Table.Row} maxWidth={LAPTOP_MEDIA_WIDTH - 1}>
              <Table.HeaderCell>{t('projectDetails.teamMember')}</Table.HeaderCell>
              <Table.HeaderCell>{t('projectDetails.locationShort')}</Table.HeaderCell>
              <Table.HeaderCell width={1} />
            </Responsive>
          </Table.Header>
          <Table.Body>
            {(employees?.length || 0) === 0 ? (
              <Table.Row>
                <Table.Cell colSpan={4} textAlign="center"><i>{t('common.noResultsFilters')}</i></Table.Cell>
              </Table.Row>
            ) : rows}
          </Table.Body>
        </Table>
      </Form>
    </>
  );
};

export default EmployeesTable;
