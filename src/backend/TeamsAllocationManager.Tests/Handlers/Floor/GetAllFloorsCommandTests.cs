using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Floor.Queries;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Domain.Enums;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Floor;
using TeamsAllocationManager.Infrastructure.Handlers.Floor;
using TeamsAllocationManager.Mapper.Profiles;
using TeamsAllocationManager.Tests.Helpers;

namespace TeamsAllocationManager.Tests.Handlers.Floor;

[TestFixture]
public class GetAllFloorsCommandTests
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly FloorsRepository _floorsRepository;
	private readonly BuildingsRepository _buildingsRepository;

	public GetAllFloorsCommandTests()
	{
		DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: GetType().Name)
			.Options;
		_context = new ApplicationDbContext(options);
		_mapper = new MapperConfiguration(mc => mc.AddProfile(new EntityDtoProfile())).CreateMapper();
		_floorsRepository = new FloorsRepository(_context);
		_buildingsRepository = new BuildingsRepository(_context);
	}

	[SetUp]
	public void SetupBeforeEachTest()
	{
		_context.ClearDatabase();
	}

	[Test]
	public async Task ShouldReturnProperlyGetFloors()
	{
		// given
		var teamLeader1 = new EmployeeEntity { Name = "Jan", Surname = "Kowalski", Email = "jkowalski@fp.pl", ExternalId = 1, UserLogin = "jkowalski" };
		var teamLeader2 = new EmployeeEntity { Name = "Mariusz", Surname = "Szczepañski", Email = "mszczepanski@fp.pl", ExternalId = 2, UserLogin = "mszczepanski" };
		var employee1 = new EmployeeEntity { Name = "Wojtek", Surname = "Golonka", Email = "wgolonka@fp.pl", ExternalId = 3, UserLogin = "wgolonka" };
		var employee2 = new EmployeeEntity { Name = "Anna", Surname = "Maria", Email = "amaria@fp.pl", ExternalId = 4, UserLogin = "amaria" };


		var building1 = new BuildingEntity { Name = "F1" };
		var floor1 = new FloorEntity { Building = building1, FloorNumber = 0 };
		var room1 = new RoomEntity { Area = 22.5m, Name = "001", Floor = floor1 };
		_context.Desks.AddRange(new DeskEntity { Room = room1, Number = 1 },
			DeskHelpers.CreateDeskWithReservationForWholeWeek(room1, 2, teamLeader1),
			DeskHelpers.CreateDeskWithReservationForWholeWeek(room1, 3, employee1),
			DeskHelpers.CreateDeskWithReservationForWholeWeek(room1, 4, employee2),
			new DeskEntity { Room = room1, Number = 5 }); ;

		var floor2 = new FloorEntity { Building = building1, FloorNumber = 1 };
		var room2 = new RoomEntity { Area = 12.8m, Name = "002", Floor = floor2 };
		_context.Desks.AddRange(new DeskEntity { Room = room2, Number = 1 },
			DeskHelpers.CreateDeskWithReservationForWholeWeek(room2, 2, teamLeader2),
			new DeskEntity { Room = room2, Number = 3 },
			new DeskEntity { Room = room2, Number = 4 },
			new DeskEntity { Room = room2, Number = 5 });

		var building2 = new BuildingEntity { Name = "F2" };
		var floor3 = new FloorEntity { Building = building2, FloorNumber = 2 };
		var room3 = new RoomEntity { Area = 37.2m, Name = "001", Floor = floor3 };
		_context.Desks.AddRange(new DeskEntity { Room = room3, Number = 1 },
			DeskHelpers.CreateDeskWithReservationForWholeWeek(room3, 2, employee1),
			DeskHelpers.CreateDeskWithReservationForWholeWeek(room3, 3, employee2),
			new DeskEntity { Room = room3, Number = 4 },
			new DeskEntity { Room = room3, Number = 5 });

		var floor4 = new FloorEntity { Building = building2, FloorNumber = 1 };
		var room4 = new RoomEntity { Area = 44.3m, Name = "002", Floor = floor4 };
		_context.Desks.AddRange(new DeskEntity { Room = room4, Number = 1 },
			new DeskEntity { Room = room4, Number = 2 },
			new DeskEntity { Room = room4, Number = 3 },
			new DeskEntity { Room = room4, Number = 4 },
			new DeskEntity { Room = room4, Number = 5 });

		var room5 = new RoomEntity { Area = 34.9m, Name = "003", Floor = floor4 };
		_context.Desks.AddRange(new DeskEntity { Room = room5, Number = 1 },
			new DeskEntity { Room = room5, Number = 2 },
			new DeskEntity { Room = room5, Number = 3 },
			new DeskEntity { Room = room5, Number = 4 },
			new DeskEntity { Room = room5, Number = 5 });
		_context.SaveChanges();

		var query = new GetAllFloorsQuery();
		var handler = new GetAllFloorsHandler(_floorsRepository, _buildingsRepository, _mapper);

		// when
		FloorsDto result = await handler.HandleAsync(query);

		// then
		Assert.AreEqual(2, result.MaxFloor);
		Assert.AreEqual(2, result.Buildings.Count());
		Assert.IsTrue(result.Buildings.Any(b => b.Name == building1.Name));
		Assert.IsTrue(result.Buildings.Any(b => b.Name == building2.Name));

		Assert.AreEqual(4, result.Floors.Count());

		FloorDto floor = result.Floors.Single(f => f.Building.Name == building1.Name && f.Floor == floor1.FloorNumber);
		Assert.AreEqual(22.5, floor.Area);
		Assert.AreEqual(5, floor.Capacity);
		Assert.AreEqual(3, floor.OccupiedDesks);
		Assert.AreEqual(1, floor.RoomCount);
		floor = result.Floors.Single(f => f.Building.Name == building1.Name && f.Floor == floor2.FloorNumber);
		Assert.AreEqual(12.8, floor.Area);
		Assert.AreEqual(5, floor.Capacity);
		Assert.AreEqual(1, floor.OccupiedDesks);
		Assert.AreEqual(1, floor.RoomCount);

		floor = result.Floors.Single(f => f.Building.Name == building2.Name && f.Floor == floor3.FloorNumber);
		Assert.AreEqual(37.2m, floor.Area);
		Assert.AreEqual(5, floor.Capacity);
		Assert.AreEqual(2, floor.OccupiedDesks);
		Assert.AreEqual(1, floor.RoomCount);
		floor = result.Floors.Single(f => f.Building.Name == building2.Name && f.Floor == floor4.FloorNumber);
		Assert.AreEqual(79.2, floor.Area);
		Assert.AreEqual(10, floor.Capacity);
		Assert.AreEqual(0, floor.OccupiedDesks);
		Assert.AreEqual(2, floor.RoomCount);
	}

	[Test]
	public async Task ShouldReturnEmptyResponseWhenDbIsEmpty()
	{
		// given
		var query = new GetAllFloorsQuery();
		var handler = new GetAllFloorsHandler(_floorsRepository, _buildingsRepository, _mapper);

		// when
		FloorsDto result = await handler.HandleAsync(query);

		// then 
		Assert.Zero(result.Floors.Count());
		Assert.Zero(result.MaxFloor);
		Assert.Zero(result.Buildings.Count());
	}
}
