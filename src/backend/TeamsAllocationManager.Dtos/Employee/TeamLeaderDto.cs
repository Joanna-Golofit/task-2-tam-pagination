using System;

namespace TeamsAllocationManager.Dtos.Employee;

public class TeamLeaderDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Surname { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
}
