import React, { useEffect } from 'react';
import { Segment, Header, Icon } from 'semantic-ui-react';
import { useSelector, useDispatch } from 'react-redux';
import { useTranslation } from 'react-i18next';
import { AppState } from '../../store';
import { setLoadingAction } from '../../store/global/actions';
import styles from './userDetails.module.scss';
import UserBaseInfoSection from './components/userBaseInfoSection';
import UserTeamsSection from './components/userTeamsSection';
import UserWorkmodeSection from './components/userWorkmodeSection';
import { fetchUserDetails } from '../../store/userDetails/actions';
import UserEquipmentsSection from './components/userEquipmentsSections';

interface Props {
  userId: string;
}

const PersonDetails: React.FC<Props> = (props) => {
  const { t } = useTranslation();
  const { userId } = props;
  const dispatch = useDispatch();
  const userDetailsStore = useSelector((state: AppState) => state.userDetails);
  const { loggedUserData } = useSelector((state: AppState) => state.global);
  const userDetails = userDetailsStore.item;

  useEffect(() => {
    dispatch(setLoadingAction(true));
    dispatch(fetchUserDetails(userId));
  }, [dispatch, userId]);

  useEffect(() => {
    document.title = `${t('common.tam')} - ${userDetails.employeeName} ${userDetails.employeeSurname}`;
  }, [userDetails.employeeName]);

  useEffect(() => {
    document.title = `${t('common.tam')} - ${userDetails.employeeName} ${userDetails.employeeSurname}`;
  }, [userDetails.employeeSurname]);

  return (
    userDetails && (
      <>
        <Segment basic>
          <Header as="h1">
            <span className={styles.userNameHeader}>
              <Icon color="blue" name="user" />{`${t('common.tam')} - ${userDetails.employeeName} ${userDetails.employeeSurname}`}
            </span>
          </Header>

          <UserBaseInfoSection userDetails={userDetails} />
          {!loggedUserData.isStandardUser() && (
            <UserWorkmodeSection userDetails={userDetails} />
          )}
          <UserTeamsSection userDetails={userDetails} />

          <UserEquipmentsSection userId={userId} />
        </Segment>
      </>
    ));
};

export default PersonDetails;
