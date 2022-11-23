using System;
using System.Collections.Generic;
using System.Text;

namespace TeamsAllocationManager.Dtos.Import;

public class ImportReportDto
{
	public int RemovedEmptyProjectsCount { get; set; }
	public int RemovedEmployeesWithoutProjectsCount { get; set; }
	public int UsersBeforeImportCount { get; set; }
	public int UsersAfterImportCount { get; set; }
	public int ProjectsBeforeImportCount { get; set; }
	public int ProjectsAfterImportCount { get; set; }
	public int AssignmentsBeforeImportCount { get; set; }
	public int AssignmentsAfterImportCount { get; set; }
	public int RemovedEmployeesFromFutureAssigmentsCount { get; set; }
}
