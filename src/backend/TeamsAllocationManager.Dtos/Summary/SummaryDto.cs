using TeamsAllocationManager.Dtos.Enums;

namespace TeamsAllocationManager.Dtos.Summary;

public class SummaryDto
{
	public int ProjectsCount { get; set; }

	// Desks
	public int DesksCount { get; set; }
	public int FreeDesksCount { get; set; }
	public int OccupiedDesksCount { get; set; }
	public int HotDesksCount { get; set; }
	public int DisabledDesksCount { get; set; }

	// All employees
	public int AllEmployeesCount { get; set; }
	public int AllOfficeEmployeesCount { get; set; }
	public int AllUnassignedOfficeEmployeesCount { get; set; }
	public int AllAssignedOfficeEmployeesCount { get; set; }
	public int AllHybridEmployeesCount { get; set; }
	public int AllUnassignedHybridEmployeesCount { get; set; }
	public int AllAssignedHybridEmployeesCount { get; set; }
	public int AllRemoteEmployeesCount { get; set; }
	public int AllNotSetEmployeesCount { get; set; }
	public int AllAssignedToDesksCount { get; set; }
	public int AllUnassignedToDesksCount { get; set; }

	// FP employees
	public int FpEmployeesCount { get; set; }
	public int FpOfficeEmployeesCount { get; set; }
	public int FpUnassignedOfficeEmployeesCount { get; set; }
	public int FpAssignedOfficeEmployeesCount { get; set; }
	public int FpHybridEmployeesCount { get; set; }
	public int FpUnassignedHybridEmployeesCount { get; set; }
	public int FpAssignedHybridEmployeesCount { get; set; }
	public int FpRemoteEmployeesCount { get; set; }
	public int FpNotSetEmployeesCount { get; set; }
	public int FpAssignedToDesksCount { get; set; }
	public int FpUnassignedToDesksCount { get; set; }

	// Contractors
	public int ContractorEmployeesCount { get; set; }
	public int ContractorOfficeEmployeesCount { get; set; }
	public int ContractorUnassignedOfficeEmployeesCount { get; set; }
	public int ContractorAssignedOfficeEmployeesCount { get; set; }
	public int ContractorHybridEmployeesCount { get; set; }
	public int ContractorUnassignedHybridEmployeesCount { get; set; }
	public int ContractorAssignedHybridEmployeesCount { get; set; }
	public int ContractorRemoteEmployeesCount { get; set; }
	public int ContractorNotSetEmployeesCount { get; set; }
	public int ContractorAssignedToDesksCount { get; set; }
	public int ContractorUnassignedToDesksCount { get; set; }

	public ErrorCodes ErrorCode { get; set; }
}
