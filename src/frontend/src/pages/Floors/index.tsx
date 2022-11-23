import React, { useEffect, useState } from 'react';
import { Header, Grid, Segment, Button, Table, TableCell, Responsive } from 'semantic-ui-react';
import { useSelector, useDispatch } from 'react-redux';
import { useTranslation } from 'react-i18next';
import { AppState } from '../../store';
import { fetchFloors } from '../../store/floors/actions';
import { setLoadingAction } from '../../store/global/actions';
import { TABLET_MEDIA_WIDTH } from '../../globalConstants';

const Floors: React.FC = () => {
  const dispatch = useDispatch();
  const floors = useSelector((state: AppState) => state.floors);
  const [filteredFloors, setFilteredFloors] = useState(floors.floorsResponse.floors);
  const [visible, setVisible] = useState(false);
  const { t } = useTranslation();

  useEffect(() => {
    dispatch(setLoadingAction(true));
    dispatch(fetchFloors());
  }, [dispatch]);

  useEffect(() => {
    document.title = `${t('common.tam')} - ${t('floors.header')}`;
  }, [t]);

  useEffect(() => {
    setFilteredFloors(floors?.floorsResponse?.floors);
  }, [floors]);

  let occupiedDesksCountSum = 0;
  let unoccupiedDesksCountSum = 0;
  let capacitySum = 0;

  const rows = filteredFloors.map((i) => {
    occupiedDesksCountSum += i.occupiedDesks;
    unoccupiedDesksCountSum += (i.capacity - i.occupiedDesks);
    capacitySum += i.capacity;
    return (
      <Table.Row key={i.id}>
        <Table.Cell>{i.building.name}</Table.Cell>
        <Table.Cell>{i.floor}</Table.Cell>
        <Table.Cell>{i.area}</Table.Cell>
        <Table.Cell>{i.roomCount}</Table.Cell>
        <Table.Cell>{i.occupiedDesks}</Table.Cell>
        <Table.Cell>{i.capacity - i.occupiedDesks}</Table.Cell>
        <Table.Cell>{i.capacity}</Table.Cell>
      </Table.Row>
    );
  });

  return (
    <Segment basic>
      <Grid columns={2} padded="vertically">
        <Grid.Row verticalAlign="middle">
          <Grid.Column>
            <Header as="h1">{t('floors.header')}</Header>
          </Grid.Column>
          <Grid.Column textAlign="right">
            <Button onClick={() => setVisible(!visible)}>
              {visible ? t('common.hideFilters') : t('common.showFilters')}
            </Button>
          </Grid.Column>
        </Grid.Row>
      </Grid>
      <Table compact unstackable color="orange">
        <Table.Header>
          {/* Show long header labels if screen width bigger than tablet */}
          <Responsive as={Table.Row} minWidth={TABLET_MEDIA_WIDTH}>
            <Table.HeaderCell>{t('floors.building')}</Table.HeaderCell>
            <Table.HeaderCell>{t('floors.floor')}</Table.HeaderCell>
            <Table.HeaderCell>{t('floors.area')}</Table.HeaderCell>
            <Table.HeaderCell>{t('floors.roomsCount')}</Table.HeaderCell>
            <Table.HeaderCell>{t('floors.occupiedDesks')}</Table.HeaderCell>
            <Table.HeaderCell>{t('floors.unoccupiedDesks')}</Table.HeaderCell>
            <Table.HeaderCell>{t('floors.capacity')}</Table.HeaderCell>
          </Responsive>
          {/* Show short header labels if screen width smaller than tablet */}
          <Responsive as={Table.Row} maxWidth={TABLET_MEDIA_WIDTH - 1}>
            <Table.HeaderCell>{t('floors.buildingShort')}</Table.HeaderCell>
            <Table.HeaderCell>{t('floors.floorShort')}</Table.HeaderCell>
            <Table.HeaderCell>{t('floors.areaShort')}</Table.HeaderCell>
            <Table.HeaderCell>{t('floors.roomsCountShort')}</Table.HeaderCell>
            <Table.HeaderCell>
              {t('floors.occupiedDesksShort')}
            </Table.HeaderCell>
            <Table.HeaderCell>
              {t('floors.unoccupiedDesksShort')}
            </Table.HeaderCell>
            <Table.HeaderCell>{t('floors.capacityShort')}</Table.HeaderCell>
          </Responsive>
        </Table.Header>
        <Table.Body>
          {filteredFloors.length !== 0 ? (
            rows
          ) : (
            <Table.Row>
              <TableCell textAlign="center" colSpan={7}>
                <i>{t('common.noResultsFilters')}</i>
              </TableCell>
            </Table.Row>
          )}
        </Table.Body>
        {filteredFloors.length !== 0 && (
          <Table.Footer>
            {/* Show long footer labels if screen width bigger than tablet */}
            <Responsive as={Table.Row} minWidth={TABLET_MEDIA_WIDTH}>
              <Table.HeaderCell colSpan={4}>
                <b>{t('floors.summary')}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{t('floors.occupiedDesks')}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{t('floors.unoccupiedDesks')}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{t('floors.capacity')}</b>
              </Table.HeaderCell>
            </Responsive>
            <Responsive as={Table.Row} minWidth={TABLET_MEDIA_WIDTH}>
              <Table.HeaderCell colSpan={4} />
              <Table.HeaderCell>
                <b>{occupiedDesksCountSum}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{unoccupiedDesksCountSum}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{capacitySum}</b>
              </Table.HeaderCell>
            </Responsive>
            {/* Show short footer labels if screen width smaller than tablet */}
            <Responsive as={Table.Row} maxWidth={TABLET_MEDIA_WIDTH - 1}>
              <Table.HeaderCell colSpan={4}>
                <b>{t('floors.summaryShort')}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{t('floors.occupiedDesksShort')}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{t('floors.unoccupiedDesksShort')}</b>
              </Table.HeaderCell>
              <Table.HeaderCell><b>{t('floors.capacityShort')}</b></Table.HeaderCell>
            </Responsive>
            <Responsive as={Table.Row} maxWidth={TABLET_MEDIA_WIDTH - 1}>
              <Table.HeaderCell colSpan={4} />
              <Table.HeaderCell>
                <b>{occupiedDesksCountSum}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{unoccupiedDesksCountSum}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{capacitySum}</b>
              </Table.HeaderCell>
            </Responsive>
          </Table.Footer>
        )}
      </Table>
    </Segment>
  );
};

export default Floors;
