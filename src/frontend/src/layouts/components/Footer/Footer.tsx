import React from 'react';
import { useSelector } from 'react-redux';
import { AppState } from '../../../store';
import styles from './Footer.module.scss';

const Footer: React.FC = () => {
  const mode = useSelector((state: AppState) => state.darkMode);

  const { darkMode } = mode;

  return (
    <div className={styles.footer} id={darkMode ? `${styles.darkFooter}` : undefined}>
      Teams Allocation Manager v{process.env.REACT_APP_VERSION ?? ' locally'}
    </div>
  );
};

export default Footer;
