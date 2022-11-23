using System;
using System.Collections.Generic;

namespace TeamsAllocationManager.Integrations.FutureDatabase.Models;

public class User
{
	public int Id { get; set; }

	public string? DomainUserLogin { get; set; }

	public string? DomainUserSid { get; set; }

	public int? FpDevId { get; set; }

	public string? FirstName { get; set; }

	public string? LastName { get; set; }

	public string? SecondName { get; set; }

	public string Email { get; set; } = null!;

	public int? AuditCreatedBy { get; set; }

	public int? AuditLastModifiedBy { get; set; }

	public DateTime AuditCreatedDate { get; set; }

	public DateTime AuditLastModifiedDate { get; set; }

	public int UserTypeId { get; set; }

	public ICollection<Assignment> AssignmentUsers { get; set; } = new List<Assignment>();
}
