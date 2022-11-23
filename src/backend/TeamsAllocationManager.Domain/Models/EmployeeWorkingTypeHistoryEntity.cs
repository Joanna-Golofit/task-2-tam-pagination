using System;
using System.Collections.Generic;
using System.Linq;
using TeamsAllocationManager.Domain.Enums;

namespace TeamsAllocationManager.Domain.Models;

public class EmployeeWorkingTypeHistoryEntity : Entity
{
	public Guid? EmployeeId { get; set; }
	public EmployeeEntity Employee { get; set; } = null!;
	public DateTime From { get; set; }
	public DateTime? To { get; set; }

	public WorkspaceType? WorkspaceType { get; set; }

}
