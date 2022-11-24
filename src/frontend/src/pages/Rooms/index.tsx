import React, { useEffect, useState } from 'react';
import {
  Header,
  Table,
  Button,
  Transition,
  Grid,
  Segment,
  TableCell,
  Responsive,
} from 'semantic-ui-react';
import { useSelector, useDispatch } from 'react-redux';
import { useTranslation } from 'react-i18next';
import { useHistory } from 'react-router-dom';
import { AppState } from '../../store';
import { fetchRooms } from '../../store/rooms/actions';
import Filters, { FilterOptions } from './components/filters';
import { Routes } from '../../Routes';
import styles from './rooms.module.scss';
import { setLoadingAction } from '../../store/global/actions';
import { TABLET_MEDIA_WIDTH } from '../../globalConstants';
import usePagination from '../../hooks/use-pagination';

const Rooms: React.FC = () => {
  const dispatch = useDispatch();
  const rooms = useSelector((state: AppState) => state.rooms); // tu mamy rooms.roomsResponse.rooms
  const [filteredRooms, setFilteredRooms] = useState(rooms.roomsResponse.rooms);

  const {
    currentPageNo,
    pageSize,
    startFromIndex,
    paginatedFilteredData,
    paginationNavigation,
  } = usePagination(filteredRooms);

  const [visible, setVisible] = useState(false);
  const { t } = useTranslation();
  const routerHistory = useHistory();

  const onFilter = (filters: FilterOptions) => {
    setFilteredRooms(rooms?.roomsResponse?.rooms.filter(
      (r) => (!filters.room || r.name.toLowerCase().includes(filters.room.trim().toLowerCase())) &&
      (!filters.buildingIds || filters.buildingIds.some((x) => x === r.building.id)) &&
        (!filters.floors || filters.floors.some((x) => x === r.floor)) &&
        (!filters.freeSpaceMin || r.capacity - r.occupiedDesksCount - r.hotDesksCount - r.disabledDesksCount >= filters.freeSpaceMin) &&
        (!filters.freeSpaceMax || r.capacity - r.occupiedDesksCount - r.hotDesksCount - r.disabledDesksCount <= filters.freeSpaceMax) &&
        (!filters.capacityMin || r.capacity >= filters.capacityMin) &&
        (!filters.capacityMax || r.capacity <= filters.capacityMax),
    ));
  };

  useEffect(() => {
    dispatch(setLoadingAction(true));
    dispatch(fetchRooms());
  }, [dispatch]);

  useEffect(() => {
    setFilteredRooms(rooms?.roomsResponse?.rooms);
  }, [rooms, startFromIndex, pageSize, currentPageNo]);

  useEffect(() => {
    document.title = `${t('common.tam')} - ${t('rooms.header')}`;
  }, [t]);

  function handleTableRowClick(roomId: string): void {
    routerHistory.push(`${Routes.Rooms}/${roomId}`);
  }

  let occupiedDesksCountSum = 0;
  let capacitySum = 0;
  let unoccupiedDesksCountSum = 0;
  let disabledDesksCountSum = 0;
  let hotDeskCountSum = 0;

  const rows = paginatedFilteredData.map((i) => {
    occupiedDesksCountSum += i.occupiedDesksCount;
    capacitySum += i.capacity;
    unoccupiedDesksCountSum += (i.capacity - i.occupiedDesksCount - i.hotDesksCount - i.disabledDesksCount);
    disabledDesksCountSum += i.disabledDesksCount;
    hotDeskCountSum += i.hotDesksCount;
    return (
      <Table.Row
        key={i.id}
        onClick={() => handleTableRowClick(i.id)}
        className={styles.roomRow}
        negative={i.freeDesksCount === 0}
      >
        <Table.Cell>{i.building.name}</Table.Cell>
        <Table.Cell>{i.name}</Table.Cell>
        <Table.Cell>{i.floor}</Table.Cell>
        <Table.Cell>{i.occupiedDesksCount}</Table.Cell>
        <Table.Cell>{i.freeDesksCount}</Table.Cell>
        <Table.Cell>{i.disabledDesksCount}</Table.Cell>
        <Table.Cell>{i.hotDesksCount}</Table.Cell>
        <Table.Cell>{i.capacity}</Table.Cell>
      </Table.Row>
    );
  });

  const buildingOptions = rooms.roomsResponse.buildings.map((b) => ({ key: b.id, text: b.name, value: b.id }));
  const handleToggleFilters = () => setVisible(!visible);

  return (
    <Segment basic>
      <Grid columns={2} padded="vertically">
        <Grid.Row verticalAlign="middle">
          <Grid.Column>
            <Header as="h1">{t('rooms.header')}</Header>
          </Grid.Column>
          <Grid.Column textAlign="right">
            <Button onClick={handleToggleFilters}>
              {visible ? t('common.hideFilters') : t('common.showFilters')}
            </Button>
          </Grid.Column>
        </Grid.Row>
      </Grid>
      <Transition visible={visible} duration={0}>
        <div>
          <Filters
            buildingOptions={buildingOptions}
            maxFloor={rooms.roomsResponse.maxFloor}
            onFilter={onFilter}
          />
        </div>
      </Transition>
      <Table compact unstackable selectable color="orange">
        <Table.Header>
          {/* Show long header labels if screen width bigger than tablet */}
          <Responsive as={Table.Row} minWidth={TABLET_MEDIA_WIDTH}>
            <Table.HeaderCell>{t('rooms.building')}</Table.HeaderCell>
            <Table.HeaderCell>{t('rooms.room')}</Table.HeaderCell>
            <Table.HeaderCell>{t('rooms.floor')}</Table.HeaderCell>
            <Table.HeaderCell>{t('rooms.occupiedDesks')}</Table.HeaderCell>
            <Table.HeaderCell>{t('rooms.unoccupiedDesks')}</Table.HeaderCell>
            <Table.HeaderCell>{t('rooms.disabledDesks')}</Table.HeaderCell>
            <Table.HeaderCell>{t('rooms.hotDesks')}</Table.HeaderCell>
            <Table.HeaderCell>{t('rooms.allPlaces')}</Table.HeaderCell>
          </Responsive>
          {/* Show short header labels if screen width smaller than tablet */}
          <Responsive as={Table.Row} maxWidth={TABLET_MEDIA_WIDTH - 1}>
            <Table.HeaderCell>{t('rooms.buildingShort')}</Table.HeaderCell>
            <Table.HeaderCell>{t('rooms.roomShort')}</Table.HeaderCell>
            <Table.HeaderCell>{t('rooms.floorShort')}</Table.HeaderCell>
            <Table.HeaderCell>{t('rooms.occupiedDesksShort')}</Table.HeaderCell>
            <Table.HeaderCell>
              {t('rooms.unoccupiedDesksShort')}
            </Table.HeaderCell>
            <Table.HeaderCell>{t('rooms.disabledDesksShort')}</Table.HeaderCell>
            <Table.HeaderCell>{t('rooms.hotDesksShort')}</Table.HeaderCell>
            <Table.HeaderCell>{t('rooms.allPlacesShort')}</Table.HeaderCell>
          </Responsive>
        </Table.Header>
        <Table.Body>
          {filteredRooms.length !== 0 ? (
            rows
          ) : (
            <Table.Row>
              <TableCell colSpan={7} textAlign="center">
                <i>{t('common.noResultsFilters')}</i>
              </TableCell>
            </Table.Row>
          )}
        </Table.Body>
        {filteredRooms.length !== 0 && (
          <Table.Footer>
            {/* Show long header labels if screen width bigger than tablet */}
            <Responsive as={Table.Row} minWidth={TABLET_MEDIA_WIDTH}>
              <Table.HeaderCell colSpan={3}>
                <b>{t('rooms.summary')}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{t('rooms.occupiedDesks')}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{t('rooms.unoccupiedDesks')}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{t('rooms.disabledDesks')}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{t('rooms.hotDesks')}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{t('rooms.allPlaces')}</b>
              </Table.HeaderCell>
            </Responsive>
            <Responsive as={Table.Row} minWidth={TABLET_MEDIA_WIDTH}>
              <Table.HeaderCell colSpan={3} />
              <Table.HeaderCell>
                <b>{occupiedDesksCountSum}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{unoccupiedDesksCountSum}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{disabledDesksCountSum}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{hotDeskCountSum}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{capacitySum}</b>
              </Table.HeaderCell>
            </Responsive>
            {/* Show long header labels if screen width bigger than tablet */}
            <Responsive as={Table.Row} maxWidth={TABLET_MEDIA_WIDTH - 1}>
              <Table.HeaderCell colSpan={3}>
                <b>{t('rooms.summaryShort')}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{t('rooms.occupiedDesksShort')}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{t('rooms.unoccupiedDesksShort')}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{t('rooms.disabledDesksShort')}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{t('rooms.hotDesksShort')}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{t('rooms.allPlacesShort')}</b>
              </Table.HeaderCell>
            </Responsive>
            <Responsive as={Table.Row} maxWidth={TABLET_MEDIA_WIDTH - 1}>
              <Table.HeaderCell colSpan={3} />
              <Table.HeaderCell>
                <b>{occupiedDesksCountSum}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{unoccupiedDesksCountSum}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{disabledDesksCountSum}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{hotDeskCountSum}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{capacitySum}</b>
              </Table.HeaderCell>
            </Responsive>
          </Table.Footer>
        )}
      </Table>
      {paginationNavigation}
    </Segment>
  );
};

export default Rooms;
