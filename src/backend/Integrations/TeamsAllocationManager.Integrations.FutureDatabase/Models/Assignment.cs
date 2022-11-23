using System;

namespace TeamsAllocationManager.Integrations.FutureDatabase.Models;

public class Assignment
{
	public int Id { get; set; }

	public int GroupId { get; set; }

	public int UserId { get; set; }
		
	public int RoleId { get; set; }

	public DateTime? FromDate { get; set; }
		
	public DateTime? ToDate { get; set; }

	public int? Participation { get; set; }

	public int? ParticipationOfficial { get; set; }

	public int? AuditCreatedBy { get; set; }

	public int? AuditLastModifiedBy { get; set; }

	public DateTime AuditCreatedDate { get; set; }

	public DateTime AuditLastModifiedDate { get; set; }

	public bool IsHalfLeader { get; set; }
}
