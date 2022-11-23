using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database;

public static class ApiDataSeeder
{
	public static void EnsureSeed(IServiceProvider serviceProvider,
		List<Tuple<string, int, string, decimal, int>> buildingsRoomsDesksFromCSV)
	{
		using IServiceScope scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
		ApplicationDbContext context = scope.ServiceProvider.GetService<ApplicationDbContext>()!;

		AddRoles(context);

		AddBuldingsStructures(context, buildingsRoomsDesksFromCSV);
		Console.WriteLine("Import complete. You can close the window.");
	}

	private static void AddRoles(ApplicationDbContext dbContext)
	{
		var rolesToAdd = new List<RoleEntity>();

		RoleEntity? roleAdmin = dbContext.Roles.FirstOrDefault(x => x.Name.Equals(RoleEntity.Admin));

		if (roleAdmin == null)
		{
			roleAdmin = RoleEntity.CreateRole(RoleEntity.Admin);

			rolesToAdd.Add(roleAdmin);
		}

		RoleEntity? roleTeamLeader = dbContext.Roles.FirstOrDefault(x => x.Name.Equals(RoleEntity.TeamLeader));

		if (roleTeamLeader == null)
		{
			roleTeamLeader = RoleEntity.CreateRole(RoleEntity.TeamLeader);

			rolesToAdd.Add(roleTeamLeader);
		}

		if (rolesToAdd.Any())
		{
			dbContext.Roles.AddRange(rolesToAdd);
			dbContext.SaveChanges();
		}
	}

	private static void AddBuldingsStructures(ApplicationDbContext dbContext, List<Tuple<string, int, string, decimal, int>> buildingsRoomsDesksFromCSV)
	{
		if (dbContext.Buildings.Any())
		{
			return;
		}

		// Buildings

		var buildingNames = new List<String> { "F1", "F2", "F3", "F4", "F4C" };

		foreach (string tempBuildingName in buildingNames)
		{
			dbContext.Buildings.Add(new BuildingEntity { Name = tempBuildingName });
		}

		dbContext.SaveChanges();

		// Floors, Rooms, Desks

		var roomsToAddList = new List<RoomEntity>();

		// Tuple -> buldingName, floorNumber, roomNumber, area, numberOfDesks
		foreach (Tuple<string, int, string, decimal, int> item in buildingsRoomsDesksFromCSV)
		{
			FloorEntity? floorEntity = dbContext.Floors.FirstOrDefault(f => f.FloorNumber == item.Item2 && f.Building.Name.Equals(item.Item1));

			if (floorEntity == null)
			{
				BuildingEntity buildingEntity = dbContext.Buildings.Single(x => x.Name.Equals(item.Item1));
				floorEntity = new FloorEntity { Building = buildingEntity, FloorNumber = item.Item2 };
				dbContext.Floors.Add(floorEntity);
				dbContext.SaveChanges();
			}

			var roomToAdd = new RoomEntity { Floor = floorEntity, Name = item.Item3, Area = item.Item4 };

			roomsToAddList.Add(roomToAdd);

			for (int i = 0; i < item.Item5; i++)
			{
				dbContext.Desks.Add(new DeskEntity { Number = i + 1, Room = roomToAdd });
			}
		}

		dbContext.SaveChanges();
	}
}
