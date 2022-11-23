import React from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { Header, Message, Segment } from 'semantic-ui-react';
import { UserRole } from '../../services/user/enums';
import { AppState } from '../../store';

type AuthGuardProps = {
    userRole: UserRole;
  };

const AuthGuard: React.FC<AuthGuardProps> = ({ children, userRole }) => {
  const { loggedUserData, isLoadingCounter } = useSelector((state: AppState) => state.global);
  const { t } = useTranslation();

  return (
    <>
      {
        loggedUserData.isUserAdmin() || loggedUserData.hasRole(userRole) ?
          children : (!isLoadingCounter && (
            <Segment basic>
              <Header as="h1">{t('auth.header')}</Header>
              <Message>
                <Message.Header>{t('auth.errorMsg')}{userRole}</Message.Header>
                <p>
                  {t('auth.contactAdmin')}
                </p>
              </Message>
            </Segment>
          ))
        }
    </>
  );
};

export default AuthGuard;
