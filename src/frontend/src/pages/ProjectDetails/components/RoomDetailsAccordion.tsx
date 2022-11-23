import React, { useEffect, useState } from 'react';
import { Table, Accordion, Image, Button, Responsive, Modal } from 'semantic-ui-react';
import { useSelector } from 'react-redux';
import { useTranslation } from 'react-i18next';
import { useHistory } from 'react-router-dom';
import { AppState } from '../../../store';
import styles from './RoomDetailsAccordion.module.scss';
import { Routes } from '../../../Routes';
import { EmployeeForProjectDetailsDto, RoomForProjectDto } from '../../../services/project/models';
import SimpleModal from '../../../layouts/components/SimpleModal';
import { LAPTOP_MEDIA_WIDTH } from '../../../globalConstants';
import RoomView from '../../../components/RoomView/RoomView';
import { fetchProjectDetails } from '../../../store/projectDetails/actions';

type ComponentProps = {
  roomId: string;
  activeRooms: string[];
  onClickRemoveRoom: (roomId: string, event: React.MouseEvent<HTMLButtonElement>) => void
  onClickToggleAccordion: (roomId: string, event: React.MouseEvent<HTMLButtonElement>) => void;
  employees: EmployeeForProjectDetailsDto[];
};

const RoomDetailsAccordion: React.FC<ComponentProps> = ({
  roomId,
  activeRooms,
  onClickRemoveRoom,
  onClickToggleAccordion,
  employees,
}) => {
  const { t } = useTranslation();
  const history = useHistory();

  const [disabledEmployees, setDisabledEmployees] = useState([] as (string | undefined)[]);
  const [clickEvent, setClickEvent] = useState({} as React.MouseEvent<HTMLButtonElement>);
  const [isOpen, setIsOpen] = useState(false);
  const [isPlanOpen, setIsPlanOpen] = useState(false);

  const { newRoom, projectDetailsResponse: project } = useSelector((state: AppState) => state.projectDetails);
  const { rooms } = project;
  let roomDtos = rooms;
  if (!rooms.find((r) => r.id === newRoom.id)) {
    roomDtos = [newRoom].concat(rooms);
  }
  const room = roomDtos?.find((r) => r.id === roomId);
  const { id, building, name, sasTokenForRoomPlans, freeDesksCount } = room || {} as RoomForProjectDto;
  const { name: buildingName } = building || '';
  const desks = room?.desksInRoom;

  const employeesIds = employees.map((e) => e.id);

  const occupiedDesksCount = desks?.filter((d) => employeesIds.some((e) => d.reservations?.map((r) => r.employee.id).includes(e))).length || 0;

  useEffect(() => {
    if (desks) {
      setDisabledEmployees(
        desks?.flatMap((d) => d.reservations?.map((r) => r.employee.id)),
      );
    }
  }, [rooms]);

  const storageAccountName = process.env.REACT_APP_TAM_AZURE_STORAGE_NAME;
  const roomPlansBlobContainerUrl = `https://${storageAccountName}.blob.core.windows.net/room-plans/`;
  const cleanRoomName = name?.replace(new RegExp(' ', 'g'), '').replace(new RegExp('-', 'g'), '');
  const filename = `${buildingName + cleanRoomName}.jpg`;
  const roomPlanImageUrl = `${roomPlansBlobContainerUrl + filename}${sasTokenForRoomPlans}`;

  const navigateToRoom = (targetId: string) => {
    history.push(`${Routes.Rooms}/${targetId}`);
  };

  const clickRemoveRoomHandler = (event: React.MouseEvent<HTMLButtonElement>) => {
    event.stopPropagation();
    setIsOpen(true);
    setClickEvent(event);
  };

  const active = activeRooms.some((activeRoom) => activeRoom === roomId);

  const roomPlan = (
    sasTokenForRoomPlans && (
    <Image
      className={styles.roomPlan}
      src={roomPlanImageUrl}
      rounded
      centered
    />
    )
  );
  return (
    <>
      {desks && (
      <Accordion.Title key={id} as={Table.Row} className={`${styles.accordion} ${styles.ui} ${styles.roomRow}`}>
        <Table.Cell key={`room-name-${id}-key`}>
          {/* eslint-disable-next-line jsx-a11y/click-events-have-key-events */}
          <span role="presentation" className={styles.roomLink} onClick={() => navigateToRoom(id)}>{buildingName} {name}</span>
        </Table.Cell>
        <Table.Cell key={`occupied-desks-${id}-key`}>
          {`${occupiedDesksCount} / ${freeDesksCount}`}
        </Table.Cell>
        <Table.Cell
          textAlign="center"
          key={`expand-${id}-key`}
        ><Button
          className={styles.expand}
          onClick={(event: any) =>
            onClickToggleAccordion(roomId, event)}
          icon={active ? 'chevron down' : 'chevron left'}
          basic
          circular
          color="grey"
        />
        </Table.Cell>
        <Table.Cell textAlign="center">
          <Button
            circular
            color="red"
            basic
            icon="trash"
            onClick={(event: React.MouseEvent<HTMLButtonElement>) => {
              clickRemoveRoomHandler(event);
            }}
          />
        </Table.Cell>
        <Table.Cell textAlign="center">
          {/* Show room plan button that opens modal with room plan if screen size smaller than laptop*/}
          <Responsive maxWidth={LAPTOP_MEDIA_WIDTH}>
            <Button
              circular
              color="blue"
              basic
              icon="map outline"
              onClick={() => setIsPlanOpen(true)}
            />
          </Responsive>
        </Table.Cell>
      </Accordion.Title>
      )}
      <Accordion.Content
        as={Table.Row}
        active={active}
      >
        <Table.Cell colSpan={5} className={styles.desksRoomPlanCell}>
          {active && (
            <RoomView
              desks={room?.desksInRoom}
              employees={employees}
              disabledEmployees={disabledEmployees}
              projectId={project.id}
              roomFetchAction={fetchProjectDetails(project.id)}
              room={room}
              roomId={roomId}
            />
          )}
        </Table.Cell>
      </Accordion.Content>
      <SimpleModal
        isOpen={isOpen}
        text={t('projectDetails.removeRoomMessage')}
        closeFunction={() => setIsOpen(false)}
        yesOkFunction={() => {
          onClickRemoveRoom(roomId, clickEvent);
        }}
        isOkBtnOnly={false}
      />
      {/* Modal for room plan button */}
      <Modal open={isPlanOpen}>
        <Button
          icon="close"
          size="massive"
          onClick={() => setIsPlanOpen(false)}
          className={styles.button}
        />{roomPlan}
      </Modal>
    </>
  );
};

export default RoomDetailsAccordion;
