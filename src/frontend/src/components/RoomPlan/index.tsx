import React from 'react';
import { Image }
  from 'semantic-ui-react';
import { RoomDetailsDto } from '../../services/room/models';
import styles from './roomPlan.module.scss';
import { generateRoomPlanDownloadUrl }
  from '../../services/room/uploadRoomPlanAzureApiService';
import { RoomForProjectDto } from '../../services/project/models';

type Props = {
  roomItem: RoomDetailsDto | RoomForProjectDto;
  imageKey?: string;
}

const RoomPlan: React.FC<Props> = ({ roomItem, imageKey }) => (
  <>
    <Image
      className={styles.roomPlan}
      src={roomItem.name ? generateRoomPlanDownloadUrl(roomItem) : ''}
      rounded
      centered
      key={imageKey || ''}
    />
  </>
);

export default RoomPlan;
