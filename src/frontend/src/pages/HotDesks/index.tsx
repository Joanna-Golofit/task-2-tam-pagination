import React, { useEffect, useState } from 'react';
import { Header, Table, Grid, Segment, TableCell, Responsive } from 'semantic-ui-react';
import { useSelector, useDispatch } from 'react-redux';
import { useTranslation } from 'react-i18next';
import { useHistory } from 'react-router-dom';
import { AppState } from '../../store';
import { Routes } from '../../Routes';
import styles from './hotDesks.module.scss';
import { setLoadingAction } from '../../store/global/actions';
import { TABLET_MEDIA_WIDTH } from '../../globalConstants';
import { fetchHotDesksAction } from '../../store/hotDesks/actions';
import { HotDeskFilters, HotDeskRespones } from '../../services/hotDesks/models';
import usePagination from '../../hooks/use-pagination';

const HotDesks: React.FC = () => {
  const dispatch = useDispatch();
  const { response, filters } = useSelector((state: AppState) => state.hotDesks);
  const [filteredRooms, setFilteredRooms] = useState(response.rooms);
  const [otherFilters] = useState<HotDeskFilters>({
    room: '',
    buildingIds: Array(0),
    floors: Array(0),
    freeHotDeskMin: undefined,
    freeHotDeskMax: undefined,
  });

  const {
    paginatedFilteredData,
    paginationNavigation,
  } = usePagination(filteredRooms, 2);

  const { t } = useTranslation();
  const history = useHistory();

  useEffect(() => {
    dispatch(setLoadingAction(true));
    dispatch(fetchHotDesksAction(filters));
  }, [filters]);

  useEffect(() => {
    runFilter(response);
  }, [otherFilters]);

  useEffect(() => {
    runFilter(response);
  }, [response]);

  useEffect(() => {
    document.title = `${t('common.tam')} - ${t('hotDesks.header')}`;
  }, [t]);

  function runFilter(res: HotDeskRespones) {
    setFilteredRooms(res.rooms.filter(
      (r) => (!otherFilters.room || r.name.toLowerCase().includes(otherFilters.room.trim().toLowerCase())) &&
        (otherFilters.buildingIds!.length === 0 || otherFilters.buildingIds!.some((x) => x === r.building.id)) &&
        (otherFilters.floors!.length === 0 || otherFilters.floors!.some((x) => x === r.floor)) &&
        (!otherFilters.freeHotDeskMin || r.freeHotDeskCount >= otherFilters.freeHotDeskMin) &&
        (!otherFilters.freeHotDeskMax || r.freeHotDeskCount <= otherFilters.freeHotDeskMax),
    ));
  }

  function handleTableRowClick(roomId: string): void {
    history.push(`${Routes.HotDesks}/${roomId}`);
  }

  let freeHotDeskSum = 0;
  let totalHotDeskCount = 0;

  const rows = paginatedFilteredData.map((i) => {
    freeHotDeskSum += i.freeHotDeskCount;
    totalHotDeskCount += i.hotDesksCount;
    return (
      <Table.Row
        key={i.id}
        onClick={() => handleTableRowClick(i.id)}
        className={styles.roomRow}
      >
        <Table.Cell>{i.building.name}</Table.Cell>
        <Table.Cell>{i.name}</Table.Cell>
        <Table.Cell>{i.floor}</Table.Cell>
        <Table.Cell>{i.freeHotDeskCount}</Table.Cell>
        <Table.Cell>{i.hotDesksCount}</Table.Cell>
      </Table.Row>
    );
  });

  return (
    <Segment basic>
      <Grid columns={2} padded="vertically">
        <Grid.Row verticalAlign="middle">
          <Grid.Column>
            <Header as="h1">{t('hotDesks.header')}</Header>
          </Grid.Column>
        </Grid.Row>
      </Grid>
      <Table compact unstackable selectable color="orange">
        <Table.Header>
          {/* Show long header labels if screen width bigger than tablet */}
          <Responsive as={Table.Row} minWidth={TABLET_MEDIA_WIDTH}>
            <Table.HeaderCell>{t('hotDesks.building')}</Table.HeaderCell>
            <Table.HeaderCell>{t('hotDesks.room')}</Table.HeaderCell>
            <Table.HeaderCell>{t('hotDesks.floor')}</Table.HeaderCell>
            <Table.HeaderCell>{t('hotDesks.freeHotDesks')}</Table.HeaderCell>
            <Table.HeaderCell>{t('hotDesks.totalHotDesks')}</Table.HeaderCell>
          </Responsive>
          {/* Show short header labels if screen width smaller than tablet */}
          <Responsive as={Table.Row} maxWidth={TABLET_MEDIA_WIDTH - 1}>
            <Table.HeaderCell>{t('hotDesks.buildingShort')}</Table.HeaderCell>
            <Table.HeaderCell>{t('hotDesks.roomShort')}</Table.HeaderCell>
            <Table.HeaderCell>{t('hotDesks.floorShort')}</Table.HeaderCell>
            <Table.HeaderCell>
              {t('hotDesks.freeHotDesksShort')}
            </Table.HeaderCell>
            <Table.HeaderCell>
              {t('hotDesks.totalHotDesksShort')}
            </Table.HeaderCell>
          </Responsive>
        </Table.Header>
        <Table.Body>
          {filteredRooms.length !== 0 ? (
            rows
          ) : (
            <Table.Row>
              <TableCell colSpan={5} textAlign="center">
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
                <b>{t('hotDesks.freeHotDesks')}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{t('hotDesks.totalHotDesks')}</b>
              </Table.HeaderCell>
            </Responsive>
            <Responsive as={Table.Row} minWidth={TABLET_MEDIA_WIDTH}>
              <Table.HeaderCell colSpan={3} />
              <Table.HeaderCell>
                <b>{freeHotDeskSum}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{totalHotDeskCount}</b>
              </Table.HeaderCell>
            </Responsive>
            {/* Show long header labels if screen width bigger than tablet */}
            <Responsive as={Table.Row} maxWidth={TABLET_MEDIA_WIDTH - 1}>
              <Table.HeaderCell colSpan={3}>
                <b>{t('rooms.summaryShort')}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{t('hotDesks.freeHotDesksShort')}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{t('hotDesks.totalHotDesksShort')}</b>
              </Table.HeaderCell>
            </Responsive>
            <Responsive as={Table.Row} maxWidth={TABLET_MEDIA_WIDTH - 1}>
              <Table.HeaderCell colSpan={3}>
                <b>{t('rooms.summaryShort')}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{freeHotDeskSum}</b>
              </Table.HeaderCell>
              <Table.HeaderCell>
                <b>{totalHotDeskCount}</b>
              </Table.HeaderCell>
            </Responsive>
          </Table.Footer>
        )}
      </Table>
      {paginationNavigation}
    </Segment>
  );
};

export default HotDesks;
