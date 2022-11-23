import React, { useState, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { Button }
  from 'semantic-ui-react';
import { RoomDetailsDto } from '../../../services/room/models';
import { AppState } from '../../../store';
import styles from './roomPlanSection.module.scss';
import { uploadRoomPlanAzureApiService }
  from '../../../services/room/uploadRoomPlanAzureApiService';
import { showNotifyAction } from '../../../store/global/actions';
import RoomPlan from '../../../components/RoomPlan';
import { RoomForProjectDto } from '../../../services/project/models';

type Props = {
  roomItem: RoomDetailsDto | RoomForProjectDto;
  roomToolsVisible: boolean;
}

const RoomPlanSection: React.FC<Props> = ({ roomItem, roomToolsVisible }) => {
  const dispatch = useDispatch();
  const { t } = useTranslation();
  const inputFileRef = useRef(null as any);
  const [isUploading, setIsUploading] = useState(false);
  const [imageKey, setImageKey] = useState(Math.random().toString(36));

  const roomStore = useSelector((state: AppState) => state.room);

  async function onFileChange(event: any): Promise<any> {
    const file = event.target.files[0];

    if (!file) return;

    if (!file.type || !file.type.toUpperCase().includes('JPEG')) {
      dispatch(showNotifyAction(`${t('roomDetails.incorrectFileType')}`, true));
      return;
    }

    setIsUploading(true);

    await uploadRoomPlanAzureApiService(roomItem, file);

    setIsUploading(false);
    refreshImage();
    resetFileInput();
  }

  function refreshImage() {
    setImageKey(Math.random().toString(36));
  }

  function resetFileInput() {
    inputFileRef.current.value = null;
  }

  return (
    <>
      <div className={styles.header}>
        <div>
          <h3>{t('roomDetails.roomPlan')}</h3>
        </div>
        {roomToolsVisible && (
        <Button
          className={styles.uploadBtn}
          color="blue"
          basic
          circular
          icon="upload"
          onClick={() => inputFileRef.current.click()}
          disabled={roomStore.isViewDisabled}
          loading={isUploading}
        />
        )}
        <input ref={inputFileRef} hidden type="file" onChange={onFileChange} accept=".jpg" />
      </div>
      <RoomPlan
        roomItem={roomItem}
        imageKey={imageKey || ''}
      />
    </>
  );
};

export default RoomPlanSection;
