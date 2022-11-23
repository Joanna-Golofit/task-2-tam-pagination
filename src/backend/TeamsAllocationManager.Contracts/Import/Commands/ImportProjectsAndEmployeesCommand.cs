using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Dtos.Import;

namespace TeamsAllocationManager.Contracts.Import.Commands;

public class ImportProjectsAndEmployeesCommand : ICommand<ImportReportDto>
{
	public ImportProjectsAndEmployeesCommand(string currentUsername)
	{
		CurrentUsername = currentUsername;
		IsAutoImport = false;
	}

	public ImportProjectsAndEmployeesCommand()
	{
		IsAutoImport = true;
	}

	public string? CurrentUsername { get; }
	public bool IsAutoImport { get; }
}
