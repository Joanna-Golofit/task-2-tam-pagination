import React from 'react';
import { Icon, Label } from 'semantic-ui-react';
import styles from './styles.module.scss';

type Props = {
    email: string;
  };

const EmailLabel: React.FC<Props> = ({ email }) => (
  <Label href={`mailto:${email}`}>
    <Icon color="blue" name="mail" />
    <Label.Detail>
      <span className={styles.email}>{email}</span>
    </Label.Detail>
  </Label>
);

export default EmailLabel;
