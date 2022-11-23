using System;
using System.Collections.Generic;
using System.Text;

namespace TeamsAllocationManager.Domain.Models.ConfigDataModels;

public class IgnoredProjectConfigData
{
	public int ExternalId { get; set; }
	public string Name { get; set; } = null!;
}
