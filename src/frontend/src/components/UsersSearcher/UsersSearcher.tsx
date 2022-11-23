import React, { useEffect, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { useHistory } from 'react-router-dom';
import { Search } from 'semantic-ui-react';
import { useDispatch, useSelector } from 'react-redux';
import { Routes } from '../../Routes';
import { AppState } from '../../store';
import {
  finishSearch, getAllUsersForSearcher,
  setLoadingUsersSearcher, startSearch, updateSelection,
} from '../../store/users/actions';
import styles from './UserSearcher.module.scss';

function escapeRegExp(text: string) {
  return text.replace(/[-/\\^$*+?.()|[\]{}]/g, '\\$&');
}

const UsersSearcher: React.FC = () => {
  const history = useHistory();
  const dispatch = useDispatch();
  const { t } = useTranslation();
  const { loading, results, value, users } = useSelector((state: AppState) => state.users);

  const timeoutRef = useRef(null as any);

  const projectTranslate = t('usersSearch.projects');

  const handleSearchChange = (_: any, data: any) => {
    clearTimeout(timeoutRef.current);
    dispatch(startSearch(data.value));
    timeoutRef.current = setTimeout(() => {
      const re = new RegExp(escapeRegExp(data.value), 'i');

      dispatch(finishSearch(users.filter((i) => (re.test(i.title) ||
        re.test(i.description)))));
    }, 300);
  };

  useEffect(() => {
    dispatch(setLoadingUsersSearcher(true));
    dispatch(getAllUsersForSearcher());
    clearTimeout(timeoutRef.current);
  }, []);

  const resultRenderer = ({ title, description, workspace, projects }: any) => {
    const projectLabel = `${projectTranslate}: ${projects.length}`;
    return (
      <div key="content" className="content">
        {title && <div className="title">{title}</div>}
        {description && <div className="description">{description}</div>}
        <div className={styles.flex}>
          {projects.length > 0 && (
            <div className={`${styles.project}`} title={projects.join('\r\n')}>{projectLabel}</div>
          )}
          {workspace && <div className={`price ${styles.alignRight}`}>{workspace}</div>}
        </div>
      </div>
    );
  };

  return (
    <Search
      placeholder={t('usersSearch.searchPerson')}
      loading={loading}
      onResultSelect={(e, data) => {
        dispatch(updateSelection(''));
        history.push(`${Routes.Users}/${data.result.id}`);
      }}
      onSearchChange={handleSearchChange}
      results={results}
      value={value}
      resultRenderer={resultRenderer}
      noResultsMessage={t('common.noResultsFilters')}
      size="tiny"
      className={styles.search}
    />
  );
};

export default UsersSearcher;
