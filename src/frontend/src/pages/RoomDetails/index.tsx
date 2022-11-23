import React, { useEffect } from 'react';
import { Segment, Header, GridRow, GridColumn, Grid } from 'semantic-ui-react';
import { useSelector, useDispatch } from 'react-redux';
import { useTranslation } from 'react-i18next';
import { AppState } from '../../store';
import { clearRoomDetails, closeModalAction, fetchRoom } from '../../store/roomDetails/actions';
import DesksSection from './components/desksSection';
import TeamsSection from './components/teamsSection';
import RoomInfo from './components/roomInfo';
import SimpleModal from '../../layouts/components/SimpleModal';
import { setLoadingAction } from '../../store/global/actions';
import styles from './roomDetails.module.scss';
import OutlookBookingMessage from './components/OutlookBookingMessage';

interface Props {
  id: string;
}

const Room: React.FC<Props> = (props) => {
  const { t } = useTranslation();
  const { id } = props;
  const dispatch = useDispatch();

  const { modalState, item: room, projectEmployees } = useSelector((state: AppState) => state.room);
  const roomsToBookInOutlook = ['F4-204', 'F4-209', 'F4C-225'];
  const currentRoom = `${room.building.name}-${room.name}`;
  const bookThisRoomInOutlook = roomsToBookInOutlook.includes(currentRoom);

  useEffect(() => {
    dispatch(setLoadingAction(true));
    dispatch(fetchRoom(id));
  }, [id]);

  useEffect(() => () => {
    dispatch(clearRoomDetails());
  }, []);

  useEffect(() => {
    document.title = `${t('common.tam')} - ${room.building.name} ${room.name}`;
  }, [room.building.name, room.name]);

  function handleCloseModal(): void {
    dispatch(closeModalAction());
  }

  function handleYesModal(): void {
    if (modalState.yesOkFunction) modalState.yesOkFunction();
    else dispatch(closeModalAction());
  }

  return (
    room && (
      <>
        <Segment basic>
          <Grid columns={2} stackable>
            <GridRow>
              <GridColumn>
                {t('roomDetails.room')}
                <Header as="h1">
                  {room.building.name} <span className={styles.roomNameHeader}>{room.name}</span>
                </Header>
              </GridColumn>
            </GridRow>
          </Grid>

          <RoomInfo room={room} />

          <OutlookBookingMessage visible={bookThisRoomInOutlook} />

          <TeamsSection teams={room.projectsInRoom} />

          <DesksSection desks={room.desksInRoom} bookThisRoomInOutlook={bookThisRoomInOutlook} employees={projectEmployees} roomId={id} />

        </Segment>

        <SimpleModal
          isOpen={modalState.isModalOpen}
          text={modalState.text}
          isOkBtnOnly={modalState.isOkBtnOnly}
          closeFunction={handleCloseModal}
          yesOkFunction={handleYesModal}
        />
      </>
    ));
};

export default Room;
