import React from 'react';
import { Modal, Button } from 'semantic-ui-react';
import { useTranslation } from 'react-i18next';
import styles from './SimpleModal.module.scss';

type Props = {
  isOpen: boolean;
  text: string;
  closeFunction: ()=>void;
  yesOkFunction?: (()=>void) | undefined;
  isOkBtnOnly?: boolean;
};

const SimpleModal: React.FC<Props> = ({ isOpen, text, closeFunction,
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
    >
      <Modal.Header>{text}</Modal.Header>
      <Modal.Actions>
        {!isOkBtnOnly && <Button onClick={handleClickCloseButton} content={t('common.no')} />}
        <Button onClick={handleClickYesOkButton} content={isOkBtnOnly ? t('common.ok') : t('common.yes')} />
      </Modal.Actions>
    </Modal>
  );
};

export default SimpleModal;
