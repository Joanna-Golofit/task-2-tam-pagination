using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Desks.Commands;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Domain.Enums;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Infrastructure.Handlers.Desk;

namespace TeamsAllocationManager.Tests.Handlers.Desk;

[TestFixture]
public class UpdateDeskPersonHandlerTests
{
	private readonly ApplicationDbContext _context;

	public UpdateDeskPersonHandlerTests()
	{
		DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: GetType().Name)
			.Options;
		_context = new ApplicationDbContext(options);
	}

	[SetUp]
	public void SetupBeforeEachTest()
	{
		_context.ClearDatabase();

		var employee = new EmployeeEntity { Name = "test", Surname = "person", Email = "tperson@fp.pl", ExternalId = 2 };

		var teamLeader1 = new EmployeeEntity { Name = "Jan", Surname = "Kowalski", Email = "jkowalski@fp.pl", ExternalId = 1 };
		var project1 = new ProjectEntity
		{
			Name = "Project 1", EndDate = DateTime.Now.AddDays(100), Employees = new List<EmployeeProjectEntity> {
				new EmployeeProjectEntity { Employee = teamLeader1, IsTeamLeaderProjectRole = true },
				new EmployeeProjectEntity { Employee = employee },
			}
		};
		_context.Projects.Add(project1);

		var floor = new FloorEntity { Building = new BuildingEntity { Name = "F1" }, FloorNumber = 0 };
		var room1 = new RoomEntity { Area = 22.5m, Name = "001", Floor = floor };

		_context.Desks.AddRange(
			new DeskEntity { Room = room1, Number = 1 },
			new DeskEntity { Room = room1, Number = 2 },
			new DeskEntity { Room = room1, Number = 3 },
			new DeskEntity { Room = room1, Number = 4 },
			new DeskEntity { Room = room1, Number = 5 });

		_context.SaveChanges();
	}
}
