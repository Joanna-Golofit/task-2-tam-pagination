import React, { useEffect } from 'react';
import { Segment, Header, Icon } from 'semantic-ui-react';
import { useSelector, useDispatch, shallowEqual } from 'react-redux';
import { useTranslation } from 'react-i18next';
import { AppState } from '../../store';
import { fetchUserDetails } from '../../store/userDetails/actions';
import UserDetails from './components/UserDetails';
import UserReservations from './components/UserReservations';
import { setLoadingAction } from '../../store/global/actions';
import UserProjects from './components/UserProjects';
import UserEquipmentsSection from '../UserDetails/components/userEquipmentsSections';

const UserDashboard: React.FC = () => {
  const { t } = useTranslation();
  const dispatch = useDispatch();

  const { loggedUserData } = useSelector((state: AppState) => state.global, shallowEqual);
  const userDetailsStore = useSelector((state: AppState) => state.userDetails);
  const userDetails = userDetailsStore.item;

  useEffect(() => {
    if (loggedUserData?.id) {
      dispatch(setLoadingAction(true));
      dispatch(fetchUserDetails(loggedUserData.id));
    }
  }, [dispatch, loggedUserData]);

  useEffect(() => {
    document.title = t('common.tam');
  }, []);

  return (
    userDetails && (
      <>
        <Segment basic>
          <Header as="h1">
            <span>
              <Icon color="blue" name="user" />{`${userDetails.employeeName} ${userDetails.employeeSurname}`}
            </span>
          </Header>

          <UserDetails />
          <UserProjects projects={userDetails.projects} />
          <UserReservations
            reservationInfo={userDetails.reservationInfo}
            email={loggedUserData.email}
            forTeamLeader={false}
          />
          {!loggedUserData.isStandardUser() && (
            <UserReservations
              reservationInfo={userDetails.ledProjectsReservationInfo}
              email={loggedUserData.email}
              forTeamLeader
            />
          )}
          {userDetails.id && <UserEquipmentsSection userId={userDetails.id} />}
        </Segment>
      </>
    ));
};

export default UserDashboard;
