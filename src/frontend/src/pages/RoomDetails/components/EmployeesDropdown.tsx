import React from 'react';
import { useTranslation } from 'react-i18next';
import { Dropdown, DropdownItemProps, DropdownProps } from 'semantic-ui-react';

type Props = {
  employeeOptions: DropdownItemProps[];
  onEmployeeChange: (event: React.SyntheticEvent<HTMLElement, Event>, data: DropdownProps) => void
  selectedEmployee: string;
  disabled: boolean;
  styles: string
}

const EmployeesDropdown: React.FC<Props> = ({ employeeOptions, onEmployeeChange, selectedEmployee, disabled, styles,
}) => {
  const { t } = useTranslation();

  return (
    <Dropdown
      text={employeeOptions.find((e) => e.key === selectedEmployee)?.text as string || t('roomDetails.selectEmployee')}
      value={selectedEmployee}
      selectOnBlur={false}
      scrolling
      fluid
      options={employeeOptions}
      onChange={onEmployeeChange}
      disabled={disabled}
      className={styles}
    />
  );
};

export default EmployeesDropdown;
