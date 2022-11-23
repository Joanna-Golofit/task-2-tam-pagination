using System.ComponentModel.DataAnnotations;

namespace TeamsAllocationManager.Dtos.Project;

public class NewExternalCompanyDto
{
	[Required]
	public string Name { get; set; } = "";
	[Required]
	[EmailAddress]
	public string Email { get; set; } = "";
	[Required]
	public int? InitialEmployeeCount { get; set; }
}
