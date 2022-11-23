import React from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { Label, Icon, Dropdown } from 'semantic-ui-react';
import { shortDate } from '../../../providers/dateTimeFormatProvider';
import { ProjectDetails } from '../../../services/project/models';
import { AppState } from '../../../store';

type ComponentProps = {
  project: ProjectDetails;
};

const ProjectInfo: React.FC<ComponentProps> = ({ project }) => {
  const { t } = useTranslation();
  const { projectDetailsResponse } = useSelector((state: AppState) => state.projectDetails);
  const { rooms, employees } = projectDetailsResponse;
  const employeesIds = employees.map((e) => e.id);
  const assignedPeopleCount = rooms.map((r) => r.desksInRoom)
    .flat().filter((d) => employeesIds.some((e) => d.reservations.map((r) => r.employee.id).includes(e))).length;
  const teamLeaders = project?.teamLeaders;

  return (project && (
  <Label.Group size="big">
    {teamLeaders.length === 0 && (<Label>{t('common.noTeamleader')}</Label>)}
    {teamLeaders.length === 1 && (
    <Label as="a" href={`mailto:${teamLeaders[0].email}`}>
      <Icon name="mail" />
      {teamLeaders[0].name} {teamLeaders[0].surname}
      <Label.Detail>{t('projectDetails.teamLeader')}</Label.Detail>
    </Label>
    )}
    {teamLeaders.length > 1 && (
    <Label>
      <Dropdown
        text={t('projectDetails.teamLeaders')}
        scrolling
      >
        <Dropdown.Menu>
          {(teamLeaders.map((teamLeader) => (
            <Dropdown.Item key={teamLeader.id}>
              <Label as="a" href={`mailto:${teamLeader.email}`}>
                <Icon name="mail" />
                {teamLeader.name} {teamLeader.surname}
                <Label.Detail>{t('projectDetails.teamLeader')}</Label.Detail>
              </Label>
            </Dropdown.Item>
          )))}
        </Dropdown.Menu>
      </Dropdown>
    </Label>
    )}
    {project.endDate && (
    <Label>
      <Icon name="calendar" />
      {shortDate(project.endDate)}
      <Label.Detail>{t('projectDetails.deadline')}</Label.Detail>
    </Label>
    )}
    <Label>
      <Icon name="group" />
      {assignedPeopleCount}
      <Label.Detail>{t('projectDetails.desksTaken')}</Label.Detail>
    </Label>
  </Label.Group>
  ));
};

export default ProjectInfo;
