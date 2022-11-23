import React from 'react';
import { useTranslation } from 'react-i18next';
import { Statistic, Table } from 'semantic-ui-react';

type ComponentProps = {
    deskNumber: number;
  };

const RegularDesk: React.FC<ComponentProps> = ({ deskNumber }) => {
  const { t } = useTranslation();

  return (
    <Table.Row>
      <Table.Cell>
        <Statistic size="tiny" color="black">
          <Statistic.Value>{deskNumber}</Statistic.Value>
        </Statistic>
      </Table.Cell>

      <Table.Cell textAlign="left" disabled>
        <div>{t('hotDesks.deskTaken')}</div>
      </Table.Cell>
      <Table.Cell width={2} />
    </Table.Row>
  );
};

export default RegularDesk;
