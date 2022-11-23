import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Table, TableCell, Button, Responsive, Modal }
  from 'semantic-ui-react';
import { useDispatch, useSelector } from 'react-redux';
import { LAPTOP_MEDIA_WIDTH } from '../../../globalConstants';
import HotDeskRow from './hotDesk';
import styles from './deskSection.module.scss';
import { AppState } from '../../../store';
import RegularDesk from './regularDesk';
import ReservationModal from './reservationModal';
import { openReservationModalAction } from '../../../store/hotDeskDetails/actions';
import RoomPlan from '../../../components/RoomPlan';

const DesksSection: React.FC = () => {
  const { t } = useTranslation();
  const dispatch = useDispatch();

  const roomStore = useSelector((state: AppState) => state.room);

  const { item } = roomStore;

  const [isPlanOpen, setIsPlanOpen] = useState(false);

  const openCalendar = (deskId: string, deskNo: number) => {
    dispatch(openReservationModalAction(deskId, deskNo));
  };

  const tableBody = (
    item.desksInRoom.length !== 0 ?
      item.desksInRoom.map((d) => (d.isHotDesk ?
        <HotDeskRow key={d.id} deskNumber={d.number} openCalendar={() => openCalendar(d.id, d.number)} /> :
        (
          <RegularDesk key={d.id} deskNumber={d.number} />
        ))) :
      (
        <Table.Row textAlign="center" colSpan="3">
          <TableCell colSpan="4">
            {t('roomDetails.noRecords')}
          </TableCell>
        </Table.Row>
      )
  );

  const roomPlan = () => (
    <div>
      <h3 className={styles.roomPlanHeader}>{t('roomDetails.roomPlan')}</h3>
      <RoomPlan roomItem={item} />
    </div>
  );

  return (
    <>
      <Table color="orange" stackable striped>
        <Table.Header />
        <Table.Body>
          <Table.Row>
            <Table.Cell colSpan={3} className={`${styles.desksRoomPlanCell} tableBackgroundExternalDark`}>
              <div className={styles.wrapper}>
                <div className={styles.desks}>
                  <h3>{t('hotDeskDetails.reserveHotDesk')}</h3>

                  <Responsive maxWidth={LAPTOP_MEDIA_WIDTH}>
                    <Button
                      circular
                      color="blue"
                      basic
                      icon="map outline"
                      onClick={() => setIsPlanOpen(true)}
                    />
                  </Responsive>
                  <Table striped basic="very" id={styles.table} className="inner-table">
                    <Table.Body>
                      <Table.Row className={styles.header}>
                        <Table.HeaderCell width={2}>{t('projectDetails.deskNumber')}</Table.HeaderCell>
                        <Table.HeaderCell colSpan={4} />
                      </Table.Row>
                      {tableBody}
                    </Table.Body>
                  </Table>
                </div>

                <Responsive minWidth={LAPTOP_MEDIA_WIDTH} className={styles.roomPlan}>
                  {roomPlan()}
                </Responsive>
              </div>
            </Table.Cell>
          </Table.Row>
        </Table.Body>
      </Table>

      <Modal open={isPlanOpen}>
        <Button
          icon="close"
          size="massive"
          onClick={() => setIsPlanOpen(false)}
          className={styles.button}
        />
        {roomPlan()}
      </Modal>

      <ReservationModal roomId={item.id} />
    </>
  );
};

export default DesksSection;
