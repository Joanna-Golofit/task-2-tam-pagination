import React, { useState } from 'react';
import { Table, Button } from 'semantic-ui-react';
import { useTranslation } from 'react-i18next';
import styles from './DeskHistoryPeek.module.scss';
import { DeskHistoryDto } from '../../services/room/models';
import ModalWithContent from '../../layouts/components/ModalWithContent';

type ComponentProps = {
  deskNumber: number;
  deskHistory: DeskHistoryDto[];
  employeeDropdown: boolean;
};

const DeskHistoryPeek: React.FC<ComponentProps> = ({ deskHistory, deskNumber, employeeDropdown }) => {
  const { t } = useTranslation();

  const [historyModal, setHistoryModal] = useState({ isOpen: false, title: '', body: <></> });

  const historyTableContent = (
    deskHistory.length !== 0 ?
      deskHistory.map((d) => (
        <Table.Row key={d.id} textAlign="center">
          <Table.Cell>
            {`${d.employeeName} ${d.employeeSurname}`}
          </Table.Cell>
          <Table.Cell>
            {new Date(d.from).toLocaleString('pl-PL')}
          </Table.Cell>
          <Table.Cell>
            {d.to !== null ? new Date(d.to).toLocaleString('pl-PL') : t('roomDetails.now')}
          </Table.Cell>
        </Table.Row>
      )) :
      (
        <Table.Row textAlign="center" colSpan="3">
          <Table.Cell colSpan="3">
            {t('roomDetails.noRecords')}
          </Table.Cell>
        </Table.Row>
      )
  );

  const historyTable = (
    <Table color="brown" stackable striped className={styles.historyModalTable}>
      <Table.Header className={styles.historyModalHead}>
        <Table.Row>
          <Table.HeaderCell textAlign="center" width={1}>
            {t('roomDetails.historyWho')}
          </Table.HeaderCell>
          <Table.HeaderCell textAlign="center" width={1}>
            {t('roomDetails.historyFrom')}
          </Table.HeaderCell>
          <Table.HeaderCell textAlign="center" width={1}>
            {t('roomDetails.historyTo')}
          </Table.HeaderCell>
        </Table.Row>
      </Table.Header>
      <Table.Body>
        {historyTableContent}
      </Table.Body>
    </Table>
  );

  const historyButtonHandler = () => {
    setHistoryModal({
      isOpen: true,
      title: `${t('roomDetails.historyTitle')} ${deskNumber}`,
      body: historyTable });
  };

  return (
    <>
      <div className={employeeDropdown ? `${styles.buttonUp}` : ''}>
        <Button
          icon="history"
          color="blue"
          basic
          circular
          size="tiny"
          onClick={historyButtonHandler}
          title={t('roomDetails.deskHistory')}
        />
      </div>
      <ModalWithContent
        isOpen={historyModal.isOpen}
        title={historyModal.title}
        body={historyModal.body}
        closeFunction={() => {
          setHistoryModal((prevState) => ({ ...prevState, isOpen: false }));
        }}
        isOkBtnOnly
      />
    </>
  );
};

export default DeskHistoryPeek;
