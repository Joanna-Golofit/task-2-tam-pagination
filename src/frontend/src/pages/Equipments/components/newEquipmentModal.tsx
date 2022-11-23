import { useFormik } from 'formik';
import React, { useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { Button, Form, Modal } from 'semantic-ui-react';
import * as Yup from 'yup';
import { AppState } from '../../../store';
import { addEquipmentAction, closeAddEquipmentModalAction } from '../../../store/equipments/actions';
import { setLoadingAction } from '../../../store/global/actions';

type ComponentProps = {
}

const NewEquipmentModal: React.FC<ComponentProps> = () => {
  const { t } = useTranslation();
  const dispatch = useDispatch();

  const equipmentState = useSelector((state: AppState) => state.equipments);

  const { isAddEquipmentModalOpen: modalOpen } = equipmentState;

  const formik = useFormik({
    initialValues: {
      name: '',
      additionalInfo: '',
      count: 0,
    },
    validationSchema: Yup.object({
      name: Yup.string()
        .max(100, t('equipments.nameMaxErrMsg'))
        .min(3, t('equipments.nameMinErrMsg'))
        .required(t('equipments.nameReqErrMsg')),
      count: Yup.number()
        .required(t('equipments.countReqErrMsg')),
    }),
    onSubmit: (values) => {
      dispatch(setLoadingAction(true));
      dispatch(addEquipmentAction(values));
    },
  });

  useEffect(() => {
    formik.handleReset(null);
  }, [modalOpen]);

  const handleOnBlur = () => {
    formik.setValues({
      name: formik.values.name.trim(),
      additionalInfo: formik.values.additionalInfo.trim(),
      count: formik.values.count,
    });
  };

  const handleOnClose = () => {
    dispatch(closeAddEquipmentModalAction());
  };

  return (
    <Modal
      as={Form}
      onSubmit={formik.handleSubmit}
      onClose={handleOnClose}
      open={modalOpen}
    >
      <Modal.Header>{t('equipments.newEquipmentHeader')}</Modal.Header>
      <Modal.Content image>
        <Modal.Description>
          <Form.Group widths="equal">
            <Form.Input
              fluid
              label={t('equipments.name')}
              placeholder={t('equipments.name')}
              name="name"
              value={formik.values.name}
              onChange={formik.handleChange}
              onBlur={handleOnBlur}
              type="text"
              minLength={1}
              maxLength={255}
              width={12}
              error={formik.touched.name && formik.errors.name}
            />
            <Form.Input
              fluid
              label={t('equipments.additionalInfo')}
              placeholder={t('equipments.additionalInfo')}
              name="additionalInfo"
              value={formik.values.additionalInfo}
              onChange={formik.handleChange}
              onBlur={handleOnBlur}
              type="text"
            />
            <Form.Input
              fluid
              name="count"
              type="number"
              width="8"
              label={t('equipments.count')}
              placeholder={t('equipments.count')}
              onChange={formik.handleChange}
              min={0}
              value={formik.values.count || ''}
              error={formik.touched.count && formik.errors.count}
            />
          </Form.Group>
        </Modal.Description>
      </Modal.Content>
      <Modal.Actions>
        <Button
          positive
          type="submit"
        >
          {t('equipments.addEquipmentBtn')}
        </Button>
        <Button color="black" onClick={handleOnClose}>
          {t('common.close')}
        </Button>
      </Modal.Actions>
    </Modal>
  );
};

export default NewEquipmentModal;
