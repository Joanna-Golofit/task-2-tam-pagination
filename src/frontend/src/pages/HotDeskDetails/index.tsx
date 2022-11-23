import React, { useEffect } from 'react';
import { Segment, Header, GridRow, GridColumn, Grid } from 'semantic-ui-react';
import { useSelector, useDispatch } from 'react-redux';
import { useTranslation } from 'react-i18next';
import { AppState } from '../../store';
import { clearRoomDetails, fetchRoom } from '../../store/roomDetails/actions';
import { setLoadingAction } from '../../store/global/actions';
import DesksSection from './components/deskSection';

interface Props {
  id: string;
}

const HotDeskDetails: React.FC<Props> = (props) => {
  const { t } = useTranslation();
  const { id } = props;
  const dispatch = useDispatch();

  const { item: room } = useSelector((state: AppState) => state.room);

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

  return (
    room && (
      <>
        <Segment basic>
          <Grid columns={2} stackable>
            <GridRow>
              <GridColumn>
                {t('roomDetails.room')}
                <Header as="h1">
                  {room.building.name} <span>{room.name}</span>
                </Header>
              </GridColumn>
            </GridRow>
          </Grid>

          <DesksSection />

        </Segment>
      </>
    ));
};

export default HotDeskDetails;
