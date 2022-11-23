using System.Collections.Generic;

namespace TeamsAllocationManager.Dtos.Common;

public class MailDto
{
	public IEnumerable<string> Recipients { get; set; } = null!;
	public IEnumerable<string>? Cc { get; set; }
	public string Subject { get; set; } = null!;
	public string Body { get; set; } = null!;
}
