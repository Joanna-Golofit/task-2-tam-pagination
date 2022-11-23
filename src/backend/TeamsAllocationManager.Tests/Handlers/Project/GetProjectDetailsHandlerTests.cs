using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Project.Queries;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Domain.Enums;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Project;
using TeamsAllocationManager.Infrastructure.Handlers.Project;
using TeamsAllocationManager.Mapper.Profiles;
using TeamsAllocationManager.Tests.Helpers;

namespace TeamsAllocationManager.Tests.Handlers.Project;

public class GetProjectDetailsHandlerTests
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly ProjectsRepository _projectRepository;
	private readonly RoomRepository _roomRepository;

	public GetProjectDetailsHandlerTests()
	{
		DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: GetType().Name)
			.Options;
		_context = new ApplicationDbContext(options);
		_mapper = new MapperConfiguration(mc => mc.AddProfile(new EntityDtoProfile())).CreateMapper();
		_projectRepository = new ProjectsRepository(_context);
		_roomRepository = new RoomRepository(_context);
	}

	[SetUp]
	public void SetupBeforeEachTest()
	{
		_context.ClearDatabase();
	}

	[Test]
	public async Task ShouldReturnProjectDetails()
	{
		// given
		var teamLeader1 = new EmployeeEntity { Name = "Jan", Surname = "Kowalski", Email = "jkowalski@fp.pl", ExternalId = 1, UserLogin = "jkowalski" };
		var employee1 = new EmployeeEntity { Name = "Katarzyna", Surname = "Nowak", Email = "knowak@fp.pl", ExternalId = 2, UserLogin = "knowak" };

		var project1 = new ProjectEntity
		{
			Name = "Project 1", EndDate = DateTime.Now.AddDays(100), ExternalId = 1,
			Employees = new List<EmployeeProjectEntity> {
				new EmployeeProjectEntity { Employee = teamLeader1, IsTeamLeaderProjectRole = true },
				new EmployeeProjectEntity { Employee = employee1 }
			}
		};
		_context.Projects.Add(project1);

		var project2 = new ProjectEntity
		{
			Name = "Project 2", EndDate = DateTime.Now.AddDays(100), ExternalId = 2,
			Employees = new List<EmployeeProjectEntity> {
				new EmployeeProjectEntity { Employee = teamLeader1, IsTeamLeaderProjectRole = true }
			}
		};
		_context.Projects.Add(project2);

		var building1 = new BuildingEntity { Name = "F1" };
		var floor1 = new FloorEntity { Building = building1, FloorNumber = 0 };
		var room1 = new RoomEntity { Area = 22.5m, Name = "001", Floor = floor1 };
		var floor2 = new FloorEntity { Building = building1, FloorNumber = 0 };
		var room2 = new RoomEntity { Area = 26.5m, Name = "002", Floor = floor2 };
		var floor3 = new FloorEntity { Building = building1, FloorNumber = 0 };
		var room3 = new RoomEntity { Area = 12.5m, Name = "003", Floor = floor3 };

		var hotDesk1 = new DeskEntity { Room = room1, Number = 6, IsHotDesk = true, IsEnabled = true };
		var hotDesk2 = new DeskEntity { Room = room2, Number = 3, IsHotDesk = true, IsEnabled = true };

		_context.Desks.AddRange(
			new DeskEntity { Room = room1, Number = 1, IsEnabled = true },
			DeskHelpers.CreateDeskWithReservationForWholeWeek(room1, 2, employee1),
			DeskHelpers.CreateDeskWithReservationForWholeWeek(room1, 4, teamLeader1),
			DeskHelpers.CreateDeskWithReservationForWholeWeek(room2, 5, teamLeader1),
			hotDesk1,
			hotDesk2,
			new DeskEntity { Room = room3, Number = 7, IsEnabled = true },
			new DeskEntity { Room = room3, Number = 8, IsEnabled = true }
		);

		_context.SaveChanges();

		var query = new GetProjectDetailsQuery(project1.Id);
		var handler = new GetProjectDetailsHandler(_projectRepository, _roomRepository, _mapper);

		// when
		ProjectDetailsDto result = (await handler.HandleAsync(query))!;

		// then
		Assert.AreEqual(project1.Id, result.Id);
		Assert.AreEqual(project1.Name, result.Name);
		Assert.AreEqual(project1.Employees.Count, result.PeopleCount);
		Assert.AreEqual(project1.EndDate, result.EndDate);
		Assert.AreEqual(2, result.Rooms.Count());
		Assert.AreEqual(1, result.Rooms.Count(r => r.Id == room1.Id));
		Assert.AreEqual(1, result.Rooms.Count(r => r.Id == room2.Id));
		Assert.AreEqual(1, result.TeamLeaders.Count);

		var rooms = result.Rooms.ToList();
		Assert.AreEqual(2, rooms[0].OccupiedDesksCount);
		Assert.AreEqual(1, rooms[0].HotDesksCount);
		Assert.AreEqual(1, rooms[0].FreeDesksCount);
		Assert.AreEqual(1, rooms[1].OccupiedDesksCount);
		Assert.AreEqual(1, rooms[1].HotDesksCount);
		Assert.AreEqual(0, rooms[1].FreeDesksCount);
	}

	[Test]
	public async Task ShouldReturnProjectDetails_TeamManager()
	{
		// given
		var teamLeader1 = new EmployeeEntity { Name = "Jan", Surname = "Kowalski", Email = "jkowalski@fp.pl", ExternalId = 1, UserLogin = "jkowalski" };
		var teamLeader2 = new EmployeeEntity { Name = "Jan", Surname = "Kowalski2", Email = "jkowalski2@fp.pl", ExternalId = 3, UserLogin = "jkowalski2" };
		var teamLeader3 = new EmployeeEntity { Name = "Jan", Surname = "Kowalski3", Email = "jkowalski3@fp.pl", ExternalId = 4, UserLogin = "jkowalski3" };
		var employee1 = new EmployeeEntity { Name = "Katarzyna", Surname = "Nowak", Email = "knowak@fp.pl", ExternalId = 2, UserLogin = "knowak" };

		var project1 = new ProjectEntity
		{
			Name = "Project 1", EndDate = DateTime.Now.AddDays(100), ExternalId = 1,
			Employees = new List<EmployeeProjectEntity> {
				new EmployeeProjectEntity { Employee = teamLeader1, IsTeamLeaderProjectRole = true },
				new EmployeeProjectEntity { Employee = teamLeader2, IsTeamLeaderProjectRole = true },
				new EmployeeProjectEntity { Employee = teamLeader3, IsTeamLeaderProjectRole = true },
				new EmployeeProjectEntity { Employee = employee1 }
			}
		};
		_context.Projects.Add(project1);

		var project2 = new ProjectEntity
		{
			Name = "Project 2", EndDate = DateTime.Now.AddDays(100), ExternalId = 2,
			Employees = new List<EmployeeProjectEntity> {
				new EmployeeProjectEntity { Employee = teamLeader1, IsTeamLeaderProjectRole = true }
			}
		};
		_context.Projects.Add(project2);

		var building1 = new BuildingEntity { Name = "F1" };
		var floor1 = new FloorEntity { Building = building1, FloorNumber = 0 };
		var room1 = new RoomEntity { Area = 22.5m, Name = "001", Floor = floor1 };
		var floor2 = new FloorEntity { Building = building1, FloorNumber = 0 };
		var room2 = new RoomEntity { Area = 26.5m, Name = "002", Floor = floor2 };
		var floor3 = new FloorEntity { Building = building1, FloorNumber = 0 };
		var room3 = new RoomEntity { Area = 12.5m, Name = "003", Floor = floor3 };

		_context.Desks.AddRange(
			new DeskEntity { Room = room1, Number = 1 },
			DeskHelpers.CreateDeskWithReservation(room1, 2, employee1),
			new DeskEntity { Room = room2, Number = 3 },
			DeskHelpers.CreateDeskWithReservation(room1, 4, teamLeader1),
			DeskHelpers.CreateDeskWithReservation(room2, 5, teamLeader1),
			new DeskEntity { Room = room1, Number = 6 },
			new DeskEntity { Room = room3, Number = 7 },
			new DeskEntity { Room = room3, Number = 8 });

		_context.SaveChanges();

		var query = new GetProjectDetailsQuery(project1.Id);
		var handler = new GetProjectDetailsHandler(_projectRepository, _roomRepository, _mapper);

		// when
		ProjectDetailsDto result = (await handler.HandleAsync(query))!;

		// then
		Assert.AreEqual(project1.Id, result.Id);
		Assert.AreEqual(project1.Name, result.Name);
		Assert.AreEqual(project1.Employees.Count, result.PeopleCount);
		Assert.AreEqual(project1.EndDate, result.EndDate);
		Assert.AreEqual(2, result.Rooms.Count());
		Assert.AreEqual(1, result.Rooms.Count(r => r.Id == room1.Id));
		Assert.AreEqual(1, result.Rooms.Count(r => r.Id == room2.Id));
		Assert.AreEqual(3, result.TeamLeaders.Count);
	}

	[Test]
	public async Task ShouldThrowForNotExisting()
	{
		// given
		var teamLeader1 = new EmployeeEntity { Name = "Jan", Surname = "Kowalski", Email = "jkowalski@fp.pl", ExternalId = 1, UserLogin = "jkowalski" };
		var employee1 = new EmployeeEntity { Name = "Adam", Surname = "Nowak", Email = "anowak@fp.pl", ExternalId = 2, UserLogin = "anowak" };

		var project1 = new ProjectEntity
		{
			Name = "Project 1", EndDate = DateTime.Now.AddDays(100),
			Employees = new List<EmployeeProjectEntity> {
				new EmployeeProjectEntity { Employee = teamLeader1, IsTeamLeaderProjectRole = true },
				new EmployeeProjectEntity { Employee = employee1 }
			}
		};
		_context.Projects.Add(project1);

		var building1 = new BuildingEntity { Name = "F1" };
		var floor1 = new FloorEntity { Building = building1, FloorNumber = 0 };
		var room1 = new RoomEntity { Area = 22.5m, Name = "001", Floor = floor1 };
		_context.Desks.AddRange(
			new DeskEntity { Room = room1, Number = 1 },
			new DeskEntity { Room = room1, Number = 2 },
			new DeskEntity { Room = room1, Number = 3 }
			);

		_context.SaveChanges();

		var query = new GetProjectDetailsQuery(Guid.NewGuid());
		var handler = new GetProjectDetailsHandler(_projectRepository, _roomRepository, _mapper);

		// when-then
		await Should.ThrowAsync<Exception>(async () => await handler.HandleAsync(query));
	}
}
