import React, { useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { Header, Segment, Icon, Divider, Grid, Button, GridColumn, GridRow } from 'semantic-ui-react';
import { AppState } from '../../store';
import { importBubblesAction } from '../../store/admin/actions';
import styles from './admin.module.scss';

const Admin: React.FC = () => {
  const { t } = useTranslation();
  const dispatch = useDispatch();
  const { isImportPending } = useSelector((state: AppState) => state.admin);
  const { loggedUserData } = useSelector((state: AppState) => state.global);

  useEffect(() => {
    document.title = `${t('common.tam')} - ${t('admin.adminPanel')}`;
  }, [t]);

  function handleImportFromBubblesButton(): void {
    dispatch(importBubblesAction());
  }

  return (loggedUserData.isUserAdmin() ?
    (
      <Segment basic>
        <Header as="h1">
          <span className={styles.pageTitle}>
            <Icon color="green" name="cogs" size="small" />{t('admin.adminPanel')}
          </span>
        </Header>
        <Divider />
        <Grid columns={2}>
          <GridRow id={styles.row}>
            <GridColumn textAlign="left">
              <span className={styles.optionName}>{t('admin.importFromBubbles')}</span>
            </GridColumn>
            <GridColumn textAlign="left">
              <Button
                className={styles.uploadBtn}
                color="green"
                basic
                circular
                icon="refresh"
                onClick={handleImportFromBubblesButton}
                loading={isImportPending}
              />
            </GridColumn>
          </GridRow>
          <Divider />
        </Grid>
      </Segment>
    ) : (<span>{t('admin.accessDenied')}</span>)
  );
};

export default Admin;
