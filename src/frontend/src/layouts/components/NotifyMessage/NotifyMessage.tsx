import React from 'react';
import { Message, TransitionablePortal } from 'semantic-ui-react';
import styles from './NotifyMessage.module.scss';

type Props = {
  isOpen: boolean;
  onCloseFunc: () => void;
  info?: boolean;
  negative?: boolean;
  positive?: boolean;
  success?: boolean;
  warning?: boolean;
  title?: string;
};

const NotifyMessage: React.FC<Props> = ({ children, isOpen, onCloseFunc, info = false,
  negative = false, positive = false, success = false, warning = false, title = '' }) => {
  function hanldeOnClose(): void {
    if (onCloseFunc) onCloseFunc();
  }

  return (
    <TransitionablePortal
      open={isOpen}
      transition={{ animation: 'fly left', duration: 1500 }}
      onClose={hanldeOnClose}
    >
      <Message
        info={info}
        negative={negative}
        positive={positive}
        success={success}
        warning={warning}
        size="large"
        className={styles.notifyDiv}
      >
        <Message.Header>{title}</Message.Header>
        {children}
      </Message>
    </TransitionablePortal>
  );
};

export default NotifyMessage;
