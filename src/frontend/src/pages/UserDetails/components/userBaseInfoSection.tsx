import React from 'react';
import { Label, Icon, Dropdown, LabelDetail } from 'semantic-ui-react';
import { useHistory } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import styles from './userBaseInfoSection.module.scss';
import { UserDetailsDto } from '../../../services/user/models';
import { Routes } from '../../../Routes';
import { getEmployeeTypeDisplayName, getWorkspaceTypeDisplayName, WorkspaceType } from '../../../services/user/enums';

type Props = {
  userDetails: UserDetailsDto;
};

const UserBaseInfoSection: React.FC<Props> = ({ userDetails }) => {
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
      {userDetails.isExternal === false && (
      <Label href={`mailto:${userDetails.email}`}>
        <Icon color="blue" name="mail" />
        <Label.Detail>
          <span className={styles.email}>{userDetails.email}</span>
        </Label.Detail>
      </Label>
      )}
      <Label>
        <Icon color="grey" name="user circle outline" />
        <Label.Detail>{getEmployeeTypeDisplayName(userDetails.employeeType)}</Label.Detail>
      </Label>
      <Label>
        <Icon color={userDetails.workspaceType === null ? 'red' : 'grey'} name="map marker alternate" />
        <Label.Detail>
          <span className={userDetails.workspaceType === null ? styles.workspaceNotSet : ''}>
            {getWorkspaceTypeDisplayName(userDetails.workspaceType)}
          </span>
        </Label.Detail>
      </Label>
      {userDetails.workspaceType !== WorkspaceType.Remote && userDetails.workspaceType !== null && (
      <>
        {userDetails?.locations.length === 0 && (
        <Label as="div">
          <Icon color="grey" name="warehouse" />
          <Label.Detail>
            <span>{t('userDetails.noDeskAssignments')}</span>
          </Label.Detail>
        </Label>
        )}
        {userDetails?.locations.length === 1 && (
        <Label
          as={userDetails.locations ? 'a' : 'div'}
          onClick={(event) => navigateToRoom(event, userDetails.locations[0].roomId)}
        >
          <Icon color="blue" name="warehouse" />
          <Label.Detail>
            <span className={styles.locationText}>
              {`${userDetails.locations[0].buildingName} - ${userDetails.locations[0].roomName} 
            ${t('projectDetails.desk')} ${userDetails.locations[0].deskNumber}`}
            </span>
          </Label.Detail>
        </Label>
        )}
        {userDetails.locations.length > 1 && (
        <Label>
          <div
            id={styles.labelLocation}
            className={styles.labelLocation}
          >
            <Icon color="grey" name="warehouse" />
            <LabelDetail>
              <Dropdown
                text={t('userDetails.locations')}
                scrolling
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
            </LabelDetail>
          </div>
        </Label>
        )}
      </>
      )}
    </Label.Group>

  );
};

export default UserBaseInfoSection;
