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
using TeamsAllocationManager.Dtos.Room;
using TeamsAllocationManager.Infrastructure.EntityQueries;
using TeamsAllocationManager.Infrastructure.Handlers.Room;
using TeamsAllocationManager.Mapper.Profiles;

namespace TeamsAllocationManager.Tests.Handlers.Room;

[TestFixture]
public class GetAllRoomsHandlerTests
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly IEnumerable<RoomEntityQuery> _entityQueries;
	public GetAllRoomsHandlerTests()
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

		var building1 = new BuildingEntity { Name = "F1" };
		var building2 = new BuildingEntity { Name = "F2" };
		var floor1 = new FloorEntity { Building = building1, FloorNumber = 0 };
		var floor2 = new FloorEntity { Building = building1, FloorNumber = 1 };
		var floor3 = new FloorEntity { Building = building2, FloorNumber = 0 };
		var floor4 = new FloorEntity { Building = building2, FloorNumber = 1 };

		var room1 = new RoomEntity { Area = 22.5m, Name = "1", Floor = floor1 };
		var room2 = new RoomEntity { Area = 22.5m, Name = "2", Floor = floor1 };
		var room3 = new RoomEntity { Area = 22.5m, Name = "3", Floor = floor2 };
		var room4 = new RoomEntity { Area = 22.5m, Name = "4", Floor = floor2 };
		var room5 = new RoomEntity { Area = 22.5m, Name = "5", Floor = floor3 };
		var room6 = new RoomEntity { Area = 22.5m, Name = "6", Floor = floor3 };
		var room7 = new RoomEntity { Area = 22.5m, Name = "7", Floor = floor4 };
		var room8 = new RoomEntity { Area = 22.5m, Name = "8", Floor = floor4 };

		_context.Rooms.AddRange(new List<RoomEntity> {
			room1,
			room2,
			room3,
			room4,
			room5,
			room6,
			room7,
			room8
			}
		);

		_context.SaveChanges();
	}

	[Test]
	public async Task Should_Return_AllRooms()
	{
		//given
		var query = new GetAllRoomsQuery(_entityQueries);
		var queryHandler = new GetAllRoomsHandler();

		// when
		RoomsDto queryResult = await queryHandler.HandleAsync(query);
		var result = queryResult.Rooms.Select(r => r.Id).ToList();

		// then
		Assert.AreEqual(_context.Rooms.Count(), queryResult.Rooms.Count());
		Assert.IsTrue(new HashSet<Guid>(_context.Rooms.Select(x => x.Id)).SetEquals(queryResult.Rooms.Select(r => r.Id)));
		Assert.AreEqual(_context.Buildings.Count(), queryResult.Buildings.Count());
		Assert.IsTrue(new HashSet<Guid>(_context.Buildings.Select(x => x.Id)).SetEquals(queryResult.Buildings.Select(r => r.Id)));
		Assert.IsTrue(queryResult.AreaMinLevelPerPerson == 4);
		Assert.IsTrue(queryResult.ErrorCode == ErrorCodes.NoError);
		Assert.IsTrue(queryResult.MaxFloor == 1);
	}
}
