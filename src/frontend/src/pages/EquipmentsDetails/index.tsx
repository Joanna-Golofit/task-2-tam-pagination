import React, { useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { RouteComponentProps, useHistory } from 'react-router-dom';
import { Header, Segment } from 'semantic-ui-react';
import SimpleModal from '../../layouts/components/SimpleModal';
import { Routes } from '../../Routes';
import { AppState } from '../../store';
import {
  clearEquipmentDetailsAction, fetchEquipmentDetailsAction, removeEquipmentAction,
  closeRemoveEquipmentModalAction,
} from '../../store/equipmentDetails/actions';
import { setLoadingAction } from '../../store/global/actions';
import EquipmentInfo from './components/equipmentInfo';
import EquipmentsTable from './components/EquipmentsTable';

type EquipmentsDetailsProps = {
  equipmentId: string;
} & RouteComponentProps;

const EquipmentsDetails: React.FC<EquipmentsDetailsProps> = ({ equipmentId }) => {
  const dispatch = useDispatch();
  const { t } = useTranslation();
  const history = useHistory();

  const equipmentDetails = useSelector((state: AppState) => state.equipmentDetails);
  const { equipmentDetailsResponse: equipment, isRemoveEquipmentModalOpen } = equipmentDetails;

  useEffect(() => {
    dispatch(setLoadingAction(true));

    dispatch(fetchEquipmentDetailsAction(equipmentId));
    return () => {
      dispatch(clearEquipmentDetailsAction());
    };
  }, []);

  useEffect(() => {
    document.title = `${t('common.tam')} - ${equipment.name}`;
  }, [equipment.name]);

  const handleRemoveSubmit = () => {
    dispatch(setLoadingAction(true));
    dispatch(closeRemoveEquipmentModalAction());
    dispatch(removeEquipmentAction(equipmentId, navigate));
  };

  const closeRemoveModal = () => {
    dispatch(closeRemoveEquipmentModalAction());
  };

  const navigate = () => {
    history.push(Routes.Equipments);
  };

  return (
    <>
      <Segment basic>
        {
          equipment && (
            <>
              {t('equipmentDetails.header')}
              <Header as="h1">
                {equipment?.name}
              </Header>
              <EquipmentInfo />
              <EquipmentsTable />
            </>
          )
        }
      </Segment>

      <SimpleModal
        isOpen={isRemoveEquipmentModalOpen}
        text={t('equipmentDetails.removeEquipmentMsg')}
        isOkBtnOnly={false}
        closeFunction={closeRemoveModal}
        yesOkFunction={handleRemoveSubmit}
      />
    </>
  );
};

export default EquipmentsDetails;
