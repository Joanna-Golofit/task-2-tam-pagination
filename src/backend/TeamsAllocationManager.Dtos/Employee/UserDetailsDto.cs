using System;
using System.Collections.Generic;
using TeamsAllocationManager.Dtos.Desk;

namespace TeamsAllocationManager.Dtos.Employee;

public class UserDetailsDto
{
	public Guid Id { get; set; }
	public string Email { get; set; } = null!;
	public string EmployeeName { get; set; } = null!;
	public string EmployeeSurname { get; set; } = null!;
	public int EmployeeType { get; set; }
	public int? WorkspaceType { get; set; }
	public bool IsExternal { get; set; }
	public List<UserLocationDto> Locations { get; set; } = new List<UserLocationDto>();
	public List<UserProjectDto> Projects { get; set; } = new List<UserProjectDto>();
	public List<ReservationInfoDto> ReservationInfo { get; set; } = new List<ReservationInfoDto>();
	public List<HotDeskReservationInfoDto> LedProjectsReservationInfo { get; set; } = new List<HotDeskReservationInfoDto>();
}
