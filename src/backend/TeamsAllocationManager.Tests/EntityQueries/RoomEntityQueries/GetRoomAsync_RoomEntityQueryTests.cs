using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Enums;
using TeamsAllocationManager.Dtos.Room;
using TeamsAllocationManager.Infrastructure.EntityQueries;
using TeamsAllocationManager.Infrastructure.Exceptions;
using TeamsAllocationManager.Mapper.Profiles;
using TeamsAllocationManager.Tests.Helpers;

namespace TeamsAllocationManager.Tests.EntityQueries.RoomEntityQueries;

[TestFixture]
public class GetRoomAsync_RoomEntityQueryTests
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly RoomEntityQuery _entityQuery;

	public GetRoomAsync_RoomEntityQueryTests()
	{
		DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: GetType().Name)
			.Options;
		_context = new ApplicationDbContext(options);
		_mapper = new MapperConfiguration(mc => mc.AddProfile(new EntityDtoProfile())).CreateMapper();
		_entityQuery = new RoomEntityQuery(_context, _mapper);
	}

	[SetUp]
	public void SetupBeforeEachTest()
	{
		_context.ClearDatabase();
	}

	[Test]
	public async Task ShouldReturnProperlyRoom()
	{
		// given
		var teamLeader1 = new EmployeeEntity { Name = "Jan", Surname = "Kowalski", Email = "jkowalski@fp.pl", ExternalId = 1, UserLogin = "jkowalski" };
		var employee1 = new EmployeeEntity { Name = "Adam", Surname = "Nowak", Email = "anowak@fp.pl", ExternalId = 2, UserLogin = "anowak" };
		var employee2 = new EmployeeEntity { Name = "Anna", Surname = "Maria", Email = "amaria@fp.pl", ExternalId = 3, UserLogin = "amaria" };

		var project1 = new ProjectEntity
		{
			Name = "Project 1", EndDate = DateTime.Now.AddDays(100), ExternalId = 1, Employees = new List<EmployeeProjectEntity> {
				new EmployeeProjectEntity { Employee = teamLeader1, IsTeamLeaderProjectRole = true },
				new EmployeeProjectEntity { Employee = employee1 }
			}
		};
		_context.Projects.Add(project1);

		var project2 = new ProjectEntity
		{
			Name = "Project 2", EndDate = DateTime.Now.AddDays(100), ExternalId = 2, Employees = new List<EmployeeProjectEntity> {
				new EmployeeProjectEntity { Employee = teamLeader1, IsTeamLeaderProjectRole = true },
				new EmployeeProjectEntity { Employee = employee2 }
			}
		};
		_context.Projects.Add(project2);

		var building1 = new BuildingEntity { Name = "F1" };
		var floor1 = new FloorEntity { Building = building1, FloorNumber = 0 };
		var room1 = new RoomEntity { Area = 22.5m, Name = "001", Floor = floor1 };

		var hotDesk1 = new DeskEntity { Id = Guid.NewGuid(), Room = room1, Number = 4, IsHotDesk = true };
		var hotDesk2 = new DeskEntity { Id = Guid.NewGuid(), Room = room1, Number = 5, IsHotDesk = true };

		_context.Desks.AddRange(
			new DeskEntity { Room = room1, Number = 1 },
			DeskHelpers.CreateDeskWithReservation(room1, 2, employee1),
			DeskHelpers.CreateDeskWithReservation(room1, 3, employee2),
			hotDesk1,
			hotDesk2);

		_context.SaveChanges();
			
		// when
		RoomDetailsDto result = await _entityQuery.GetRoomAsync(room1.Id);

		// then
		Assert.AreEqual(room1.Id, result.Id);
		Assert.AreEqual(room1.Area, result.Area);
		Assert.AreEqual(room1.Floor.FloorNumber, result.Floor);
		Assert.AreEqual(building1.Name, result.Building.Name);
		Assert.AreEqual(room1.Desks.Count, result.Capacity);
		Assert.AreEqual(room1.Desks?.Count(d => d.DeskReservations.Any(dr => dr.IsSchedule
																			 && dr.ScheduledWeekdays.Contains(DayOfWeek.Monday)
																			 && dr.ScheduledWeekdays.Contains(DayOfWeek.Tuesday)
																			 && dr.ScheduledWeekdays.Contains(DayOfWeek.Wednesday)
																			 && dr.ScheduledWeekdays.Contains(DayOfWeek.Thursday)
																			 && dr.ScheduledWeekdays.Contains(DayOfWeek.Friday))) ?? 0, result.OccupiedDesksCount);
		Assert.AreEqual(2, result.ProjectsInRoom!.Count());
		Assert.AreEqual(room1.Desks?.Count ?? 0, result.DesksInRoom!.Count());
		Assert.IsNotNull(result.ProjectsInRoom!.First().TeamLeaders);
		Assert.IsTrue(result.ProjectsInRoom!.Any(p => p.Name == project1.Name));
		Assert.IsTrue(result.AreaMinLevelPerPerson == 4);

		var resultHotDesks = result.DesksInRoom!.Where(d => d.IsHotDesk == true).ToList();
		Assert.AreEqual(2, resultHotDesks.Count);
		Assert.AreEqual(hotDesk1.Id, resultHotDesks[0].Id);
		Assert.AreEqual(hotDesk2.Id, resultHotDesks[1].Id);
	}

	[Test]
	public async Task ShouldWorkForNotExistingRoom()
	{
		// given
		var building1 = new BuildingEntity { Name = "F1" };
		var floor1 = new FloorEntity { Building = building1, FloorNumber = 0 };
		var room1 = new RoomEntity { Area = 22.5m, Name = "001", Floor = floor1 };
		_context.Desks.AddRange(new DeskEntity { Room = room1, Number = 1 },
			new DeskEntity { Room = room1, Number = 2 },
			new DeskEntity { Room = room1, Number = 3 });

		await _context.SaveChangesAsync();
			
		// then
		Assert.ThrowsAsync<EntityNotFoundException<RoomEntity>>(async() => await _entityQuery.GetRoomAsync(Guid.NewGuid()));
	}
}

