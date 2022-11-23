export const employeeUrlPart: string = 'Employee';

export type UpdateEmployeeWorkspaceTypeDto = {
  employeeId: string;
  workspaceType: number;
}

export type TeamLeader = {
  id: string;
  name: string;
  surname: string;
  email: string;
}

export type TeamLeaderProjectForDropdownDto = {
  id: string;
  name: string;
}
