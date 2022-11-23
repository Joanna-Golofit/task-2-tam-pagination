import React, { useState } from 'react';
import { Icon, Table } from 'semantic-ui-react';
import { useTranslation } from 'react-i18next';
import { SummaryDto } from '../../../services/summary/models';
import styles from './allSummarySection.module.scss';

type Props = {
    summary: SummaryDto;
  };

const AllSummarySection: React.FC<Props> = ({ summary }) => {
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
          <Table.HeaderCell width="12">{t('summary.allEmployeesCount')}</Table.HeaderCell>
          <Table.HeaderCell>{summary.allEmployeesCount}</Table.HeaderCell>
        </Table.Row>
      </Table.Header>
      <Table.Body>
        <Table.Row>
          <Table.Cell><span className={styles.titleCell}>{t('summary.allOfficeEmployeesCount')}</span></Table.Cell>
          <Table.Cell>{summary.allOfficeEmployeesCount}</Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell><span className={styles.titleCell}>{t('summary.allHybridEmployeesCount')}</span></Table.Cell>
          <Table.Cell>{summary.allHybridEmployeesCount}</Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell><span className={styles.titleCell}>{t('summary.allRemoteEmployeesCount')}</span></Table.Cell>
          <Table.Cell>{summary.allRemoteEmployeesCount}</Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell><span className={styles.titleCell}>{t('summary.allNotSetEmployeesCount')}</span></Table.Cell>
          <Table.Cell>{summary.allNotSetEmployeesCount}</Table.Cell>
        </Table.Row>
        <Table.Row onClick={() => expandRowClickHandler('assigned')} className={styles.clickableRow}>
          <Table.Cell>
            {expandedRowName.some((name) => name === 'assigned') ?
              <Icon name="caret down" /> : <Icon name="caret right" />}
            <span>
              {t('summary.allAssignedToDesksCount')}
            </span>
          </Table.Cell>
          <Table.Cell>{summary.allAssignedToDesksCount}</Table.Cell>
        </Table.Row>
        {expandedRowName.some((name) => name === 'assigned') && (
          <>
            <Table.Row>
              <Table.Cell>
                <span className={styles.titleSubCell}>{t('summary.allOfficeEmployeesCount')}</span>
              </Table.Cell>
              <Table.Cell>{summary.allAssignedOfficeEmployeesCount}</Table.Cell>
            </Table.Row>
            <Table.Row>
              <Table.Cell>
                <span className={styles.titleSubCell}>{t('summary.allHybridEmployeesCount')}</span>
              </Table.Cell>
              <Table.Cell>{summary.allAssignedHybridEmployeesCount}</Table.Cell>
            </Table.Row>
          </>
        )}
        <Table.Row onClick={() => expandRowClickHandler('unassigned')} className={styles.clickableRow}>
          <Table.Cell>
            {expandedRowName.some((name) => name === 'unassigned') ?
              <Icon name="caret down" /> : <Icon name="caret right" />}
            <span>
              {t('summary.allUnassignedToDesksCount')}
            </span>
          </Table.Cell>
          <Table.Cell>{summary.allUnassignedToDesksCount}</Table.Cell>
        </Table.Row>
        {expandedRowName.some((name) => name === 'unassigned') && (
        <>
          <Table.Row>
            <Table.Cell>
              <span className={styles.titleSubCell}>{t('summary.allOfficeEmployeesCount')}</span>
            </Table.Cell>
            <Table.Cell>{summary.allUnassignedOfficeEmployeesCount}</Table.Cell>
          </Table.Row>
          <Table.Row>
            <Table.Cell>
              <span className={styles.titleSubCell}>{t('summary.allHybridEmployeesCount')}</span>
            </Table.Cell>
            <Table.Cell>{summary.allUnassignedHybridEmployeesCount}</Table.Cell>
          </Table.Row>
        </>
        )}
      </Table.Body>
    </Table>
  );
};

export default AllSummarySection;
