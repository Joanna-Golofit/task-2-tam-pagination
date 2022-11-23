import React from 'react';
import { Table, TableCell } from 'semantic-ui-react';
import { useHistory } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import styles from './userTeamsSection.module.scss';
import { UserDetailsDto } from '../../../services/user/models';
import { Routes } from '../../../Routes';
import { AppState } from '../../../store';

type Props = {
  userDetails: UserDetailsDto;
};

const UserTeamsSection: React.FC<Props> = ({ userDetails }) => {
  const routerHistory = useHistory();
  const { t } = useTranslation();

  const { loggedUserData } = useSelector((state: AppState) => state.global);

  const navigateToProject = (id: string) => {
    routerHistory.push(`${Routes.Projects}/${id}`);
  };

  const rows = userDetails.projects.map((userProjectDto) => (loggedUserData.isStandardUser() ? (
    <Table.Row
      key={userProjectDto.id}
    >
      <Table.Cell>
        <span
          className={styles.projectName}
          aria-hidden="true"
        >{userProjectDto.name}
        </span>
      </Table.Cell>
      <Table.Cell>
        {userProjectDto.teamLeadersNames.length !== 0 ? userProjectDto.teamLeadersNames.join(', ') :
        <b><i>{t('common.noTeamleader')}</i></b>}
      </Table.Cell>
    </Table.Row>
  ) : (
    <Table.Row
      key={userProjectDto.id}
      className={styles.projectRow}
      onClick={() => navigateToProject(userProjectDto.id)}
    >
      <Table.Cell>
        <span
          className={styles.projectName}
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
  )));

  const tableBody = (() => (
    <Table.Body>
      {userDetails.projects.length ?
        (rows) :
        (
          <Table.Row>
            <TableCell textAlign="center" colSpan="2">
              {t('userDetails.noTeams')}
            </TableCell>
          </Table.Row>
        )}
    </Table.Body>
  ))();

  return (
    <Table color="orange" unstackable>
      <Table.Header>
        <Table.Row>
          <Table.HeaderCell>{t('userDetails.team')}</Table.HeaderCell>
          <Table.HeaderCell>{t('userDetails.leader')}</Table.HeaderCell>
        </Table.Row>
      </Table.Header>
      {tableBody}
    </Table>
  );
};

export default UserTeamsSection;
