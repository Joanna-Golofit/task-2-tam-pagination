import React, { useState } from 'react';
import { Table, Form, Radio, Checkbox, Responsive } from 'semantic-ui-react';
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { NavLink } from 'react-router-dom';
import { updateEmployeesWorkspaceTypesAction } from '../../../store/projectDetails/actions';
import SimpleModal from '../../../layouts/components/SimpleModal';
import { AppState } from '../../../store';
import styles from './usersTable.module.scss';
import { Routes } from '../../../Routes';
import { TABLET_MEDIA_WIDTH } from '../../../globalConstants';
import { setLoadingAction } from '../../../store/global/actions';
import { WorkspaceType } from '../../../services/user/enums';

type ComponentProps = {
};

interface employeeIdWorkspaceType {
  employeeId: string, workspaceType: number,
}

const UsersTable: React.FC<ComponentProps> = () => {
  const { t } = useTranslation();
  const dispatch = useDispatch();

  const [employeesIdsWorkspace, setEmployeesIdsWorkspace] =
    useState([] as employeeIdWorkspaceType[]);

  const [isOpenSingle, setIsOpenSingle] = useState(false);
  const [isOpenForAll, setIsOpenForAll] = useState(false);

  const projectDetails = useSelector((state: AppState) => state.projectDetails);
  const { projectDetailsResponse: project } = projectDetails;
  const { employees } = project;

  const allOffice = !employees.some((e) => (e.workmode !== WorkspaceType.Office));
  const allHybrid = !employees.some((e) => (e.workmode !== WorkspaceType.Hybrid));
  const allRemote = !employees.some((e) => (e.workmode !== WorkspaceType.Remote));

  const closeClickHandler = () => {
    setIsOpenSingle(false);
    setIsOpenForAll(false);
    setEmployeesIdsWorkspace([]);
  };

  const yesClickHandler = () => {
    dispatch(setLoadingAction(true));
    dispatch(updateEmployeesWorkspaceTypesAction(employeesIdsWorkspace, project.id));
    setEmployeesIdsWorkspace([]);
    setIsOpenSingle(false);
    setIsOpenForAll(false);
  };

  const selectRemoteHandler = (employeeId: string) => {
    const assignments = roomsDesks
      .filter((rd) => rd.employeesIds.includes(employeeId))
      .map((rd) => ({ deskId: rd.deskId, employeeId }));

    if (assignments.length === 0) {
      updateWorkmode(employeeId, WorkspaceType.Remote);
    } else {
      setEmployeesIdsWorkspace([{ employeeId, workspaceType: WorkspaceType.Remote }]);
      setIsOpenSingle(true);
    }
  };

  const selectRemoteForAllHandler = () => {
    if (!allRemote) {
      const assignments = roomsDesks
        .map((rd) => rd.employeesIds
          .filter((employeeId) => employees.some((e) => e.id === employeeId))
          .map((employeeId) => ({ deskId: rd.deskId, employeeId })))
        .flat();

      if (assignments.length === 0) {
        updateWorkmodeInBulk(WorkspaceType.Remote);
      } else {
        setEmployeesIdsWorkspace(employees.map((e) => ({ employeeId: e.id, workspaceType: WorkspaceType.Remote })));
        if (assignments.length > 1) {
          setIsOpenForAll(true);
        } else {
          setIsOpenSingle(true);
        }
      }
    }
  };

  const updateWorkmode = (employeeId: string, workspaceType: number) => {
    dispatch(setLoadingAction(true));
    dispatch(updateEmployeesWorkspaceTypesAction([{ employeeId, workspaceType }], project.id));
  };

  const updateWorkmodeInBulk = (workspaceType: number) => {
    dispatch(setLoadingAction(true));
    dispatch(updateEmployeesWorkspaceTypesAction(employees.map((e) => ({ employeeId: e.id, workspaceType })), project.id));
  };

  const roomsDesks = project.rooms.flatMap((r) => r.desksInRoom.map((d) => ({
    roomName: `${r.building.name}-${r.name}`,
    desk: d.number,
    deskId: d.id,
    employeesIds: d.reservations.map((dr) => dr.employee?.id),
  })));

  const rows = employees.map(({ id, name, surname, workmode }) => {
    const employeeDesks = roomsDesks?.filter((rd) => rd.employeesIds.includes(id));
    return (
      <Table.Row key={id}>
        <Table.Cell><NavLink to={`${Routes.Users}/${id}`} exact><b>{`${name} ${surname}`}</b></NavLink></Table.Cell>
        <Table.Cell textAlign="right">
          <Radio
            checked={workmode === WorkspaceType.Office}
            name={id}
            onChange={() => updateWorkmode(id, WorkspaceType.Office)}
          />
        </Table.Cell>
        <Table.Cell textAlign="right">
          <Radio
            checked={workmode === WorkspaceType.Hybrid}
            name={id}
            onChange={() => updateWorkmode(id, WorkspaceType.Hybrid)}
          />
        </Table.Cell>
        <Table.Cell textAlign="right">
          <Radio
            checked={workmode === WorkspaceType.Remote}
            name={id}
            onChange={() => selectRemoteHandler(id)}
          />
        </Table.Cell>
        <Table.Cell />
        <Table.Cell>
          {employeeDesks.length ? employeeDesks.map((erd) => (
            <p key={`${erd?.roomName}_${erd?.desk}`}>
              {erd?.roomName} <span className={styles.deskNumber}>{t('projectDetails.desk')} {erd?.desk}</span>
            </p>
          )) : '-'}
        </Table.Cell>
      </Table.Row>
    );
  });

  return (
    <>
      <Form>
        <Table color="orange" unstackable>
          <Table.Header>
            <Table.Row>
              <Table.HeaderCell width={3}>{t('projectDetails.teamMember')}</Table.HeaderCell>
              <Table.HeaderCell width={3} textAlign="right">
                <div className={styles.headerCell}>
                  {t('projectDetails.officeWork')}
                  <Checkbox
                    checked={allOffice}
                    onChange={() => {
                      if (!allOffice) {
                        updateWorkmodeInBulk(WorkspaceType.Office);
                      }
                    }}
                  />
                </div>
              </Table.HeaderCell>
              <Table.HeaderCell width={3} textAlign="right">
                <div className={styles.headerCell}>
                  <Responsive minWidth={TABLET_MEDIA_WIDTH}> {t('projectDetails.hybridWork')} </Responsive>
                  <Responsive maxWidth={TABLET_MEDIA_WIDTH - 1}> {t('projectDetails.hybridWorkShort')}</Responsive>
                  <Checkbox
                    checked={allHybrid}
                    onChange={() => {
                      if (!allHybrid) {
                        updateWorkmodeInBulk(WorkspaceType.Hybrid);
                      }
                    }}
                  />
                </div>
              </Table.HeaderCell>
              <Table.HeaderCell width={3} textAlign="right">
                <div className={styles.headerCell}>
                  <Responsive minWidth={TABLET_MEDIA_WIDTH}> {t('projectDetails.remoteWork')} </Responsive>
                  <Responsive maxWidth={TABLET_MEDIA_WIDTH - 1}> {t('projectDetails.remoteWorkShort')}</Responsive>
                  <Checkbox
                    checked={allRemote}
                    onChange={selectRemoteForAllHandler}
                  />
                </div>
              </Table.HeaderCell>
              <Table.HeaderCell />
              <Responsive as={Table.HeaderCell} minWidth={TABLET_MEDIA_WIDTH}>
                {t('projectDetails.location')}
              </Responsive>
              <Responsive as={Table.HeaderCell} maxWidth={TABLET_MEDIA_WIDTH - 1}>
                {t('projectDetails.locationShort')}
              </Responsive>
            </Table.Row>
          </Table.Header>
          <Table.Body>
            {rows}
          </Table.Body>
        </Table>
      </Form>
      <SimpleModal
        isOpen={isOpenSingle}
        text={t('projectDetails.setRemoteMessage')}
        closeFunction={closeClickHandler}
        yesOkFunction={yesClickHandler}
        isOkBtnOnly={false}
      />
      <SimpleModal
        isOpen={isOpenForAll}
        text={t('projectDetails.setRemoteForAllMessage')}
        closeFunction={closeClickHandler}
        yesOkFunction={yesClickHandler}
        isOkBtnOnly={false}
      />
    </>
  );
};

export default UsersTable;
