using System.ComponentModel.DataAnnotations;

namespace TeamsAllocationManager.Dtos.Project;

public class EditExternalCompanyDto
{
	public string? Name { get; set; }
	[EmailAddress]
	public string? Email { get; set; }
}
