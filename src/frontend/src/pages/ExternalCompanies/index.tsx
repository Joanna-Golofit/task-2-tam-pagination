import React, { useCallback, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useHistory, useLocation } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { Segment, Grid, Header, Button, Table, Responsive, TableCell } from 'semantic-ui-react';
import { AppState } from '../../store';
// import Paginator from '../../components/Paginator/Paginator';
import { ListOptions } from '../../services/common/models';
import { LAPTOP_MEDIA_WIDTH } from '../../globalConstants';
import NewCompanyModal from './components/newCompanyModal';
import { setLoadingAction } from '../../store/global/actions';
import styles from './externalCompanies.module.scss';
import { fetchProjects, openAddProjectModalAction } from '../../store/projects/actions';
import usePagination from '../../hooks/use-pagination';

const ExternalCompanies: React.FC = () => {
  const history = useHistory();
  const location = useLocation();
  const { t } = useTranslation();
  const dispatch = useDispatch();

  const externalCompaniesState = useSelector((state: AppState) => state.projects);
  const columnCount = 3;

  const [listOptions] = useState({ pageNumber: 1, pageSize: 20 } as ListOptions);
  const { projects: companies, filters, projectsCount: companiesCount } = externalCompaniesState;
  const [filtersState] = useState(filters);

  const { paginatedFilteredData, paginationNavigation } = usePagination(
    companies,
    25,
  );

  const fetchExternalCompaniesList = useCallback(() => {
    dispatch(setLoadingAction(true));
    dispatch(fetchProjects());
  }, [filtersState, listOptions]);

  useEffect(() => {
    fetchExternalCompaniesList();
  }, [listOptions, filtersState, fetchExternalCompaniesList]);

  useEffect(() => {
    document.title = `${t('common.tam')} - ${t('externalCompanies.header')}`;
  }, [t]);

  const navigateToCompany = (id: string) => {
    history.push(`${location.pathname}/${id}`);
  };

  // const onPageChange = (activePage: number, pageSize: number) => {
  //   setListOptions((options) => (
  //     {
  //       ...options,
  //       pageNumber: activePage,
  //       pageSize,
  //     }
  //   ));
  // };

  const openModal = () => {
    dispatch(openAddProjectModalAction());
  };

  const rows = paginatedFilteredData.map((company) => (
    <Table.Row
      key={company.id}
      onClick={() => navigateToCompany(company.id)}
      className={styles.row}
    >
      <Table.Cell>{company.name}</Table.Cell>
      <Table.Cell>{company.peopleCount}</Table.Cell>
      <Table.Cell>{company.unassignedMembersCount}</Table.Cell>
    </Table.Row>
  ));

  return (
    <>
      <Segment basic>
        <Grid columns={2} padded="vertically">
          <Grid.Row verticalAlign="middle">
            <Grid.Column>
              <div className={styles.headerContainer}>
                <Header as="h1">{t('externalCompanies.header')}</Header>
                <Button
                  color="green"
                  basic
                  circular
                  icon="plus"
                  onClick={openModal}
                  className={styles.newCompanyBtn}
                />
              </div>
            </Grid.Column>
          </Grid.Row>
        </Grid>
        <Table compact unstackable color="orange">
          <Table.Header>
            {/* Show long header labels if screen width bigger than tablet */}
            <Responsive as={Table.Row} minWidth={LAPTOP_MEDIA_WIDTH}>
              <Table.HeaderCell>{t('externalCompanies.name')}</Table.HeaderCell>
              <Table.HeaderCell>
                {t('externalCompanies.assignedPeople')}
              </Table.HeaderCell>
              <Table.HeaderCell>
                {t('externalCompanies.unassignedMembersCount')}
              </Table.HeaderCell>
            </Responsive>
            {/* Show short header labels if screen width smaller than tablet */}
            <Responsive as={Table.Row} maxWidth={LAPTOP_MEDIA_WIDTH - 1}>
              <Table.HeaderCell>{t('externalCompanies.name')}</Table.HeaderCell>
              <Table.HeaderCell>
                {t('externalCompanies.assignedPeopleShort')}
              </Table.HeaderCell>
              <Table.HeaderCell>
                {t('externalCompanies.unassignedMembersCountShort')}
              </Table.HeaderCell>
            </Responsive>
          </Table.Header>
          <Table.Body>
            {companiesCount === 0 ? (
              <Table.Row>
                <TableCell colSpan={columnCount} textAlign="center">
                  <i>{t('common.noResultsFilters')}</i>
                </TableCell>
              </Table.Row>
            ) : (
              rows
            )}
          </Table.Body>
        </Table>
        {paginationNavigation}
      </Segment>
      <NewCompanyModal navigate={navigateToCompany} />
    </>
  );
};

export default ExternalCompanies;
