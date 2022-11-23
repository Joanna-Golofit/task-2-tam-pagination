import React from 'react';
import { useTranslation } from 'react-i18next';
import { Label, Icon } from 'semantic-ui-react';
import styles from './roomInfo.module.scss';
import { RoomDetailsDto } from '../../../services/room/models';

type Props = {
  room: RoomDetailsDto;
};

const RoomInfo: React.FC<Props> = ({ room }) => {
  const { t } = useTranslation();
  const takenAreaPerPerson = (room.occupiedDesksCount ? (room.area / room.occupiedDesksCount).toFixed(2) : 0) as number;
  const isAreaPerPersonBelowMin = takenAreaPerPerson ? takenAreaPerPerson < room.areaMinLevelPerPerson : false;

  return (
    <Label.Group size="big" className={styles.label}>
      <Label>
        {room.capacity}
        <Label.Detail>{t('roomDetails.desks')}</Label.Detail>
      </Label>
      <Label>
        {room.area} m2
        <Label.Detail>{t('roomDetails.area')}</Label.Detail>
      </Label>
      <Label>
        {room.desksInRoom && room.desksInRoom.length ?
          (room.area / room.desksInRoom.length).toFixed(2) : '-'} m2/{t('roomDetails.person')}
      </Label>
      <Label>
        <span className={styles.numberOfPeople}>{room.occupiedDesksCount}</span>
        <Label.Detail><Icon color="blue" name="group" /></Label.Detail>
      </Label>
      <Label>
        <span className={isAreaPerPersonBelowMin ? styles.takenAreaAlert : styles.takenArea}>
          {takenAreaPerPerson || '-' } m2/{t('roomDetails.person')}
        </span>
      </Label>
      <Label>
        <span className={styles.numberOfFreeDesks}>{room.freeDesksCount}</span>
        <Label.Detail className={styles.numberOfFreeDesks}>{t('roomDetails.free')}</Label.Detail>
      </Label>
    </Label.Group>
  );
};

export default RoomInfo;
