import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Modal, Form, InputOnChangeData } from 'semantic-ui-react';
import { useSelector, useDispatch } from 'react-redux';
import { AppState } from '../../../store';
import { addDesksAction, openAddDesksModalAction } from '../../../store/roomDetails/actions';
import { setLoadingAction } from '../../../store/global/actions';

const AddDesksModal: React.FC = () => {
  const dispatch = useDispatch();
  const { t } = useTranslation();
  const roomStore = useSelector((state: AppState) => state.room);
  const roomItem = roomStore.item;
  const [addDesksInputParameters, setAddDesksInputParameters] = useState({ firstDeskNumber: 1,
    numberOfDesks: 1,
    errorFirstDeskNumber: false as any,
    errorNumberOfDesks: false as any });

  const resetAddDesksInputParameters = () => setAddDesksInputParameters({ firstDeskNumber: 1,
    numberOfDesks: 1,
    errorFirstDeskNumber: false,
    errorNumberOfDesks: false });

  function handleAddDesksSubmit(): void {
    dispatch(openAddDesksModalAction(false));
    resetAddDesksInputParameters();
    dispatch(setLoadingAction(true));
    dispatch(addDesksAction({ roomId: roomItem.id,
      firstDeskNumber: addDesksInputParameters.firstDeskNumber,
      numberOfDesks: addDesksInputParameters.numberOfDesks }));
  }

  function handleOnCloseAddDesksModal(): void {
    dispatch(openAddDesksModalAction(false));
    resetAddDesksInputParameters();
  }

  function handleOnChangeAddDesksModal(event: React.ChangeEvent<HTMLInputElement>, data: InputOnChangeData): void {
    const valueInt = Number(data.value);
    switch (data.name) {
      case 'firstDeskNumber':
        {
          const errorFirstDeskNumber: any = valueInt < 1 ? t('roomDetails.valueMustBeGreaterThanZero') : false;
          setAddDesksInputParameters({ ...addDesksInputParameters,
            firstDeskNumber: Number(data.value),
            errorFirstDeskNumber });
        }
        break;
      case 'numberOfDesks':
        {
          const errorNumberOfDesks: any = valueInt < 1 ? t('roomDetails.valueMustBeGreaterThanZero') : false;
          setAddDesksInputParameters({ ...addDesksInputParameters,
            numberOfDesks: Number(data.value),
            errorNumberOfDesks });
        }
        break;
      default:
        break;
    }
  }

  return (
    <Modal
      open={roomStore.isOpenAddDesksModal}
      closeOnEscape={false}
      closeOnDimmerClick={false}
      size="small"
      closeIcon
      onClose={handleOnCloseAddDesksModal}
    >
      <Modal.Header>{t('roomDetails.addDesksToRoom')} {roomItem.building.name} {roomItem.name}</Modal.Header>
      <Modal.Content>
        <h5>{t('roomDetails.areYouSureToChangePlans')}</h5>
        <Form onSubmit={handleAddDesksSubmit}>
          <Form.Group widths="equal">
            <Form.Input
              fluid
              label={t('roomDetails.firstDeskNumber')}
              placeholder={t('roomDetails.firstDeskNumber')}
              name="firstDeskNumber"
              value={addDesksInputParameters.firstDeskNumber}
              onChange={handleOnChangeAddDesksModal}
              type="number"
              min={1}
              max={50}
              error={addDesksInputParameters.errorFirstDeskNumber}
            />
            <Form.Input
              fluid
              label={t('roomDetails.numberOfDesks')}
              placeholder={t('roomDetails.numberOfDesks')}
              name="numberOfDesks"
              value={addDesksInputParameters.numberOfDesks}
              onChange={handleOnChangeAddDesksModal}
              type="number"
              min={1}
              max={50}
              error={addDesksInputParameters.errorNumberOfDesks}
            />
          </Form.Group>
          <Form.Button>{t('roomDetails.add')}</Form.Button>
        </Form>
      </Modal.Content>
    </Modal>
  );
};

export default AddDesksModal;
