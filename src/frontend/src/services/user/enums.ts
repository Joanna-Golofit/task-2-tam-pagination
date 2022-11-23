import i18n from 'i18next';

export enum WorkspaceType {
    Office = 0,
    Remote = 1,
    Hybrid = 2
}

export enum EmployeeType {
    Common = 0,
    TeamLeader = 1
}

export enum UserRole {
  TeamLeader = 'TeamLeader',
  Admin = 'Admin'
}

export function getWorkspaceTypeDisplayName(type: WorkspaceType | null): string {
  let displayName = '';

  switch (type) {
    case WorkspaceType.Office:
      displayName = i18n.t('workspaceType.office');
      break;
    case WorkspaceType.Remote:
      displayName = i18n.t('workspaceType.remote');
      break;
    case WorkspaceType.Hybrid:
      displayName = i18n.t('workspaceType.hybrid');
      break;
    case null:
      displayName = i18n.t('workspaceType.notSet');
      break;
    default:
      displayName = '?';
      break;
  }

  return displayName;
}

export function getEmployeeTypeDisplayName(type: EmployeeType): string {
  let displayName = '';

  switch (type) {
    case EmployeeType.Common:
      displayName = i18n.t('employeeType.common');
      break;
    case EmployeeType.TeamLeader:
      displayName = i18n.t('employeeType.teamLeader');
      break;
    default:
      displayName = '?';
      break;
  }

  return displayName;
}
