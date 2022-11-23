using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Room;
using TeamsAllocationManager.Infrastructure.EntityQueries;
using TeamsAllocationManager.Mapper.Profiles;
using TeamsAllocationManager.Tests.Helpers;

namespace TeamsAllocationManager.Tests.EntityQueries.RoomEntityQueries;

[TestFixture]
public class GetAllRoomsAsync_RoomEntityQueryTests
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly RoomEntityQuery _entityQuery;

	public GetAllRoomsAsync_RoomEntityQueryTests()
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
	public async Task ShouldReturnProperlySetRooms()
	{
		// given
		var employee1 = new EmployeeEntity { Name = "Adam", Surname = "Nowak", Email = "anowak@fp.pl", ExternalId = 1, UserLogin = "anowak" };
		var building1 = new BuildingEntity { Name = "F1" };
		var floor1 = new FloorEntity { Building = building1, FloorNumber = 0 };
		var room1 = new RoomEntity { Area = 22.5m, Name = "001", Floor = floor1 };
		_context.Desks.AddRange(
			new DeskEntity { Room = room1, Number = 1, IsEnabled = true },
			new DeskEntity { Room = room1, Number = 2, IsEnabled = true },
			new DeskEntity { Room = room1, Number = 3, IsEnabled = true });

		var building2 = new BuildingEntity { Name = "F2" };
		var floor2 = new FloorEntity { Building = building2, FloorNumber = 2 };
		var room2 = new RoomEntity { Area = 37.2m, Name = "001", Floor = floor2 };
		_context.Desks.AddRange(
			new DeskEntity { Room = room2, Number = 1, IsEnabled = true },
			new DeskEntity { Room = room2, Number = 2, IsEnabled = true },
			new DeskEntity { Room = room2, Number = 3, IsEnabled = true },
			new DeskEntity { Room = room2, Number = 4, IsEnabled = true });

		var floor3 = new FloorEntity { Building = building2, FloorNumber = 1 };
		var room3 = new RoomEntity { Area = 44.3m, Name = "002", Floor = floor3 };

		var deskWithReservation = new DeskEntity { Room = room3, Number = 3, IsEnabled = true };
		var deskReservation = DeskReservationEntity.NewDeskReservation(DateTime.Now, new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday }, deskWithReservation, employee1);
		deskWithReservation.DeskReservations.Add(deskReservation);

		_context.Desks.AddRange(
			new DeskEntity { Room = room3, Number = 1, IsEnabled = true },
			deskWithReservation,
			new DeskEntity { Room = room3, Number = 3, IsEnabled = true },
			new DeskEntity { Room = room3, Number = 4, IsHotDesk = true, IsEnabled = true },
			new DeskEntity { Room = room3, Number = 5, IsEnabled = true });
		_context.SaveChanges();

		// when
		RoomsDto result = await _entityQuery.GetAllRoomsAsync();

		// then
		Assert.AreEqual(2, result.MaxFloor);
		Assert.AreEqual(2, result.Buildings.Count());
		Assert.IsTrue(result.Buildings.Any(b => b.Name == building1.Name));
		Assert.IsTrue(result.Buildings.Any(b => b.Name == building2.Name));
		Assert.AreEqual(3, result.Rooms.Count());
		Assert.AreEqual(3, result.Rooms.Single(r => r.Name == room1.Name && r.Building.Name == building1.Name).Capacity);
		Assert.AreEqual(1, result.Rooms.Single(r => r.Name == room3.Name && r.Building.Name == building2.Name).OccupiedDesksCount);
		Assert.AreEqual(3, result.Rooms.Single(r => r.Name == room3.Name && r.Building.Name == building2.Name).FreeDesksCount);
		Assert.IsTrue(result.AreaMinLevelPerPerson == 4);
	}

	[Test]
	public async Task ShouldReturnEmptyResponseWhenDbIsEmpty()
	{
		// given

		// when
		RoomsDto result = await _entityQuery.GetAllRoomsAsync();

		// then 
		Assert.Zero(result.Rooms.Count());
		Assert.Zero(result.MaxFloor);
		Assert.Zero(result.Buildings.Count());
	}
}
