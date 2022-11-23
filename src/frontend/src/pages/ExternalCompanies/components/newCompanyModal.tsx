import React, { useEffect } from 'react';
import { Button, Modal, Form, InputOnChangeData } from 'semantic-ui-react';
import { useTranslation } from 'react-i18next';
import { useFormik } from 'formik';
import { useDispatch, useSelector } from 'react-redux';
import * as Yup from 'yup';
import { setLoadingAction } from '../../../store/global/actions';
import { AppState } from '../../../store';
import { addProjectAction, closeAddProjectModalAction } from '../../../store/projects/actions';

type ComponentProps = {
  navigate: (id: string) => void
}

const NewCompanyModal: React.FC<ComponentProps> = ({ navigate }) => {
  const { t } = useTranslation();
  const dispatch = useDispatch();

  const externalCompaniesState = useSelector((state: AppState) => state.projects);

  const { isAddProjectModalOpen: modalOpen } = externalCompaniesState;

  const formik = useFormik({
    initialValues: {
      name: '',
      email: '',
      initialEmployeeCount: 0,
    },
    validationSchema: Yup.object({
      name: Yup.string()
        .max(255, t('externalCompanies.nameMaxErrMsg'))
        .min(1, t('externalCompanies.nameMinErrMsg'))
        .required(t('externalCompanies.nameReqErrMsg')),
      email: Yup.string().email(t('externalCompanies.emailErrMsg')).required(t('externalCompanies.emailReqErrMsg')),
      initialEmployeeCount: Yup.number()
        .min(0, t('externalCompanies.numberMinErrMsg'))
        .required(t('externalCompanies.numberOfEmployeeReqErrMsg')),
    }),
    onSubmit: (values) => {
      dispatch(setLoadingAction(true));
      dispatch(addProjectAction(values, navigate));
    },
  });

  useEffect(() => {
    formik.handleReset(null);
  }, [modalOpen]);

  const handleOnChange = (event: React.SyntheticEvent<HTMLElement>, data: InputOnChangeData) => {
    formik.setFieldValue(data.name, data.type === 'number' ? Number(data.value) : data.value);
  };

  const handleOnClose = () => {
    dispatch(closeAddProjectModalAction());
  };

  return (
    <Modal
      as={Form}
      onSubmit={formik.handleSubmit}
      onClose={handleOnClose}
      open={modalOpen}
    >
      <Modal.Header>{t('externalCompanies.newCompanyHeader')}</Modal.Header>
      <Modal.Content image>
        <Modal.Description>
          <Form.Group widths="equal">
            <Form.Input
              fluid
              label={t('externalCompanies.companyName')}
              placeholder={t('externalCompanies.companyName')}
              name="name"
              value={formik.values.name}
              onChange={handleOnChange}
              type="text"
              minLength={1}
              maxLength={255}
              width={12}
              error={formik.touched.name && formik.errors.name}
            />
            <Form.Input
              fluid
              label={t('externalCompanies.numberOfEmployee')}
              placeholder={t('externalCompanies.numberOfEmployee')}
              name="initialEmployeeCount"
              value={formik.values.initialEmployeeCount}
              onChange={handleOnChange}
              type="number"
              width={4}
              error={formik.touched.initialEmployeeCount && formik.errors.initialEmployeeCount}
            />
          </Form.Group>
          <Form.Input
            fluid
            label={t('externalCompanies.companyEmail')}
            placeholder={t('externalCompanies.companyEmail')}
            name="email"
            value={formik.values.email}
            onChange={handleOnChange}
            type="text"
            error={formik.touched.email && formik.errors.email}
          />
        </Modal.Description>
      </Modal.Content>
      <Modal.Actions>
        <Button
          positive
          type="submit"
        >{t('externalCompanies.addCompanyBtm')}
        </Button>
        <Button color="black" onClick={handleOnClose}>
          {t('common.close')}
        </Button>
      </Modal.Actions>
    </Modal>
  );
};

export default NewCompanyModal;
