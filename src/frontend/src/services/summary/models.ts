import { ErrorCodes } from '../common/models';

export const summaryUrlPart: string = 'Summary';

export type SummaryDto = {
  projectsCount: number;

  desksCount: number;
  freeDesksCount: number;
  occupiedDesksCount: number;
  hotDesksCount: number;

  allEmployeesCount: number;
  allOfficeEmployeesCount: number;
  allAssignedOfficeEmployeesCount: number;
  allUnassignedOfficeEmployeesCount: number;
  allHybridEmployeesCount: number;
  allAssignedHybridEmployeesCount: number;
  allUnassignedHybridEmployeesCount: number;
  allRemoteEmployeesCount: number;
  allNotSetEmployeesCount: number;
  allAssignedToDesksCount: number;
  allUnassignedToDesksCount: number;

  fpEmployeesCount: number;
  fpOfficeEmployeesCount: number;
  fpAssignedOfficeEmployeesCount: number;
  fpUnassignedOfficeEmployeesCount: number;
  fpHybridEmployeesCount: number;
  fpAssignedHybridEmployeesCount: number;
  fpUnassignedHybridEmployeesCount: number;
  fpRemoteEmployeesCount: number;
  fpNotSetEmployeesCount: number;
  fpAssignedToDesksCount: number;
  fpUnassignedToDesksCount: number;

  contractorEmployeesCount: number;
  contractorOfficeEmployeesCount: number;
  contractorAssignedOfficeEmployeesCount: number;
  contractorUnassignedOfficeEmployeesCount: number;
  contractorHybridEmployeesCount: number;
  contractorAssignedHybridEmployeesCount: number;
  contractorUnassignedHybridEmployeesCount: number;
  contractorRemoteEmployeesCount: number;
  contractorNotSetEmployeesCount: number;
  contractorAssignedToDesksCount: number;
  contractorUnassignedToDesksCount: number;

  errorCode: ErrorCodes;
}
