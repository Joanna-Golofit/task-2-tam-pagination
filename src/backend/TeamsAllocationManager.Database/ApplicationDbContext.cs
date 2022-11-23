using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Domain;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database;

public class ApplicationDbContext : DbContext
{
	public DbSet<BuildingEntity> Buildings { get; set; } = null!;
	public DbSet<FloorEntity> Floors { get; set; } = null!;
	public DbSet<DeskEntity> Desks { get; set; } = null!;
	public DbSet<DeskReservationEntity> DeskReservations { get; set; } = null!;
	public DbSet<EmployeeEntity> Employees { get; set; } = null!;
	public DbSet<EmployeeProjectEntity> EmployeeProjects { get; set; } = null!;
	public DbSet<EquipmentEntity> Equipments { get; set; } = null!;
	public DbSet<ProjectEntity> Projects { get; set; } = null!;
	public DbSet<RoomEntity> Rooms { get; set; } = null!;
	public DbSet<RoomEquipmentEntity> RoomEquipments { get; set; } = null!;
	public DbSet<RoleEntity> Roles { get; set; } = null!;
	public DbSet<UserRoleEntity> UserRoles { get; set; } = null!;
	public DbSet<ConfigEntity> Configs { get; set; } = null!;
	public DbSet<EmployeeWorkingTypeHistoryEntity> EmployeeWorkingTypeHistory { get; set; } = null!;
	public DbSet<EmployeeDeskHistoryEntity> EmployeeDeskHistory { get; set; } = null!;
	public DbSet<EmployeeEquipmentEntity> EmployeeEquipments { get; set; } = null!;
	public DbSet<EmployeeEquipmentHistoryEntity> EmployeeEquipmentHistory { get; set; } = null!;

	public ApplicationDbContext(DbContextOptions options)
		: base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
		=> modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

	public override int SaveChanges(bool acceptAllChangesOnSuccess)
	{
		SetDates();
		return base.SaveChanges(acceptAllChangesOnSuccess);
	}

	public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
	{
		SetDates();
		return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
	}

	private void SetDates()
	{
		DateTime modificationDate = DateTime.Now;

		foreach (EntityEntry entry in ChangeTracker.Entries())
		{
			if (entry.Entity is Entity trackedEntity)
			{
				if (entry.State == EntityState.Added)
				{
					trackedEntity.Created = trackedEntity.Updated = modificationDate;
				}
				else if (entry.State == EntityState.Modified)
				{
					trackedEntity.Updated = modificationDate;
				}

				entry.CurrentValues.SetValues(trackedEntity);
			}
		}
	}
}
