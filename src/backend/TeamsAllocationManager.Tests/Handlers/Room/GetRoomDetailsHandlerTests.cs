using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Room.Queries;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Enums;
using TeamsAllocationManager.Infrastructure.EntityQueries;
using TeamsAllocationManager.Infrastructure.Handlers.Room;
using TeamsAllocationManager.Mapper.Profiles;
using TeamsAllocationManager.Tests.Helpers;

namespace TeamsAllocationManager.Tests.Handlers.Room;

[TestFixture]
public class GetRoomDetailsHandlerTests
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly IEnumerable<RoomEntityQuery> _entityQueries;

	public GetRoomDetailsHandlerTests()
	{
		DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: GetType().Name)
			.Options;
		_context = new ApplicationDbContext(options);
		_mapper = new MapperConfiguration(mc => mc.AddProfile(new EntityDtoProfile())).CreateMapper();
		_entityQueries = new List<RoomEntityQuery> { new RoomEntityQuery(_context, _mapper) };
	}

	[SetUp]
	public void SetupBeforeEachTest()
	{
		_context.ClearDatabase();
	}

	[Test]
	public async Task Should_Return_Room()
	{
		// given
		var building1 = new BuildingEntity { Name = "F1" };
		var floor1 = new FloorEntity { Building = building1, FloorNumber = 0 };

		var employee1 = new EmployeeEntity { ExternalId = 1, Projects = { new EmployeeProjectEntity { ProjectId = Guid.NewGuid(), Project = new ProjectEntity() { Name = "Project1" } } }, Email = "jkowalski@future-processing.com", UserLogin = "jkowalski" };
		var employee2 = new EmployeeEntity { ExternalId = 2, Projects = { new EmployeeProjectEntity { ProjectId = Guid.NewGuid(), Project = new ProjectEntity() { Name = "Project2" } } }, Email = "pryczek@future-processing.com", UserLogin = "pryczek" };
		var room1 = new RoomEntity
		{
			Id = Guid.NewGuid(),
			Area = 22.5m,
			Name = "1",
			Floor = floor1,
		};

		room1.Desks = new List<DeskEntity>
			{
				new DeskEntity {Id = Guid.NewGuid(), Number = 1, IsEnabled = true },
				DeskHelpers.CreateDeskWithReservationForWholeWeek(room1, 2, employee1),
				DeskHelpers.CreateDeskWithReservationForWholeWeek(room1, 3, employee2),
				new DeskEntity {Id = Guid.NewGuid(), Number = 4, IsHotDesk = true, IsEnabled = true },
				new DeskEntity {Id = Guid.NewGuid(), Number = 5, IsHotDesk = true, IsEnabled = true },
				new DeskEntity {Id = Guid.NewGuid(), Number = 6, IsHotDesk = true, IsEnabled = true }
			};

		_context.Rooms.Add(room1);
		_context.SaveChanges();

		var queryHandler = new GetRoomDetailsHandler();

		// when
		var query = new GetRoomDetailsQuery(_entityQueries, room1.Id);
		var queryResult = await queryHandler.HandleAsync(query);

		// then
		Assert.AreEqual(room1.Id, queryResult.Id);
		Assert.AreEqual(room1.Name, queryResult.Name);
		Assert.AreEqual(room1.Floor.FloorNumber, queryResult.Floor);
		Assert.AreEqual(room1.Floor.Building.Name, queryResult.Building.Name);
		Assert.AreEqual(room1.Desks.Count(), queryResult.DesksInRoom?.Count());
		Assert.AreEqual(room1.Area, queryResult.Area);
		Assert.AreEqual(2, queryResult.OccupiedDesksCount);
		Assert.AreEqual(1, queryResult.FreeDesksCount);
		Assert.AreEqual(2, queryResult.ProjectsInRoom?.Count());
	}
}
