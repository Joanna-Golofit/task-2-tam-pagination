import React from 'react';
import { useSelector } from 'react-redux';
import { Dropdown, Icon, Label } from 'semantic-ui-react';
import { useTranslation } from 'react-i18next';
import { useHistory } from 'react-router-dom';
import { Routes } from '../../../Routes';
import { AppState } from '../../../store';
import { getEmployeeTypeDisplayName, getWorkspaceTypeDisplayName, WorkspaceType } from '../../../services/user/enums';
import styles from './userDetails.module.scss';
import EmailLabel from '../../../components/EmailLabel';
import LabelIcon from '../../../components/LabelIcon';

const UserDetails: React.FC = () => {
  const userDetailsStore = useSelector((state: AppState) => state.userDetails);
  const userDetails = userDetailsStore.item;
  const { t } = useTranslation();
  const routerHistory = useHistory();

  const navigateToRoom = (event: React.MouseEvent<HTMLElement, MouseEvent>, roomId: string) => {
    event.preventDefault();
    if (userDetails.locations) {
      routerHistory.push(`${Routes.Rooms}/${roomId}`);
    }
  };

  return (
    <Label.Group as="div" size="big" className={styles.labelGroup}>
      <EmailLabel email={userDetails.email} />
      <LabelIcon iconColor="grey" iconName="user circle outline">
        {getEmployeeTypeDisplayName(userDetails.employeeType)}
      </LabelIcon>
      <LabelIcon iconColor={userDetails.workspaceType === null ? 'red' : 'grey'} iconName="map marker alternate">
        <span className={userDetails.workspaceType === null ? styles.workspaceNotSet : ''}>
          {getWorkspaceTypeDisplayName(userDetails.workspaceType)}
        </span>
      </LabelIcon>

      {userDetails.workspaceType !== WorkspaceType.Remote && userDetails.workspaceType !== null && (
        <>
          {userDetails?.locations.length === 0 && (
            <LabelIcon iconColor="grey" iconName="warehouse">
              <span>{t('userDetails.noDeskAssignments')}</span>
            </LabelIcon>
          )}
          {userDetails?.locations.length === 1 && (
            <LabelIcon iconColor="blue" iconName="warehouse">
              <span className={styles.locationText}>
                {`${userDetails.locations[0].buildingName} - ${userDetails.locations[0].roomName} 
                  ${t('projectDetails.desk')} ${userDetails.locations[0].deskNumber}`}
              </span>
            </LabelIcon>
          )}
          {userDetails.locations.length > 1 && (
            <Dropdown
              trigger={<span>{t('userDetails.locations')}<Icon name="dropdown" /></span>}
              scrolling
              button
              labeled
              icon="warehouse"
              className={`${styles.dropdown} ${styles.dropdown1} ${styles.dropdown2} ${styles.dropdown3} icon label`}
            >
              <Dropdown.Menu>
                {(userDetails.locations.map((location) => (
                  <Dropdown.Item
                    key={`${location.roomId}_${location.deskNumber}`}
                    onClick={(event) => navigateToRoom(event, location.roomId)}
                    className={styles.dropdownItem}
                  >
                    <span className={styles.locationText}>
                      {`${location.buildingName} - ${location.roomName} 
                          ${t('projectDetails.desk')} ${location.deskNumber}`}
                    </span>
                  </Dropdown.Item>
                )))}
              </Dropdown.Menu>
            </Dropdown>
          )}
        </>
      )}
    </Label.Group>
  );
};

export default UserDetails;
