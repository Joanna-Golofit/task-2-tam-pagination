using System;
using System.Collections.Generic;
using System.Text;

namespace TeamsAllocationManager.Domain.Models.ConfigDataModels;

public class DivisionConfigData
{
	public int ExternalGroupId { get; set; }
	public string Name { get; set; } = null!;
}
