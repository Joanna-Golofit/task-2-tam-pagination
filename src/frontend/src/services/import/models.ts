export type ImportReportDto = {
    removedEmptyProjectsCount: number;
    removedEmployeesWithoutProjectsCount: number;
    usersBeforeImportCount: number;
    usersAfterImportCount: number;
    projectsBeforeImportCount: number;
    projectsAfterImportCount: number;
    assignmentsBeforeImportCount: number;
    assignmentsAfterImportCount: number;
    removedEmployeesFromFutureAssigmentsCount: number;
}
