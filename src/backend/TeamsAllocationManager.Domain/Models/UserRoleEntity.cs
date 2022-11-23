using System;

namespace TeamsAllocationManager.Domain.Models;

public class UserRoleEntity : Entity
{
	public Guid EmployeeId { get; set; }
	public EmployeeEntity Employee { get; set; } = null!;
	public Guid RoleId { get; set; }
	public RoleEntity Role { get; set; } = null!;
}
