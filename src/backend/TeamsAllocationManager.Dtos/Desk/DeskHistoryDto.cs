using System;

namespace TeamsAllocationManager.Dtos.Desk;

public class DeskHistoryDto
{
	public Guid Id { get; set; }
	public Guid DeskId { get; set; }
	public Guid EmployeeId { get; set; }
	public string EmployeeName { get; set; } = null!;
	public string EmployeeSurname { get; set; } = null!;
	public DateTime? From { get; set; }
	public DateTime? To { get; set; }
	public DateTime Updated { get; set; }
}
