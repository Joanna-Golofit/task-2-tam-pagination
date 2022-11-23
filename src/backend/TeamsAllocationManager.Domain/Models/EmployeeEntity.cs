using System.Collections.Generic;
using System.Linq;
using TeamsAllocationManager.Domain.Enums;

namespace TeamsAllocationManager.Domain.Models;

public class EmployeeEntity : Entity
{
	public int? ExternalId { get; set; }

	public string? Name { get; set; }

	public string? Surname { get; set; }

	public string Email { get; set; } = null!;

	public string UserLogin { get; set; } = null!;

	public WorkspaceType? WorkspaceType { get; set; }

	public IList<EmployeeWorkingTypeHistoryEntity> EmployeeWorkingTypeHistory { get; set; } = new List<EmployeeWorkingTypeHistoryEntity>();

	public IList<EmployeeDeskHistoryEntity> EmployeeDeskHistory { get; set; } = new List<EmployeeDeskHistoryEntity>();

	public IList<DeskReservationEntity> EmployeeDeskReservations { get; set; } = new List<DeskReservationEntity>();

	public IList<EmployeeEquipmentHistoryEntity> EquipmentHistory { get; set; } = new List<EmployeeEquipmentHistoryEntity>();
		
	public IList<EmployeeEquipmentEntity> EmployeeEquipment { get; set; } = new List<EmployeeEquipmentEntity>();

	public bool IsContractor { get; set; }

	public bool IsExternal { get; set; }

	public ICollection<EmployeeProjectEntity> Projects { get; set; } = new List<EmployeeProjectEntity>();

	public IList<UserRoleEntity> UserRoles { get; set; } = new List<UserRoleEntity>();

	public virtual ICollection<ProjectEntity> LedProjects
		=> Projects
			.Where(ep => ep.IsTeamLeaderProjectRole)
			.Select(ep => ep.Project)
			.ToList();

	public static EmployeeEntity NewExternalEmployee(ProjectEntity newCompany, int idx)
		=> new EmployeeEntity
		{
			Name = newCompany.Name,
			Surname = $"{idx}",
			Email = $"{idx}{newCompany.Email!}",
			UserLogin = $"{newCompany.Name}{idx}",
			WorkspaceType = Enums.WorkspaceType.Office,
			IsExternal = true
		};

	public void CreateReservationHistory(DeskReservationEntity reservation)
		=> EmployeeDeskHistory.Add(EmployeeDeskHistoryEntity.NewEntryFromDeskReservation(reservation));

	public bool IsTeamLeaderOrAdmin()
		=> UserRoles.Any(ur => ur.Role.Name == RoleEntity.TeamLeader || ur.Role.Name == RoleEntity.Admin);

	public void SetRoleIfNotAlreadySet(RoleEntity role)
	{
		if (UserRoles.All(r => r.Role.Name != role.Name))
		{
			if (UserRoles.Count > 0)
			{
				UserRoles.First().Role = role;
			}
			else
			{
				UserRoles.Add(new UserRoleEntity
				{
					Employee = this,
					EmployeeId = Id,
					Role = role,
					RoleId = role.Id
				});

			}
		}
	}
}
