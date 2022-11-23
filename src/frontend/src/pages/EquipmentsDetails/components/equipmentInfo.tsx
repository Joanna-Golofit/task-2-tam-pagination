import React from 'react';
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { Button } from 'semantic-ui-react';
import { AppState } from '../../../store';
import { openEditEquipmentModalAction, openRemoveEquipmentModalAction, openReservationEquipmentModalAction } from '../../../store/equipmentDetails/actions';
import EditEquipmentModal from './editEquipmentModal';
import styles from './equipmentInfo.module.scss';
import ReserveEquipmentModal from './reserveEquipmentModal';

const EquipmentInfo: React.FC = () => {
  const { t } = useTranslation();
  const dispatch = useDispatch();

  const equipmentDetails = useSelector((state: AppState) => state.equipmentDetails);
  const { equipmentDetailsResponse: equipment } = equipmentDetails;

  const openRemoveModal = () => {
    dispatch(openRemoveEquipmentModalAction());
  };

  const openEditModal = () => {
    dispatch(openEditEquipmentModalAction());
  };

  const openReserveModal = () => {
    dispatch(openReservationEquipmentModalAction());
  };

  return (equipment && (
    <>
      <div className={styles.container}>
        <Button
          color="red"
          content={t('equipmentDetails.removeEquipment')}
          onClick={openRemoveModal}
        />
        <div>
          <Button
            color="blue"
            content={t('equipmentDetails.editEquipment')}
            onClick={openEditModal}
          />
          <Button
            color="green"
            content={t('equipmentDetails.reservation')}
            onClick={openReserveModal}
          />
        </div>
      </div>
      <EditEquipmentModal />
      <ReserveEquipmentModal />
    </>
  ));
};

export default EquipmentInfo;
