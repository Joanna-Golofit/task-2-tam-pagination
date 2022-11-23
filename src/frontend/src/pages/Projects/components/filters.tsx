import React, { useEffect, useState } from 'react';
import {
  Form,
  Button,
  Grid,
  GridColumnProps,
  DropdownItemProps,
  DropdownProps,
  Checkbox,
} from 'semantic-ui-react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { GRID_COLUMN_COMPUTER, GRID_COLUMN_MOBILE, GRID_COLUMN_TABLET } from '../../../globalConstants';
import { ProjectsFilterOptions } from '../../../services/project/models';
import getAllProjectsForDropdownApiService from '../../../services/project/getAllProjectsForDropdownApiService';
import getTeamLeadersApiService from '../../../services/employee/getTeamLeadersApiService';
import styles from './filters.module.scss';
import { AppState } from '../../../store';

type ComponentProps = {
  onFilter: (filtersArgs: ProjectsFilterOptions) => void
  filtersState: ProjectsFilterOptions
}

const Filters: React.FC<ComponentProps> = ({ onFilter, filtersState }) => {
  const { t } = useTranslation();
  const { loggedUserData } = useSelector((state: AppState) => state.global);
  const [projectOptions, setProjectOptions] = useState<DropdownItemProps[]>([]);
  const [teamLeaderOptions, setTeamLeaderOptions] = useState<DropdownItemProps[]>([]);
  const [isFetchingProjects, setIsFetchingProjects] = useState(false);
  const [isFetchingTeamLeaders, setIsFetchingTeamLeaders] = useState(false);

  useEffect(() => {
    onFilter({ showYourProjects: true, teamLeaderIds: loggedUserData.isTeamLeader() ? [loggedUserData.id] : [] });
  }, [loggedUserData.id]);

  useEffect(() => {
    setIsFetchingProjects(true);
    // TODO: should be refactored to use redux-observable flow
    getAllProjectsForDropdownApiService().subscribe((returnProjects) => {
      setProjectOptions([
        ...returnProjects.map((p) => ({
          key: p.id,
          text: p.name,
          value: p.id,
        })),
      ]);
      setIsFetchingProjects(false);
    });
    setIsFetchingTeamLeaders(true);
    // TODO: should be refactored to use redux-observable flow
    getTeamLeadersApiService().subscribe((returnTeamLeaders) => {
      const options = returnTeamLeaders.map((tl) => ({
        key: tl.id,
        text: `${tl.name} ${tl.surname}`,
        value: tl.id,
      }));

      setTeamLeaderOptions(options);
      setIsFetchingTeamLeaders(false);
    });
  }, []);

  const columnCommonProps: GridColumnProps = {
    mobile: GRID_COLUMN_MOBILE, tablet: GRID_COLUMN_TABLET, computer: GRID_COLUMN_COMPUTER };

  const resetFilters = () => {
    onFilter({
      teamLeaderIds: [],
      projectIds: [],
      showYourProjects: false });
  };

  const onProjectChange = (_: any, data: DropdownProps) => {
    onFilter({
      showYourProjects: (data.value as string[]).length === 0 &&
      filtersState?.teamLeaderIds![0] === loggedUserData.id,
      projectIds: data.value as string[] });
  };

  const changeCheckboxHandler = () => {
    onFilter({
      teamLeaderIds: !filtersState.showYourProjects ? [loggedUserData.id as string] : [],
      projectIds: [],
      showYourProjects: !filtersState.showYourProjects });
  };

  const changeTeamLeadersHandler = (_: any, data: DropdownProps) => {
    onFilter({
      teamLeaderIds: data.value as string[],
      showYourProjects: filtersState.projectIds?.length === 0 &&
      (data.value as string[]).length === 1 && (data.value as string[])[0] === loggedUserData.id });
  };

  return (
    <Form>
      <Grid padded="vertically">
        <Grid.Row columns={2}>
          <Grid.Column {...columnCommonProps}>
            <b>{t('projects.leader')}</b>
            <Form.Select
              name="teamLeaderIds"
              clearable
              multiple
              search
              selection
              options={teamLeaderOptions}
              loading={isFetchingTeamLeaders}
              onChange={changeTeamLeadersHandler}
              value={filtersState.teamLeaderIds as string[]}
              noResultsMessage={t('common.noResultsFilters')}
            />
          </Grid.Column>
          <Grid.Column {...columnCommonProps}>
            <b>{t('projects.header')}</b>
            <Form.Select
              name="projectIds"
              clearable
              multiple
              search
              selection
              options={projectOptions}
              loading={isFetchingProjects}
              closeOnChange
              onChange={onProjectChange}
              value={filtersState.projectIds as string[]}
              noResultsMessage={t('common.noResultsFilters')}
            />
          </Grid.Column>
        </Grid.Row>
        <Grid.Row columns={1} textAlign="right">
          {loggedUserData.isTeamLeader() && (
          <Grid.Column>
            <div className={styles.checkbox}>
              <p>{t('projects.showMyTeams')}</p>
              <Checkbox checked={filtersState.showYourProjects} onChange={changeCheckboxHandler} />
            </div>
          </Grid.Column>
          )}
        </Grid.Row>
        <Grid.Row columns={1} textAlign="right">
          <Grid.Column>
            <Button type="reset" onClick={resetFilters}>{t('common.clearFilters')}</Button>
          </Grid.Column>
        </Grid.Row>
      </Grid>
    </Form>
  );
};

export default Filters;
