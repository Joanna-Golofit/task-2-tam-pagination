import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useDispatch } from 'react-redux';
import { Button, Input, InputOnChangeData } from 'semantic-ui-react';
import { ProjectDetails } from '../../../services/project/models';
import { setLoadingAction } from '../../../store/global/actions';
import { addEmployeeToProject, openRemoveProjectModalAction } from '../../../store/projectDetails/actions';
import styles from './externalCompanyInfo.module.scss';

type ComponentProps = {
  externalCompany: ProjectDetails;
};

const ExternalCompanyInfo: React.FC<ComponentProps> = ({ externalCompany }) => {
  const { t } = useTranslation();
  const dispatch = useDispatch();

  const [numberOfEmployee, setNumberOfEmployee] = useState(1);

  const handleOnChange = (event: React.SyntheticEvent<HTMLElement>, data: InputOnChangeData) => {
    setNumberOfEmployee(Number(data.value));
  };

  const handleSumbit = () => {
    dispatch(setLoadingAction(true));
    dispatch(addEmployeeToProject({
      companyId: externalCompany.id,
      employeeCount: numberOfEmployee,
    }));
  };

  const openModal = () => {
    dispatch(openRemoveProjectModalAction());
  };

  return (externalCompany && (
    <div className={styles.container}>
      <Button
        color="red"
        content={t('externalCompanyDetails.removeCompany')}
        onClick={openModal}
      />
      <div className={styles.addBtn}>
        <span>{t('externalCompanyDetails.addEmployee')}</span>
        <Input type="number" value={numberOfEmployee} min={1} onChange={handleOnChange} />
        <span>
          {numberOfEmployee > 1 ? t('externalCompanyDetails.employees') : t('externalCompanyDetails.employee')}
        </span>
        <Button
          color="green"
          basic
          circular
          icon="plus"
          onClick={handleSumbit}
        />
      </div>
    </div>
  ));
};

export default ExternalCompanyInfo;
