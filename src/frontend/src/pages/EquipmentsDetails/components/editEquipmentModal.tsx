import { useFormik } from 'formik';
import React, { useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { Button, Form, Modal } from 'semantic-ui-react';
import * as Yup from 'yup';
import { AppState } from '../../../store';
import { closeEditEquipmentModalAction, editEquipmentAction } from '../../../store/equipmentDetails/actions';
import { setLoadingAction } from '../../../store/global/actions';

const EditEquipmentModal: React.FC = () => {
  const { t } = useTranslation();
  const dispatch = useDispatch();

  const { isEditEquipmentModalOpen: isModalOpen, equipmentDetailsResponse: equipmentDetails } = useSelector((state: AppState) => state.equipmentDetails);

  const validationSchema = Yup.object().shape({
    name: Yup.string()
      .max(100, t('equipments.nameMaxErrMsg'))
      .min(3, t('equipments.nameMinErrMsg'))
      .required(t('equipments.nameReqErrMsg')),
    count: Yup.number()
      .required(t('equipmentDetails.equipmentQuantityIsRequiredMsg'))
      .min(equipmentDetails.employees.length, t('equipmentDetails.equipmentQuantityMinMsg')),
  });

  const formik = useFormik({
    initialValues: {
      name: '',
      count: 0,
    },
    validationSchema: () => validationSchema,
    onSubmit: (values) => {
      dispatch(setLoadingAction(true));
      dispatch(editEquipmentAction({ equipmentId: equipmentDetails.id, ...values }));
    },
  });

  const submitForm = () => {
    formik.handleSubmit();
  };

  useEffect(() => {
    formik.setValues({
      name: equipmentDetails.name,
      count: equipmentDetails.count,
    });
  }, [isModalOpen]);

  const handleOnBlur = () => {
    formik.setValues({
      name: formik.values.name.trim(),
      count: formik.values.count,
    });
  };

  const onModalClose = () => {
    dispatch(closeEditEquipmentModalAction());
  };

  return (
    <Modal open={isModalOpen} onClose={onModalClose} size="tiny">
      <Modal.Header>{t('equipmentDetails.modalEditHeader')}</Modal.Header>
      <Modal.Content>
        <Form>
          <Form.Input
            fluid
            label={t('equipmentDetails.name')}
            placeholder={t('equipmentDetails.name')}
            name="name"
            value={formik.values.name}
            onChange={formik.handleChange}
            onBlur={handleOnBlur}
            type="text"
            error={formik.errors.name}
          />
          <Form.Input
            fluid
            type="number"
            label={t('equipmentDetails.numberOfEquipments')}
            placeholder={t('equipmentDetails.numberOfEquipments')}
            name="count"
            value={formik.values.count}
            onChange={formik.handleChange}
            min={0}
            width={8}
            error={formik.errors.count}
          />
        </Form>
      </Modal.Content>
      <Modal.Actions>
        <Button
          size="tiny"
          disabled={!formik.isValid}
          onClick={submitForm}
        >
          {t('equipmentDetails.save')}
        </Button>
      </Modal.Actions>
    </Modal>
  );
};

export default EditEquipmentModal;
