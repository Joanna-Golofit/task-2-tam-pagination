import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useTranslation } from 'react-i18next';
import { Segment, Header } from 'semantic-ui-react';
import { RouteComponentProps } from 'react-router-dom';
import { AppState } from '../../store';
import { fetchProjectDetails, clearProjectDetails }
  from '../../store/projectDetails/actions';
import RoomsTable from './components/roomsTable';
import ProjectInfo from './components/projectInfo';
import UsersTable from './components/usersTable';
import { setLoadingAction } from '../../store/global/actions';

type ComponentProps = {
  id: string;
} & RouteComponentProps;

const ProjectDetails: React.FC<ComponentProps> = ({ id }) => {
  const dispatch = useDispatch();
  const projectDetails = useSelector((state: AppState) => state.projectDetails);
  const { projectDetailsResponse: project } = projectDetails;

  const { employees } = project;
  const { t } = useTranslation();

  useEffect(() => {
    dispatch(setLoadingAction(true));
    dispatch(fetchProjectDetails(id));

    return () => {
      dispatch(clearProjectDetails());
    };
  }, []);

  useEffect(() => {
    document.title = `${t('common.tam')} - ${project?.name}`;
    setLoadingAction(false);
  }, [project]);

  return (
    <Segment basic>
      {project && (
        <>
          {t('projectDetails.header')}
          <Header as="h1">
            {project?.name}
          </Header>

          <ProjectInfo project={project} />

          <UsersTable />

          <RoomsTable
            employees={employees}
          />
        </>
      )}
    </Segment>
  );
};

export default ProjectDetails;
