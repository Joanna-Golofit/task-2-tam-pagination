import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import {
  Accordion,
  Icon,
} from 'semantic-ui-react';
import { EmployeeForRoomDetailsDto } from '../../services/room/models';
import styles from './DeskRow.module.scss';

type ComponentProps = {
  employee: EmployeeForRoomDetailsDto;
};

const EmployeeProjectsAccordion: React.FC<ComponentProps> = ({
  employee,
}) => {
  const { t } = useTranslation();
  const [isActive, setIsActive] = useState(false);

  return (
    <Accordion
      fluid
      className={`${!isActive ? styles.accordion : styles.accordionOpen}`}
    >
      <Accordion.Title
        active={isActive}
        onClick={() => { setIsActive((prevState) => !prevState); }}
      >
        <Icon name="dropdown" />
        {t('roomDetails.showTeams')}
      </Accordion.Title>
      <Accordion.Content
        active={isActive}
      >
        {employee.projectsNames.map((name: string) => (
          <p
            key={`project_key_${name}`}
            className={styles.menuitem}
          >{name}
          </p>
        ))}
      </Accordion.Content>
    </Accordion>
  );
};

export default EmployeeProjectsAccordion;
