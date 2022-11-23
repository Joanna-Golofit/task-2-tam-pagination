using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Tests;

internal static class DbContextExtensions
{
	public static void ClearDatabase(this ApplicationDbContext dbContext)
	{
		dbContext.RemoveRange(dbContext.Desks);
		dbContext.RemoveRange(dbContext.RoomEquipments);
		dbContext.RemoveRange(dbContext.Equipments);
		dbContext.RemoveRange(dbContext.Projects);
		dbContext.RemoveRange(dbContext.Rooms);
		dbContext.RemoveRange(dbContext.Buildings);
		dbContext.RemoveRange(dbContext.Employees);
		dbContext.RemoveRange(dbContext.UserRoles);
		dbContext.RemoveRange(dbContext.Roles);
		dbContext.RemoveRange(dbContext.Configs);

		dbContext.SaveChanges();
	}

	public static EmployeeEntity AddAdminEmployee(this ApplicationDbContext dbContext)
	{
		var adminRoleEntity = dbContext.Roles.SingleOrDefault(x => x.Name == RoleEntity.Admin);
		if (adminRoleEntity == null)
		{
			adminRoleEntity = RoleEntity.CreateRole(RoleEntity.Admin);
			dbContext.Add(adminRoleEntity);
		}

		var adminEmployee = new EmployeeEntity
		{
			Email = "test1admin@test.xxx",
			ExternalId = 9000,
			Id = Guid.NewGuid(),
			Name = "test1",
			Surname = "admin",
			UserLogin = "test1"
		};
		adminEmployee.UserRoles.Add(new UserRoleEntity { Role = adminRoleEntity });
		dbContext.Add(adminEmployee);

		dbContext.SaveChanges();

		return adminEmployee;
	}
}
