import React, { useState } from 'react';
import { Icon, Table } from 'semantic-ui-react';
import { useTranslation } from 'react-i18next';
import { SummaryDto } from '../../../services/summary/models';
import styles from './fpSummarySection.module.scss';

type Props = {
    summary: SummaryDto;
  };

const FpSummarySection: React.FC<Props> = ({ summary }) => {
  const { t } = useTranslation();

  const [expandedRowName, setExpandedRowName] = useState([] as string[]);

  const expandRowClickHandler = (rowName: string) => {
    if (expandedRowName.some((name) => name === rowName)) {
      setExpandedRowName((prevState) => prevState.filter((name) => name !== rowName));
    } else setExpandedRowName((prevState) => [...prevState, rowName]);
  };

  return (
    <Table compact unstackable color="orange">
      <Table.Header>
        <Table.Row>
          <Table.HeaderCell width="12">{t('summary.fpEmployeesCount')}</Table.HeaderCell>
          <Table.HeaderCell>{summary.fpEmployeesCount}</Table.HeaderCell>
        </Table.Row>
      </Table.Header>
      <Table.Body>
        <Table.Row>
          <Table.Cell><span className={styles.titleCell}>{t('summary.fpOfficeEmployeesCount')}</span></Table.Cell>
          <Table.Cell>{summary.fpOfficeEmployeesCount}</Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell><span className={styles.titleCell}>{t('summary.fpHybridEmployeesCount')}</span></Table.Cell>
          <Table.Cell>{summary.fpHybridEmployeesCount}</Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell><span className={styles.titleCell}>{t('summary.fpRemoteEmployeesCount')}</span></Table.Cell>
          <Table.Cell>{summary.fpRemoteEmployeesCount}</Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell><span className={styles.titleCell}>{t('summary.fpNotSetEmployeesCount')}</span></Table.Cell>
          <Table.Cell>{summary.fpNotSetEmployeesCount}</Table.Cell>
        </Table.Row>
        <Table.Row onClick={() => expandRowClickHandler('assigned')} className={styles.clickableRow}>
          <Table.Cell>
            {expandedRowName.some((name) => name === 'assigned') ?
              <Icon name="caret down" /> : <Icon name="caret right" />}
            <span>
              {t('summary.fpAssignedToDesksCount')}
            </span>
          </Table.Cell>
          <Table.Cell>{summary.fpAssignedToDesksCount}</Table.Cell>
        </Table.Row>
        {expandedRowName.some((name) => name === 'assigned') && (
        <>
          <Table.Row>
            <Table.Cell>
              <span className={styles.titleSubCell}>{t('summary.fpOfficeEmployeesCount')}</span>
            </Table.Cell>
            <Table.Cell>{summary.fpAssignedOfficeEmployeesCount}</Table.Cell>
          </Table.Row>
          <Table.Row>
            <Table.Cell>
              <span className={styles.titleSubCell}>{t('summary.fpHybridEmployeesCount')}</span>
            </Table.Cell>
            <Table.Cell>{summary.fpAssignedHybridEmployeesCount}</Table.Cell>
          </Table.Row>
        </>
        )}
        <Table.Row onClick={() => expandRowClickHandler('unassigned')} className={styles.clickableRow}>
          <Table.Cell>
            {expandedRowName.some((name) => name === 'unassigned') ?
              <Icon name="caret down" /> : <Icon name="caret right" />}
            <span>
              {t('summary.fpUnassignedToDesksCount')}
            </span>
          </Table.Cell>
          <Table.Cell>{summary.fpUnassignedToDesksCount}</Table.Cell>
        </Table.Row>
        {expandedRowName.some((name) => name === 'unassigned') && (
        <>
          <Table.Row>
            <Table.Cell>
              <span className={styles.titleSubCell}>{t('summary.fpOfficeEmployeesCount')}</span>
            </Table.Cell>
            <Table.Cell>{summary.fpUnassignedOfficeEmployeesCount}</Table.Cell>
          </Table.Row>
          <Table.Row>
            <Table.Cell>
              <span className={styles.titleSubCell}>{t('summary.fpHybridEmployeesCount')}</span>
            </Table.Cell>
            <Table.Cell>{summary.fpUnassignedHybridEmployeesCount}</Table.Cell>
          </Table.Row>
        </>
        )}
      </Table.Body>
    </Table>
  );
};

export default FpSummarySection;
