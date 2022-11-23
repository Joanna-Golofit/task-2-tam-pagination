import React, { useState, useEffect } from 'react';
import { Button, Checkbox, Grid, Popup } from 'semantic-ui-react';
import { useSelector } from 'react-redux';
import { useTranslation } from 'react-i18next';
import { SharedDeskDto } from '../../services/room/models';
import { AppState } from '../../store';
import styles from './weekPopup.module.scss';

type ComponentProps = {
  disabled: boolean,
  disabledDays: number[],
  selectedDays: number[],
  onCloseAction: (days: number[]) => void
}

const WeekPopup: React.FC<ComponentProps> = ({ disabled, disabledDays, selectedDays, onCloseAction }) => {
  const [active, setActive] = useState(false);
  const [days, setDays] = useState([] as SharedDeskDto[]);
  const { loggedUserData } = useSelector((state: AppState) => state.global);
  const { t } = useTranslation();

  useEffect(() => {
    const daysOfWeek = [
      { label: t('weekDays.mondayShort'), value: 1, disabled: false, selected: false },
      { label: t('weekDays.tuesdayShort'), value: 2, disabled: false, selected: false },
      { label: t('weekDays.wednesdayShort'), value: 3, disabled: false, selected: false },
      { label: t('weekDays.thursdayShort'), value: 4, disabled: false, selected: false },
      { label: t('weekDays.fridayShort'), value: 5, disabled: false, selected: false },
    ];

    const result = daysOfWeek
      .map((c) => (disabledDays.includes(c.value) ? { ...c, disabled: true } : c))
      .map((c) => (selectedDays.includes(c.value) ? { ...c, selected: true } : c));

    setDays(result);
  }, [disabledDays, selectedDays]);

  const button = (
    <Button
      icon="calendar"
      title={t('roomDetails.sharedDesk')}
      color={disabled ? 'grey' : 'blue'}
      basic
      circular
      disabled={disabled}
      size="tiny"
      id="blue"
    />
  );

  const onOpen = () => setActive(true);

  const onClose = () => {
    setActive(false);
    onCloseAction(days.filter((d) => d.selected).map((c) => c.value));
  };

  const clickedLastAvailableDayOnList = (currentDays: SharedDeskDto[], clickedDay: SharedDeskDto): boolean =>
    (currentDays.filter((d) => d.selected).length <= 1) && (currentDays.some((d) => d.selected && d.value === clickedDay.value));

  const handleOnClick = (value: number) => {
    const newDays = [...days];
    const dayToChange = newDays.find((c) => c.value === value);

    if (!dayToChange || clickedLastAvailableDayOnList(newDays, dayToChange)) {
      return;
    }

    dayToChange.selected = !dayToChange.selected;

    setDays(newDays);
  };

  return (
    <Popup
      trigger={button}
      flowing
      position="top center"
      closeOnTriggerMouseLeave={false}
      openOnTriggerMouseEnter={false}
      open={active}
      onClose={onClose}
      onOpen={onOpen}
      popperModifiers={{ preventOverflow: { boundariesElement: 'window' } }}
    >
      <Grid centered divided columns={5}>
        {days.map((c) => (
          <Grid.Column key={c.value}>
            <Checkbox
              className={(loggedUserData.isStandardUser() || c.disabled) ? styles.disabled : ''}
              checked={c.selected}
              disabled={loggedUserData.isStandardUser() || c.disabled}
              onChange={() => handleOnClick(c.value)}
              label={c.label}
            />
          </Grid.Column>
        ))}
      </Grid>
    </Popup>
  );
};

export default WeekPopup;
