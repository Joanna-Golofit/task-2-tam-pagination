import React, { useState, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { Search } from 'semantic-ui-react';
import { useSelector } from 'react-redux';
import { AppState } from '../../../store';
import { User } from '../../../services/user/models';
import styles from './reservationModal.module.scss';

function escapeRegExp(text: string) {
  return text.replace(/[-/\\^$*+?.()|[\]{}]/g, '\\$&');
}

type ComponentProps = {
    employeeSelectedCallback: (selectedEmployeeId: string) => void
};

const EmployeesSearch: React.FC<ComponentProps> = ({ employeeSelectedCallback }) => {
  const { t } = useTranslation();
  const { users } = useSelector((state: AppState) => state.users);
  const [searchResult, setSearchResult] = useState([] as User[]);
  const timeoutRef = useRef(null as any);

  const handleSearchChange = (_: any, data: any) => {
    clearTimeout(timeoutRef.current);
    timeoutRef.current = setTimeout(() => {
      const searchRegex = new RegExp(escapeRegExp(data.value), 'i');
      const filteredUsers = users.filter((user) => (searchRegex.test(user.title) || searchRegex.test(user.description)));
      setSearchResult(filteredUsers);
    }, 300);
  };

  const resultRenderer = ({ title, description, workspace }: any) => (
    <div key="content" className="content">
      {title && <div className="title">{title}</div>}
      {description && <div className="description">{description}</div>}
      <div>
        {workspace && <div className="price">{workspace}</div>}
      </div>
    </div>
  );

  return (
    <Search
      className={styles.search}
      placeholder={t('usersSearch.searchPerson')}
      onResultSelect={(_, data) => employeeSelectedCallback(data.result.id)}
      onSearchChange={handleSearchChange}
      results={searchResult}
      resultRenderer={resultRenderer}
      noResultsMessage={t('common.noResultsFilters')}
    />
  );
};

export default EmployeesSearch;
