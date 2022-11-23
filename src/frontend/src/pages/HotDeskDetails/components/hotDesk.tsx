import React from 'react';
import { useTranslation } from 'react-i18next';
import { Button, Statistic, Table } from 'semantic-ui-react';

type ComponentProps = {
    deskNumber: number;
    openCalendar: () => void,
  };

const HotDeskRow: React.FC<ComponentProps> = ({ deskNumber, openCalendar }) => {
  const { t } = useTranslation();

  return (
    <Table.Row>
      <Table.Cell>
        <Statistic size="tiny" color="black">
          <Statistic.Value>{deskNumber}</Statistic.Value>
        </Statistic>
      </Table.Cell>

      <Table.Cell textAlign="left">
        <div>{t('hotDesks.hotDesk')}</div>
      </Table.Cell>
      <Table.Cell textAlign="left">
        <Button primary size="tiny" onClick={openCalendar}>{t('hotDesks.reservationBtn')}</Button>
      </Table.Cell>
    </Table.Row>
  );
};

export default HotDeskRow;
