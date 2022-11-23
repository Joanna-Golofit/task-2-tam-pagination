import React from 'react';
import { Table, TableCell } from 'semantic-ui-react';
import { useHistory } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { useTranslation } from 'react-i18next';
import { UserProjectDto } from '../../../services/user/models';
import { Routes } from '../../../Routes';
import { AppState } from '../../../store';
import styles from './userProjects.module.scss';

type Props = {
  projects: UserProjectDto[];
};

const UserProjects: React.FC<Props> = ({ projects }) => {
  const routerHistory = useHistory();
  const { t } = useTranslation();
  const { loggedUserData } = useSelector((state: AppState) => state.global);

  const navigateToProject = (id: string) => {
    if (!loggedUserData.isStandardUser()) {
      routerHistory.push(`${Routes.Projects}/${id}`);
    }
  };

  const rows = (projects.length > 0 ?
    projects.map((userProjectDto) => (
      <Table.Row
        key={userProjectDto.id}
        className={loggedUserData.isStandardUser() ? '' : styles.clickable}
        onClick={() => navigateToProject(userProjectDto.id)}
      >
        <Table.Cell>
          <span
            role="link"
            tabIndex={0}
            aria-hidden="true"
          >{userProjectDto.name}
          </span>
        </Table.Cell>
        <Table.Cell>
          {userProjectDto.teamLeadersNames.length !== 0 ? userProjectDto.teamLeadersNames.join(', ') :
          <b><i>{t('common.noTeamleader')}</i></b>}
        </Table.Cell>
      </Table.Row>
    )) :
    (
      <Table.Row>
        <TableCell textAlign="center" colSpan="2">
          {t('userDashboard.noTeams')}
        </TableCell>
      </Table.Row>
    )
  );

  const tableBody = (() => (
    <Table.Body>
      {rows}
    </Table.Body>
  ))();

  return (
    <Table color="orange" unstackable>
      <Table.Header>
        <Table.Row>
          <Table.HeaderCell>{t('userDashboard.team')}</Table.HeaderCell>
          <Table.HeaderCell>{t('userDashboard.leader')}</Table.HeaderCell>
        </Table.Row>
      </Table.Header>
      {tableBody}
    </Table>
  );
};

export default UserProjects;
