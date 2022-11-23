using System;
using System.Collections.Generic;

namespace TeamsAllocationManager.Integrations.FutureDatabase.Models;

public class Group
{
	public int Id { get; set; }

	public int? GroupTypeId { get; set; }

	public GroupType? GroupType { get; set; }

	public string? Name { get; set; }

	public DateTime FromDate { get; set; }

	public DateTime? ToDate { get; set; }

	public int? ParentGroupId { get; set; }

	public ParentGroup? ParentGroup { get; set; }

	public int? AuditCreatedBy { get; set; }

	public DateTime AuditCreatedDate { get; set; }

	public int? AuditLastModifiedBy { get; set; }

	public DateTime AuditLastModifiedDate { get; set; }

	public BusinessYearCheckpoint? BusinessYearCheckpoint { get; set; }

	public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

	public override string ToString()
	{
		return @$"""{Id}"":""{Name}""";
	}
}
