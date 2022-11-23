using System.Collections.Generic;

namespace TeamsAllocationManager.Infrastructure.Options;

public class AutoImportSettings
{
	public IEnumerable<int> AutoAdminProjectExternalIds { get; set; } = new List<int>();
}
