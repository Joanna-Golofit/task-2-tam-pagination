/* eslint-disable jsx-a11y/no-static-element-interactions */
/* eslint-disable jsx-a11y/click-events-have-key-events */
import React, { useEffect, useState } from 'react';
import { Table, Select, DropdownProps, Accordion, Button } from 'semantic-ui-react';
import { useTranslation } from 'react-i18next';
import { useSelector, useDispatch } from 'react-redux';
import styles from './roomsTable.module.scss';
import { AppState } from '../../../store';
import { fetchRooms } from '../../../store/rooms/actions';
import RoomDetailsAccordion from './RoomDetailsAccordion';
import { EmployeeForProjectDetailsDto } from '../../../services/project/models';
import { clearNewRoomAction, getRoomDetailsForProject, removeEmployeesFromRoomAction } from '../../../store/projectDetails/actions';
import { setLoadingAction } from '../../../store/global/actions';

type ComponentProps = {
  employees: EmployeeForProjectDetailsDto[];
};

type Room = {
  index: number;
  id: string;
}

const RoomsTable: React.FC<ComponentProps> = ({ employees }) => {
  const { t } = useTranslation();
  const dispatch = useDispatch();

  const [activeRooms, setActiveRooms] = useState([] as string[]);
  const { roomsResponse } = useSelector((state: AppState) => state.rooms);
  const { rooms: roomsForDropdown } = roomsResponse;

  const filteredEmployees = employees.filter((e) => e.workmode !== 1 && e.workmode != null);

  const [selectedRoom, setSelectedRoom] = useState({} as Room);
  const [expandAll, setExpandAll] = useState(false);

  const { newRoom, projectDetailsResponse: project } = useSelector((state: AppState) => state.projectDetails);
  const [rooms, setRooms] = useState([] as Room[]);

  useEffect(() => {
    dispatch(setLoadingAction(true));
    dispatch(fetchRooms());
  }, []);

  const roomsOptions = roomsForDropdown.map((room, index) => ({
    index,
    key: room.id,
    text: `${room.building.name} ${room.name}`,
    value: room.id }));

  const filteredRoomOptions = roomsOptions.filter((room) => room.key !== selectedRoom?.id &&
  !rooms.some((r) => r.id === room.key));

  useEffect(() => {
    setRooms([...project.rooms.map((r, index) => ({
      index,
      id: r.id,
    }))]);
  }, [project.rooms]);

  const roomChangeHandler = (_: React.SyntheticEvent<HTMLElement, Event>, data: DropdownProps) => {
    setSelectedRoom({
      index: roomsOptions.find((r) => r.key === data.value)?.index as number,
      id: data.value as string,
    });
  };

  const toggleAccordionHandler = (roomId: string, event: React.MouseEvent<HTMLButtonElement>) => {
    event.stopPropagation();
    if (activeRooms.some((activeRoom) => roomId === activeRoom)) {
      setActiveRooms((prevState) => (prevState.filter((activeRoom) => activeRoom !== roomId)));
    } else {
      setActiveRooms((prevState) => [...prevState, roomId]);
    }
  };

  const addRoomHandler = () => {
    if (selectedRoom?.id) {
      dispatch(setLoadingAction(true));
      dispatch(getRoomDetailsForProject(selectedRoom?.id));
      setActiveRooms(() => [selectedRoom.id]);
    }
  };

  const removeRoomHandler = (roomId: string) => {
    dispatch(setLoadingAction(true));
    dispatch(removeEmployeesFromRoomAction(roomId, project.id));

    setRooms((prevState) =>
      prevState.filter((r) => r.id !== roomId));
    setSelectedRoom({} as Room);
    dispatch(clearNewRoomAction());
  };

  const expandAllHandler = () => {
    setExpandAll((prevState) => !prevState);
    if (!expandAll) setActiveRooms(project.rooms.map((room) => room.id));
    else setActiveRooms([]);
  };

  const allEmployeesRemote = filteredEmployees.length === 0;

  return (
    <>
      <div className={styles.header}>
        <Select
          search
          text={roomsOptions.find((r) => r.key === selectedRoom?.id)?.text || t('projectDetails.selectRoom')}
          selectOnBlur={false}
          onChange={roomChangeHandler}
          noResultsMessage={t('common.noResultsFilters')}
          options={filteredRoomOptions}
          disabled={filteredEmployees.length === 0}
          value={selectedRoom?.id || allEmployeesRemote}
        />
        <Button
          color="green"
          basic
          circular
          icon="plus"
          onClick={addRoomHandler}
          disabled={allEmployeesRemote}
        />
      </div>
      <Table color="orange" unstackable>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell>{t('projectDetails.room')}</Table.HeaderCell>
            <Table.HeaderCell>{t('projectDetails.occupationWithTeam')}</Table.HeaderCell>
            <Table.HeaderCell textAlign="center">
              <div className={styles.expandRoomPlans}>
                {t('projectDetails.expandRoomPlan')}
                <Button
                  className={styles.expand}
                  onClick={expandAllHandler}
                  icon={expandAll ? 'chevron down' : 'chevron left'}
                  basic
                  circular
                  size="mini"
                  color="grey"
                />
              </div>
            </Table.HeaderCell>
            <Table.HeaderCell colSpan={2} />
          </Table.Row>
        </Table.Header>
        <Accordion
          fluid
          exclusive={false}
          as={Table.Body}
        >
          {rooms.length === 0 && !newRoom.id &&
            <Table.Row><Table.Cell colSpan={4} textAlign="center"><p>{t('common.noResultsFilters')}</p></Table.Cell></Table.Row>}
          {rooms.length !== 0 &&
            rooms.filter((room) => room.id).sort((a, b) => (a.index > b.index ? 1 : -1)).map((room, i) => (
              <RoomDetailsAccordion
                employees={filteredEmployees}
                key={room.id}
                roomId={room.id}
                index={i}
                onClickRemoveRoom={removeRoomHandler}
                onClickToggleAccordion={toggleAccordionHandler}
                activeRooms={activeRooms}
              />
            ))}
          {(newRoom.id && !rooms.find((r) => r.id === newRoom.id)) && (
            <>
              <RoomDetailsAccordion
                employees={filteredEmployees}
                key={newRoom.id}
                roomId={newRoom.id}
                index={Math.max(...rooms.map((r) => r.index)) + 1}
                onClickRemoveRoom={removeRoomHandler}
                onClickToggleAccordion={toggleAccordionHandler}
                activeRooms={activeRooms}
              />
            </>
          )}
        </Accordion>
      </Table>
    </>
  );
};

export default RoomsTable;
