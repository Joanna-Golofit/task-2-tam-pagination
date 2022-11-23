import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import { useTranslation } from 'react-i18next';
import { Action } from 'redux';
import { Button, Modal, Responsive, Table } from 'semantic-ui-react';
import styles from './RoomView.module.scss';
import { LAPTOP_MEDIA_WIDTH } from '../../globalConstants';
import RoomPlanSection from './RoomPlanSection';
import { AppState } from '../../store';
import { DeskForRoomDetailsDto, EmployeeForRoomDetailsDto, RoomDetailsDto } from '../../services/room/models';
import { arrIsNullOrEmpty } from '../../helpers/helpers';
import DeskRow from './DeskRow';
import { EmployeeForProjectDetailsDto, RoomForProjectDto } from '../../services/project/models';

type Props = {
  desks: DeskForRoomDetailsDto[];
  editMode: boolean;
  employees: (EmployeeForRoomDetailsDto | EmployeeForProjectDetailsDto)[];
  projectId: string;
  room: RoomDetailsDto | RoomForProjectDto;
  roomFetchAction: Action<any>;
  roomId: string;
}

const RoomView: React.FC<Props> = ({
  desks, editMode, employees, projectId,
  room, roomId, roomFetchAction,
}) => {
  const { t } = useTranslation();
  const { loggedUserData } = useSelector((state: AppState) => state.global);
  const [isPlanOpen, setIsPlanOpen] = useState(false);

  return (
    <>
      <div className={`${styles.wrapper} tableBackgroundExternalDark`}>
        <div className={styles.desks}>
          <h3 className={styles.assignDeskText}>{t('projectDetails.assignToDesks')}</h3>
          {/* Show room plan button that opens modal with room plan if screen size smaller than laptop*/}
          <Responsive maxWidth={LAPTOP_MEDIA_WIDTH}>
            <Button
              circular
              color="blue"
              basic
              icon="map outline"
              onClick={() => setIsPlanOpen(true)}
            />
          </Responsive>
          <Table striped basic="very" id={styles.table}>
            <Table.Body>
              <Table.Row className={styles.header}>
                <Table.HeaderCell width={2}>{t('projectDetails.deskNumber')}</Table.HeaderCell>
                <Table.HeaderCell
                  colSpan={editMode ? 1 : 4}
                >{t('common.employee')}
                </Table.HeaderCell>
                {editMode && (
                <>
                  <Table.HeaderCell width={1}>{t('roomDetails.hotDesk')}</Table.HeaderCell>
                  <Table.HeaderCell width={1}>{t('roomDetails.blocked')}</Table.HeaderCell>
                  {loggedUserData.isUserAdmin() === true && (
                    <Table.HeaderCell width={1}>{t('roomDetails.removeDesk')}</Table.HeaderCell>
                  )}
                </>
                )}
              </Table.Row>
              {
                !arrIsNullOrEmpty(desks) ?
                  desks.map((desk) => (
                    <DeskRow
                      key={desk.id}
                      desk={desk}
                      disabledEmployees={desks.map((d) => d.reservations.map((r) => r.employee?.id) as string[]).flat() as string[]}
                      editMode={editMode}
                      employees={employees}
                      projectId={projectId}
                      roomId={roomId}
                      roomFetchAction={roomFetchAction}
                    />
                  )) :
                  (
                    <Table.Row textAlign="center" className={styles.noRecordsText} colSpan="3">
                      <Table.Cell colSpan="4">
                        {t('roomDetails.noRecords')}
                      </Table.Cell>
                    </Table.Row>
                  )
              }
            </Table.Body>
          </Table>
        </div>
        {/* Show room plan if screen size bigger than laptop */}
        <Responsive minWidth={LAPTOP_MEDIA_WIDTH} className={styles.roomPlan}>
          <RoomPlanSection roomItem={room} roomToolsVisible={editMode} />
        </Responsive>
      </div>
      <Modal open={isPlanOpen}>
        <Button
          icon="close"
          size="massive"
          onClick={() => setIsPlanOpen(false)}
          className={styles.button}
        />
        <RoomPlanSection roomItem={room} roomToolsVisible={editMode} />
      </Modal>
    </>
  );
};

export default RoomView;
