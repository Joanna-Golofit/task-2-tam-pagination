import React, { useCallback, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { useHistory, useLocation } from 'react-router-dom';
import { Button, Grid, Header, Responsive, Segment, Table, TableCell } from 'semantic-ui-react';
import { LAPTOP_MEDIA_WIDTH } from '../../globalConstants';
import { AppState } from '../../store';
import { fetchEquipments, openAddEquipmentModalAction } from '../../store/equipments/actions';
import { setLoadingAction } from '../../store/global/actions';
import NewEquipmentModal from './components/newEquipmentModal';
import styles from './equipments.module.scss';

const Equipments: React.FC = () => {
  const { t } = useTranslation();
  const dispatch = useDispatch();
  const history = useHistory();
  const location = useLocation();

  const equipmentsState = useSelector((state: AppState) => state.equipments);

  const { equipments, filters } = equipmentsState;
  const [filtersState] = useState(filters);

  const openAddModal = () => {
    dispatch(openAddEquipmentModalAction());
  };

  const fetchEquipmentsList = useCallback(() => {
    dispatch(setLoadingAction(true));
    dispatch(fetchEquipments());
  }, [filtersState]);

  useEffect(() => {
    fetchEquipmentsList();
  }, [filtersState]);

  useEffect(() => {
    document.title = `${t('common.tam')} - ${t('equipments.header')}`;
  }, [t]);

  const navigateToDetail = (id: string) => {
    history.push(`${location.pathname}/${id}`);
  };

  const rows = equipments.map((equipment) => (
    <Table.Row
      key={equipment.id}
      className={`${styles.projectRow}`}
      onClick={() => navigateToDetail(equipment.id)}
    >
      <Table.Cell>{equipment.name}</Table.Cell>
      <Table.Cell>{equipment.additionalInfo}</Table.Cell>
      <Table.Cell>{equipment.count}</Table.Cell>
      <Table.Cell>{equipment.assignedPeopleCount}</Table.Cell>
    </Table.Row>
  ));

  return (
    <>
      <Segment basic>
        <Grid columns={2} padded="vertically">
          <Grid.Row verticalAlign="middle">
            <Grid.Column>
              <div className={styles.headerContainer}>
                <Header as="h1">{t('equipments.header')}</Header>
                <Button
                  color="green"
                  basic
                  circular
                  icon="plus"
                  onClick={openAddModal}
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
              <Table.HeaderCell>{t('equipments.name')}</Table.HeaderCell>
              <Table.HeaderCell>{t('equipments.additionalInfo')}</Table.HeaderCell>
              <Table.HeaderCell>{t('equipments.count')}</Table.HeaderCell>
              <Table.HeaderCell>{t('equipments.assignedPeopleCount')}</Table.HeaderCell>
            </Responsive>
            {/* Show short header labels if screen width smaller than tablet */}
            <Responsive as={Table.Row} maxWidth={LAPTOP_MEDIA_WIDTH - 1}>
              <Table.HeaderCell>{t('equipments.name')}</Table.HeaderCell>
              <Table.HeaderCell>{t('equipments.additionalInfo')}</Table.HeaderCell>
              <Table.HeaderCell>{t('equipments.count')}</Table.HeaderCell>
              <Table.HeaderCell>{t('equipments.assignedPeopleCount')}</Table.HeaderCell>
            </Responsive>
          </Table.Header>
          <Table.Body>{equipments?.length === 0 ? (
            <Table.Row>
              <TableCell colSpan={4} textAlign="center"><i>{t('common.noResultsFilters')}</i></TableCell>
            </Table.Row>
          ) : rows}
          </Table.Body>
        </Table>
      </Segment>
      <NewEquipmentModal />
    </>
  );
};

export default Equipments;
