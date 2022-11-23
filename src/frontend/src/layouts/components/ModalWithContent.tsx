import React from 'react';
import { Modal, Button } from 'semantic-ui-react';
import { useTranslation } from 'react-i18next';
import styles from './ModalWithContent.module.scss';

type Props = {
  isOpen: boolean;
  title: string;
  body: any;
  closeFunction: ()=>void;
  yesOkFunction?: (()=>void) | undefined;
  isOkBtnOnly?: boolean;
};

const ModalWithContent: React.FC<Props> = ({ isOpen, title, body, closeFunction,
  yesOkFunction = undefined, isOkBtnOnly = false }) => {
  const { t } = useTranslation();

  function handleClickCloseButton() {
    if (closeFunction) closeFunction();
  }

  function handleClickYesOkButton() {
    if (yesOkFunction) yesOkFunction();
    else handleClickCloseButton();
  }

  return (
    <Modal
      open={isOpen}
      closeOnEscape={false}
      closeOnDimmerClick={false}
      size="small"
      id={styles.modal}
      className={styles.modalContent}
    >
      <Modal.Header>{title}</Modal.Header>
      <Modal.Content>{body}</Modal.Content>
      <Modal.Actions>
        {!isOkBtnOnly && <Button onClick={handleClickCloseButton} content={t('common.no')} />}
        <Button onClick={handleClickYesOkButton} content={isOkBtnOnly ? t('common.ok') : t('common.yes')} />
      </Modal.Actions>
    </Modal>
  );
};

export default ModalWithContent;
