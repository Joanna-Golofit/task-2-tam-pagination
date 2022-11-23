import React from 'react';
import { Table } from 'semantic-ui-react';
import { useTranslation } from 'react-i18next';
import { SummaryDto } from '../../../services/summary/models';

type Props = {
    summary: SummaryDto;
  };

const OverallSection: React.FC<Props> = ({ summary }) => {
  const { t } = useTranslation();

  return (
    <Table compact unstackable color="orange">
      <Table.Header>
        <Table.Row>
          <Table.HeaderCell width={12}><span>{t('summary.teamsCount')}</span></Table.HeaderCell>
          <Table.HeaderCell>{summary.projectsCount}</Table.HeaderCell>
        </Table.Row>
      </Table.Header>
    </Table>
  );
};

export default OverallSection;
