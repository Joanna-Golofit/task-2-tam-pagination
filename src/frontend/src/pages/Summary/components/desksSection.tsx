import React from 'react';
import { Table } from 'semantic-ui-react';
import { useTranslation } from 'react-i18next';
import { SummaryDto } from '../../../services/summary/models';
import styles from './fpSummarySection.module.scss';

type Props = {
    summary: SummaryDto;
  };

const DesksSection: React.FC<Props> = ({ summary }) => {
  const { t } = useTranslation();

  return (
    <Table compact unstackable color="orange">
      <Table.Header>
        <Table.Row>
          <Table.HeaderCell width="12">{t('summary.desks')}</Table.HeaderCell>
          <Table.HeaderCell>{summary.desksCount}</Table.HeaderCell>
        </Table.Row>
      </Table.Header>
      <Table.Body>
        <Table.Row>
          <Table.Cell><span className={styles.titleCell}>{t('summary.freeDesks')}</span></Table.Cell>
          <Table.Cell>{summary.freeDesksCount}</Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell><span className={styles.titleCell}>{t('summary.occupiedDesks')}</span></Table.Cell>
          <Table.Cell>{summary.occupiedDesksCount}</Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell><span className={styles.titleCell}>{t('summary.hotDesks')}</span></Table.Cell>
          <Table.Cell>{summary.hotDesksCount}</Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell><span className={styles.titleCell}>{t('summary.disabledDesks')}</span></Table.Cell>
          <Table.Cell>{summary.desksCount - summary.freeDesksCount - summary.occupiedDesksCount - summary.hotDesksCount}</Table.Cell>
        </Table.Row>
      </Table.Body>
    </Table>
  );
};

export default DesksSection;
