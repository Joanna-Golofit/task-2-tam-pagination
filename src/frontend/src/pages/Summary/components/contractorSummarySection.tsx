import React, { useState } from 'react';
import { Icon, Table } from 'semantic-ui-react';
import { useTranslation } from 'react-i18next';
import { SummaryDto } from '../../../services/summary/models';
import styles from './contractorSummarySection.module.scss';

type Props = {
    summary: SummaryDto;
  };

const ContractorSummarySection: React.FC<Props> = ({ summary }) => {
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
          <Table.HeaderCell width="12">{t('summary.contractorEmployeesCount')}</Table.HeaderCell>
          <Table.HeaderCell>{summary.contractorEmployeesCount}</Table.HeaderCell>
        </Table.Row>
      </Table.Header>
      <Table.Body>
        <Table.Row>
          <Table.Cell>
            <span className={styles.titleCell}>{t('summary.contractorOfficeEmployeesCount')}</span>
          </Table.Cell>
          <Table.Cell>{summary.contractorOfficeEmployeesCount}</Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell>
            <span className={styles.titleCell}>{t('summary.contractorHybridEmployeesCount')}</span>
          </Table.Cell>
          <Table.Cell>{summary.contractorHybridEmployeesCount}</Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell>
            <span className={styles.titleCell}>{t('summary.contractorRemoteEmployeesCount')}</span>
          </Table.Cell>
          <Table.Cell>{summary.contractorRemoteEmployeesCount}</Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell>
            <span className={styles.titleCell}>{t('summary.contractorNotSetEmployeesCount')}</span>
          </Table.Cell>
          <Table.Cell>{summary.contractorNotSetEmployeesCount}</Table.Cell>
        </Table.Row>
        <Table.Row onClick={() => expandRowClickHandler('assigned')} className={styles.clickableRow}>
          <Table.Cell>
            {expandedRowName.some((name) => name === 'assigned') ?
              <Icon name="caret down" /> : <Icon name="caret right" />}
            <span>
              {t('summary.contractorAssignedToDesksCount')}
            </span>
          </Table.Cell>
          <Table.Cell>{summary.contractorAssignedToDesksCount}</Table.Cell>
        </Table.Row>
        {expandedRowName.some((name) => name === 'assigned') && (
        <>
          <Table.Row>
            <Table.Cell>
              <span className={styles.titleSubCell}>{t('summary.contractorOfficeEmployeesCount')}</span>
            </Table.Cell>
            <Table.Cell>{summary.contractorAssignedOfficeEmployeesCount}</Table.Cell>
          </Table.Row>
          <Table.Row>
            <Table.Cell>
              <span className={styles.titleSubCell}>{t('summary.contractorHybridEmployeesCount')}</span>
            </Table.Cell>
            <Table.Cell>{summary.contractorAssignedHybridEmployeesCount}</Table.Cell>
          </Table.Row>
        </>
        )}
        <Table.Row onClick={() => expandRowClickHandler('unassigned')} className={styles.clickableRow}>
          <Table.Cell>
            {expandedRowName.some((name) => name === 'unassigned') ?
              <Icon name="caret down" /> : <Icon name="caret right" />}
            <span>
              {t('summary.contractorUnassignedToDesksCount')}
            </span>
          </Table.Cell>
          <Table.Cell>{summary.contractorUnassignedToDesksCount}</Table.Cell>
        </Table.Row>
        {expandedRowName.some((name) => name === 'unassigned') && (
        <>
          <Table.Row>
            <Table.Cell>
              <span className={styles.titleSubCell}>{t('summary.contractorOfficeEmployeesCount')}</span>
            </Table.Cell>
            <Table.Cell>{summary.contractorUnassignedOfficeEmployeesCount}</Table.Cell>
          </Table.Row>
          <Table.Row>
            <Table.Cell>
              <span className={styles.titleSubCell}>{t('summary.contractorHybridEmployeesCount')}</span>
            </Table.Cell>
            <Table.Cell>{summary.contractorUnassignedHybridEmployeesCount}</Table.Cell>
          </Table.Row>
        </>
        )}
      </Table.Body>
    </Table>
  );
};

export default ContractorSummarySection;
