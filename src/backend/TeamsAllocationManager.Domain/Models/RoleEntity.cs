using System;
using System.Collections.Generic;

namespace TeamsAllocationManager.Domain.Models;

public class RoleEntity : Entity
{
	public const string Admin = "Admin";
	public const string TeamLeader = "TeamLeader";

	public string Name { get; private set; } = null!;

	public static RoleEntity CreateRole(string roleName)
	{
		if (roleName.Equals(Admin, StringComparison.OrdinalIgnoreCase))
		{
			return new RoleEntity { Name = Admin };
		}
		else if (roleName.Equals(TeamLeader, StringComparison.OrdinalIgnoreCase))
		{
			return new RoleEntity { Name = TeamLeader };
		}
		else
		{
			throw new ArgumentException($"Unknown role name: {roleName ?? string.Empty} ");
		}
	}
}
