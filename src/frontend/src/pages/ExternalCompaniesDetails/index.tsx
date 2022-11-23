import React, { useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { RouteComponentProps, useHistory } from 'react-router-dom';
import { Header, Segment } from 'semantic-ui-react';
import { AppState } from '../../store';
import { setLoadingAction } from '../../store/global/actions';
import EmployeesTable from './components/employeesTable';
import ExternalCompanyInfo from './components/externalCompanyInfo';
import SimpleModal from '../../layouts/components/SimpleModal';
import { Routes } from '../../Routes';
import RoomsTable from '../ProjectDetails/components/roomsTable';
import { clearProjectDetails, closeRemoveProjectModalAction, fetchProjectDetails, removeProjectAction } from '../../store/projectDetails/actions';

type ExternalCompaniesDetailsProps = {
    companyId: string;
  } & RouteComponentProps;

const ExternalCompaniesDetails: React.FC<ExternalCompaniesDetailsProps> = ({ companyId }) => {
  const dispatch = useDispatch();
  const { t } = useTranslation();
  const history = useHistory();

  const externalCompanyDetails = useSelector((state: AppState) => state.projectDetails);
  const { projectDetailsResponse: company, isRemoveCompanyModalOpen } = externalCompanyDetails;

  useEffect(() => {
    dispatch(setLoadingAction(true));
    dispatch(fetchProjectDetails(companyId));

    return () => {
      dispatch(clearProjectDetails());
    };
  }, []);

  useEffect(() => {
    document.title = `${t('common.tam')} - ${company.name}`;
  }, [company.name]);

  const navigate = () => {
    history.push(Routes.ExternalCompanies);
  };

  const handleSubmit = () => {
    dispatch(setLoadingAction(true));
    dispatch(closeRemoveProjectModalAction());
    dispatch(removeProjectAction(companyId, navigate));
  };

  const closeModal = () => {
    dispatch(closeRemoveProjectModalAction());
  };

  return (
    <>
      <Segment basic>
        {
          company && (
            <>
              {t('externalCompanyDetails.header')}
              <Header as="h1">
                {company?.name}
              </Header>

              <ExternalCompanyInfo externalCompany={company} />

              <EmployeesTable />

              <RoomsTable
                employees={company.employees}
              />
            </>
          )
        }
      </Segment>

      <SimpleModal
        isOpen={isRemoveCompanyModalOpen}
        text={t('externalCompanyDetails.removeCompanyMsg')}
        isOkBtnOnly={false}
        closeFunction={closeModal}
        yesOkFunction={handleSubmit}
      />
    </>
  );
};

export default ExternalCompaniesDetails;
