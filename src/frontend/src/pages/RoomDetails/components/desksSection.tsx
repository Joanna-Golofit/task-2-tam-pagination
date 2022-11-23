import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Table, Dropdown, Button, Icon, Modal }
  from 'semantic-ui-react';
import { useSelector, useDispatch } from 'react-redux';
import styles from './deskSection.module.scss';
import { DeskForRoomDetailsDto, EmployeeForRoomDetailsDto } from '../../../services/room/models';
import { AppState } from '../../../store';
import { openAddDesksModalAction, fetchRoom, getEmployeesForRoomDetails, setRoomHotDeskAction, toggleDeskIsEnabled }
  from '../../../store/roomDetails/actions';
import { ProjectForDropdownDto } from '../../../services/project/models';
import getAllProjectsForDropdownApiService from '../../../services/project/getAllProjectsForDropdownApiService';
import RoomPlanSection from './roomPlanSection';
import { setLoadingAction } from '../../../store/global/actions';
import SimpleModal from '../../../layouts/components/SimpleModal';
import RoomView from '../../../components/RoomView/RoomView';
import AddDesksModal from './addDesksModal';

type Props = {
  desks: DeskForRoomDetailsDto[];
  employees: EmployeeForRoomDetailsDto[];
  roomId: string;
  bookThisRoomInOutlook: boolean;
}

const DesksSection: React.FC<Props> = ({ desks, employees, roomId, bookThisRoomInOutlook }) => {
  const dispatch = useDispatch();
  const { t } = useTranslation();
  const [allProjects, setAllProjects] = useState([] as Array<ProjectForDropdownDto>);

  const [modal, setModal] = useState({ isOpen: false, yesFunction: () => {}, message: '' });
  const roomStore = useSelector((state: AppState) => state.room);
  const roomItem = roomStore.item;

  const { loggedUserData } = useSelector((state: AppState) => state.global);
  const [isFetchingProjects, setIsFetchingProjects] = useState(false);
  const [selectedProjectId, setSelectedProjectId] = useState('');
  const [roomToolsVisible, setRoomToolsVisible] = useState(false);
  const [isPlanOpen, setIsPlanOpen] = useState(false);

  const [isNotHotDesksRoom, setRoomHotDesk] = useState(desks.some((desk: DeskForRoomDetailsDto) => !desk.isHotDesk));

  useEffect(() => {
    if (!loggedUserData.isStandardUser()) {
      setIsFetchingProjects(true);
      getAllProjectsForDropdownApiService().subscribe((returnProjects) => {
        setAllProjects(returnProjects);
        setIsFetchingProjects(false);
      });
    }
  }, [loggedUserData]);

  useEffect(() => {
    setRoomHotDesk(desks.some((desk: DeskForRoomDetailsDto) => !desk.isHotDesk));
  });

  const dropdownProjects = allProjects
    .map((projectDto) => ({ key: projectDto.id, value: projectDto.id, text: projectDto.name }));

  function handleOnChangeProjectsDropdown(projectId: string): void {
    setSelectedProjectId(projectId);
    dispatch(getEmployeesForRoomDetails(projectId, roomItem.id));
  }
  function handleSetRoomHotDeskButton():void {
    const disabledDesks = [];
    desks.forEach((desk) => {
      if (!desk.isEnabled) {
        disabledDesks.push(desk);
      }
    });
    const desksIds = disabledDesks.map((desk) => desk.id);
    if (desks.map((desk) => !!desk).includes(true)) {
      setModal({ isOpen: true,
        message: t('roomDetails.setRoomAsHotDeskMessage'),
        yesFunction: () => {
          dispatch(setLoadingAction(true));
          dispatch(setRoomHotDeskAction({ isHotDesk: true, roomId }, selectedProjectId));
          dispatch(toggleDeskIsEnabled({ desksIds, isEnabled: true }, selectedProjectId, roomId, fetchRoom(roomId)));
          setModal((prevState) => ({ ...prevState, isOpen: false }));
        } });
    } else {
      dispatch(setLoadingAction(true));
      dispatch(setRoomHotDeskAction({ isHotDesk: true, roomId }, selectedProjectId));
      dispatch(toggleDeskIsEnabled({ desksIds, isEnabled: true }, selectedProjectId, roomId, fetchRoom(roomId)));
    }
  }

  function handleAddDesksButton():void {
    dispatch(openAddDesksModalAction(true));
  }

  return (
    <>
      <Table color="orange" stackable striped>
        <Table.Header className={loggedUserData.isStandardUser() ? styles.hidden : ''}>
          <Table.Row>
            <Table.HeaderCell textAlign="left" width={3}>
              <div className={styles.buttonWrapper}>
                <div className={styles.insideWrapper}>
                  {t('roomDetails.edit')}
                  <Icon
                    color="blue"
                    className={styles.toggle}
                    name={roomToolsVisible ? 'toggle on' : 'toggle off'}
                    disabled={bookThisRoomInOutlook}
                    size="big"
                    onClick={() => setRoomToolsVisible((prevState) => !prevState)}
                  />
                </div>
                {loggedUserData.isUserAdmin() === true && roomToolsVisible && (
                  <div className={styles.insideWrapper}>
                    {t('roomDetails.addDesks')}
                    <Button
                      color="green"
                      basic
                      circular
                      icon="plus"
                      onClick={handleAddDesksButton}
                    />
                  </div>
                )}
                {roomToolsVisible && isNotHotDesksRoom && (
                  <div className={styles.insideWrapper}>
                    {t('roomDetails.setRoomAsHotDesk')}
                    <Button
                      color="blue"
                      basic
                      circular
                      icon="travel"
                      onClick={handleSetRoomHotDeskButton}
                    />
                  </div>
                )}
              </div>
            </Table.HeaderCell>
            {!roomToolsVisible && (
              <Table.HeaderCell textAlign="right">
                <Dropdown
                  selectOnBlur={false}
                  placeholder={t('roomDetails.selectTeam')}
                  search
                  selection
                  disabled={bookThisRoomInOutlook}
                  options={dropdownProjects}
                  loading={isFetchingProjects}
                  noResultsMessage={t('common.noResultsFilters')}
                  onChange={(event, { value }) => handleOnChangeProjectsDropdown(value as string)}
                />
              </Table.HeaderCell>
            )}
          </Table.Row>
        </Table.Header>
        <Table.Body>
          <Table.Row>
            <Table.Cell colSpan={3} id={styles.desksRoomPlanCell}>
              <RoomView
                desks={desks}
                editMode={roomToolsVisible}
                employees={employees}
                projectId={selectedProjectId}
                room={roomItem}
                roomId={roomId}
                roomFetchAction={fetchRoom(roomId)}
              />
            </Table.Cell>
          </Table.Row>
        </Table.Body>
      </Table>
      <AddDesksModal />
      {/* Modal for room plan button */}
      <Modal open={isPlanOpen}>
        <Button
          icon="close"
          size="massive"
          onClick={() => setIsPlanOpen(false)}
          className={styles.button}
        />
        <RoomPlanSection roomItem={roomItem} roomToolsVisible={roomToolsVisible} />
      </Modal>
      <SimpleModal
        isOpen={modal.isOpen}
        text={modal.message}
        closeFunction={() => {
          setModal((prevState) => ({ ...prevState, isOpen: false }));
        }}
        yesOkFunction={() => {
          modal.yesFunction();
        }}
        isOkBtnOnly={false}
      />
    </>
  );
};

export default DesksSection;
