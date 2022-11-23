/* eslint-disable jsx-a11y/no-static-element-interactions */
/* eslint-disable jsx-a11y/click-events-have-key-events */
import React from 'react';
import { Container, Dimmer, Loader } from 'semantic-ui-react';
import { useSelector, useDispatch } from 'react-redux';
import { useTranslation } from 'react-i18next';
import Navbar from './components/Navbar/Navbar';
import NotifyMessage from './components/NotifyMessage/NotifyMessage';
import { AppState } from '../store';
import { disposeErrorCodeInNotifyAction } from '../store/global/actions';
import ErrorBoundary from './components/ErrorBoundary';
import { ErrorCodes } from '../services/common/models';
import Footer from './components/Footer/Footer';
import styles from './Layout.module.scss';

const Layout: React.FC = ({ children }) => {
  const globalStore = useSelector((state: AppState) => state.global);
  const { t } = useTranslation();
  const dispatch = useDispatch();

  function mapCodeError(errorCode: ErrorCodes, status?: string | null): {content: string; title?: string} {
    const msg = status ? t(`exception.${status}`, { defaultValue: null }) : null;

    switch (errorCode) {
      case ErrorCodes.AccessDenied:
        return { content: msg || `${t('common.noAccess')}. ${t('common.contactAdmin')}.` };
      case ErrorCodes.RoomNotExist:
        return { content: msg || `${t('common.roomNotExist')}. ${t('common.contactAdmin')}.` };
      case ErrorCodes.ServerIsNotAccessible:
        return { content: msg || `${t('common.serverIsNotAccessibleContent')}`,
          title: `${t('common.serverIsNotAccessibleTitle')}` };
      case ErrorCodes.UserNotExist:
        return { content: msg || `${t('common.userNotExist')}. ${t('common.contactAdmin')}.` };
      default:
        return { content:
          msg || `${t('common.errorOccurred')}${errorCode ? `(${errorCode}).` : '.'} ${t('common.contactAdmin')}.` };
    }
  }

  function handleOnCloseNotify(): void {
    dispatch(disposeErrorCodeInNotifyAction());
  }

  const mode = useSelector((state: AppState) => state.darkMode);
  const { darkMode } = mode;

  return (
    <>
      <ErrorBoundary>
        <Navbar />
        <div className={darkMode ? 'appLayout dark' : 'appLayout'} id={darkMode ? `${styles.darkAppLayout}` : undefined}>
          <Container>
            <Dimmer active={globalStore.isLoadingCounter > 0} inverted page>
              <Loader>{t('common.loading')}</Loader>
            </Dimmer>
            {children}
          </Container>
        </div>
        <NotifyMessage
          title={globalStore.notifyState.errorCode !== 0 ?
            mapCodeError(globalStore.notifyState.errorCode).title : globalStore.notifyState.title}
          isOpen={globalStore.notifyState.isOpen}
          onCloseFunc={handleOnCloseNotify}
          negative={globalStore.notifyState.negative}
          info={!globalStore.notifyState.negative}
        >
          <div className={styles.message}>{globalStore.notifyState.text ?
            globalStore.notifyState.text :
            mapCodeError(globalStore.notifyState.errorCode, globalStore.notifyState.status).content}
          </div>
        </NotifyMessage>
        <Footer />
      </ErrorBoundary>

    </>
  );
};
export default Layout;
