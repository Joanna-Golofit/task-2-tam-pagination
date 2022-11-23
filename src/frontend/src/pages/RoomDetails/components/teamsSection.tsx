import React from 'react';
import { useTranslation } from 'react-i18next';
import { Table, Button, TableCell, Icon } from 'semantic-ui-react';
import { useHistory } from 'react-router-dom';
import { useSelector, useDispatch } from 'react-redux';
import { Routes } from '../../../Routes';
import stylesRoomDetails from '../roomDetails.module.scss';
import { ProjectForRoomDetailsDto } from '../../../services/room/models';
import { AppState } from '../../../store';
import { openModalAction, removeTeamFromRoomAction } from '../../../store/roomDetails/actions';
import styles from './teamSection.module.scss';
import { setLoadingAction } from '../../../store/global/actions';
import { stringFormat } from '../../../helpers/helpers';

type Props = {
  teams: ProjectForRoomDetailsDto[];
}

const TeamsSection: React.FC<Props> = ({ teams }) => {
  const { t } = useTranslation();
  const routerHistory = useHistory();
  const roomStore = useSelector((state: AppState) => state.room);
  const dispatch = useDispatch();
  const room = roomStore.item;
  const { loggedUserData } = useSelector((state: AppState) => state.global);

  const navigateToProject = (id: string) => {
    if (!loggedUserData.isStandardUser()) {
      routerHistory.push(`${Routes.Projects}/${id}`);
    }
  };

  function handleClickReleaseButton(name: string, projectId: string): void {
    dispatch(openModalAction(stringFormat(t('roomDetails.areYouSureToMoveOutTeam'), `${name}`),
      () => {
        dispatch(setLoadingAction(true));
        dispatch(removeTeamFromRoomAction(room.id, projectId));
      }));
  }

  const rows = teams.map(({ id, name, teamLeaders }) => (
    <Table.Row key={id}>
      <Table.Cell className={styles.projectName}>
        {!loggedUserData.isStandardUser() ? (
          /* eslint-disable-next-line jsx-a11y/click-events-have-key-events */
          <span
            className={styles.clickable}
            onClick={() => navigateToProject(id)}
            role="link"
            tabIndex={0}
          >
            {name}
          </span>
        ) : (
          <span>
            {name}
          </span>
        )}
      </Table.Cell>
      <Table.Cell>
        {teamLeaders.length !== 0 ? (
          teamLeaders.map((tl) => (
            <a key={tl.id} href={`mailto:${tl?.email}`}>
              <div>
                <Icon color="blue" name="mail" />
                {`${tl?.name} ${tl?.surname} `}
              </div>
            </a>
          ))) :
          <b><i>{t('common.noTeamleader')}</i></b> }
      </Table.Cell>
      {!loggedUserData.isStandardUser() && (
        <Table.Cell textAlign="center">
          <Button
            color="red"
            basic
            circular
            size="mini"
            icon="user delete"
            className={stylesRoomDetails.leftButton}
            onClick={() => handleClickReleaseButton(name, id)}
          />
        </Table.Cell>
      )}
    </Table.Row>
  ));

  const tableBody = (() => (
    <Table.Body>
      {rows.length ?
        (rows) :
        (
          <Table.Row>
            <TableCell textAlign="center" className={stylesRoomDetails.noRecordsText} colSpan="3">
              {t('roomDetails.noRecords')}
            </TableCell>
          </Table.Row>
        )}
    </Table.Body>
  ))();

  return (
    <>
      <Table color="orange" unstackable>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell>{t('roomDetails.team')}</Table.HeaderCell>
            <Table.HeaderCell>{t('roomDetails.leader')}</Table.HeaderCell>
            {!loggedUserData.isStandardUser() && (<Table.HeaderCell textAlign="center">{t('roomDetails.removeTeamFromRoom')}</Table.HeaderCell>)}
          </Table.Row>
        </Table.Header>
        {tableBody}
      </Table>
    </>
  );
};

export default TeamsSection;
