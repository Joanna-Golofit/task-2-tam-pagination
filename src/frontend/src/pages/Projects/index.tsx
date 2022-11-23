import React, { useEffect, useState } from 'react';
import { Header, Table, Button, Grid, Segment, TableCell, Responsive } from 'semantic-ui-react';
import { useSelector, useDispatch } from 'react-redux';
import { useTranslation } from 'react-i18next';
import { useHistory, useLocation } from 'react-router-dom';
import { AppState } from '../../store';
import { fetchProjects } from '../../store/projects/actions';
import styles from './projects.module.scss';
import { LAPTOP_MEDIA_WIDTH } from '../../globalConstants';
import { setLoadingAction } from '../../store/global/actions';

const Projects = () => {
  const columnCount = 8;
  const history = useHistory();
  const location = useLocation();
  const { t } = useTranslation();
  const dispatch = useDispatch();

  const projectsState = useSelector((state: AppState) => state.projects);
  const { projects, projectsCount } = projectsState;
  const [visible, setVisible] = useState(false);

  useEffect(() => {
    document.title = `${t('common.tam')} - ${t('projects.header')}`;
    dispatch(setLoadingAction(true));
    dispatch(fetchProjects());
  }, [t]);

  const navigateToProject = (id: string) => {
    history.push(`${location.pathname}/${id}`);
  };

  const rows = projects?.map((project) => (
    <Table.Row
      key={project.id}
      className={`${styles.projectRow}`}
      onClick={() => navigateToProject(project.id)}
    >
      <Table.Cell>{project.name}</Table.Cell>
      <Table.Cell>
        {project.teamLeaders.length !== 0 ?
          project.teamLeaders.map(({ name, surname }) => `${name} ${surname}`).join(', ') :
          <b><i>{t('common.noTeamleader')}</i></b>}
      </Table.Cell>
      <Table.Cell>{project.peopleCount}</Table.Cell>
      <Table.Cell>{project.officeEmployeesCount}</Table.Cell>
      <Table.Cell>{project.hybridEmployeesCount}</Table.Cell>
      <Table.Cell>{project.remoteEmployeesCount}</Table.Cell>
      <Table.Cell>{project.notSetMembersCount}</Table.Cell>
      <Table.Cell>{project.unassignedMembersCount}</Table.Cell>
    </Table.Row>
  ));

  return (
    <>
      <Segment basic>
        <Grid columns={2} padded="vertically">
          <Grid.Row verticalAlign="middle">
            <Grid.Column>
              <Header as="h1">{t('projects.header')}</Header>
            </Grid.Column>
            <Grid.Column textAlign="right">
              <Button onClick={() => setVisible(!visible)}>
                {visible ? t('common.hideFilters') : t('common.showFilters')}
              </Button>
            </Grid.Column>
          </Grid.Row>
        </Grid>
        <Table compact unstackable color="orange">
          <Table.Header>
            {/* Show long header labels if screen width bigger than tablet */}
            <Responsive as={Table.Row} minWidth={LAPTOP_MEDIA_WIDTH}>
              <Table.HeaderCell>{t('projects.name')}</Table.HeaderCell>
              <Table.HeaderCell>{t('projects.leader')}</Table.HeaderCell>
              <Table.HeaderCell>{t('projects.assignedPeople')}</Table.HeaderCell>
              <Table.HeaderCell>{t('projects.peopleWorkingFromOffice')}</Table.HeaderCell>
              <Table.HeaderCell>{t('projects.peopleWorkingHybrid')}</Table.HeaderCell>
              <Table.HeaderCell>{t('projects.peopleWorkingRemotely')}</Table.HeaderCell>
              <Table.HeaderCell>{t('projects.notSetMembersCount')}</Table.HeaderCell>
              <Table.HeaderCell>{t('projects.unassignedMembersCount')}</Table.HeaderCell>
            </Responsive>
            {/* Show short header labels if screen width smaller than tablet */}
            <Responsive as={Table.Row} maxWidth={LAPTOP_MEDIA_WIDTH - 1}>
              <Table.HeaderCell>{t('projects.name')}</Table.HeaderCell>
              <Table.HeaderCell>{t('projects.leader')}</Table.HeaderCell>
              <Table.HeaderCell>{t('projects.assignedPeopleShort')}</Table.HeaderCell>
              <Table.HeaderCell>{t('projects.peopleWorkingFromOfficeShort')}</Table.HeaderCell>
              <Table.HeaderCell>{t('projects.peopleWorkingHybridShort')}</Table.HeaderCell>
              <Table.HeaderCell>{t('projects.peopleWorkingRemotelyShort')}</Table.HeaderCell>
              <Table.HeaderCell>{t('projects.notSetMembersCountShort')}</Table.HeaderCell>
              <Table.HeaderCell>{t('projects.unassignedMembersCountShort')}</Table.HeaderCell>
            </Responsive>
          </Table.Header>
          <Table.Body>{projectsCount === 0 ? (
            <Table.Row>
              <TableCell colSpan={columnCount} textAlign="center"><i>{t('common.noResultsFilters')}</i></TableCell>
            </Table.Row>
          ) : rows}
          </Table.Body>
        </Table>
      </Segment>
    </>
  );
};

export default Projects;
