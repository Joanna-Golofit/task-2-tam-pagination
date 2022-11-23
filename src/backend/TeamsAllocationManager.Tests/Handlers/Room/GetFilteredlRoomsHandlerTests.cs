using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using TeamsAllocationManager.Contracts.Room.Queries;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Common;
using TeamsAllocationManager.Dtos.Enums;
using TeamsAllocationManager.Dtos.Room;
using TeamsAllocationManager.Infrastructure.EntityQueries;
using TeamsAllocationManager.Infrastructure.Exceptions;
using TeamsAllocationManager.Infrastructure.Handlers.Room;
using TeamsAllocationManager.Mapper.Profiles;
using TeamsAllocationManager.Tests.Helpers;

namespace TeamsAllocationManager.Tests.Handlers.Room;

[TestFixture]
public class GetFilteredRoomsHandlerTests
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly IEnumerable<RoomEntityQuery> _entityQueries;
	public GetFilteredRoomsHandlerTests()
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

		var teamLeader1 = new EmployeeEntity { Name = "Jan", Surname = "Kowalski", Email = "jkowalski@fp.pl", ExternalId = 1, UserLogin = "jkowalski" };

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

		_context.Desks.AddRange(new DeskEntity { Room = room2, Number = 1 },
			new DeskEntity { Room = room2, Number = 3, IsEnabled = true, IsHotDesk = false },
			new DeskEntity { Room = room2, Number = 4, IsEnabled = true, IsHotDesk = false },
			new DeskEntity { Room = room2, Number = 5, IsEnabled = true, IsHotDesk = false });

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
	public async Task ShouldReturnAllRooms_WithEmptyFilter()
	{
		//given
		var pagedOptions = new RoomsQueryFilterDto>();
		var query = new GetFilteredRoomsQuery(pagedOptions, _entityQueries);

		var queryHandler = new GetFilteredRoomsHandler();

		// when
		var queryResult = await queryHandler.HandleAsync(query);
		var result = queryResult.Rooms.Payload!.Select(r => r.Id).ToList();

		// then
		Assert.AreEqual(_context.Rooms.Count(), queryResult.Rooms.Payload!.Count());
		Assert.IsTrue(new HashSet<Guid>(_context.Rooms.Select(x => x.Id)).SetEquals(queryResult.Rooms.Payload!.Select(r => r.Id)));
		Assert.AreEqual(_context.Buildings.Count(), queryResult.Buildings.Count());
		Assert.IsTrue(new HashSet<Guid>(_context.Buildings.Select(x => x.Id)).SetEquals(queryResult.Buildings.Select(r => r.Id)));
		Assert.IsTrue(queryResult.AreaMinLevelPerPerson == 4);
		Assert.IsTrue(queryResult.ErrorCode == ErrorCodes.NoError);
		Assert.IsTrue(queryResult.MaxFloor == 1);
	}

	[Test]
	public async Task ShouldReturnResults_WithBuildingAndRoomNameFilter()
	{
		//given
		var expectedRoomName = "1";
		var expectedBuildingName = "F1";
		var pagedOptions = new RoomsQueryFilterDto>()
		{
			Filters = new RoomsQueryFilterDto()
			{
				RoomName = expectedRoomName,
				BuildingName = expectedBuildingName
			}
		};

		var query = new GetFilteredRoomsQuery(pagedOptions, _entityQueries);

		var queryHandler = new GetFilteredRoomsHandler();

		// when
		var queryResult = await queryHandler.HandleAsync(query);

		// then
		Assert.AreEqual(1, queryResult.Rooms.Payload!.Count());
		Assert.IsTrue(new HashSet<Guid>(_context.Rooms.Where(r => r.Name == expectedRoomName && r.Floor.Building.Name == expectedBuildingName).Select(x => x.Id)).SetEquals(queryResult.Rooms.Payload!.Select(r => r.Id)));
		Assert.AreEqual(_context.Buildings.Count(), queryResult.Buildings.Count());
		Assert.IsTrue(new HashSet<Guid>(_context.Buildings.Select(x => x.Id)).SetEquals(queryResult.Buildings.Select(r => r.Id)));
		Assert.IsTrue(queryResult.AreaMinLevelPerPerson == 4);
		Assert.IsTrue(queryResult.ErrorCode == ErrorCodes.NoError);
		Assert.IsTrue(queryResult.MaxFloor == 0);
	}

	[Test]
	public async Task ShouldReturnResults_WithPagination()
	{
		//given
		var pagedOptions = new RoomsQueryFilterDto>()
		{
			PageNumber = 0,
			PageSize = 2
		};

		var query = new GetFilteredRoomsQuery(pagedOptions, _entityQueries);

		var queryHandler = new GetFilteredRoomsHandler();

		// when
		var queryResult = await queryHandler.HandleAsync(query);

		// then
		Assert.AreEqual(2, queryResult.Rooms.Payload!.Count());
		Assert.AreEqual(8, queryResult.Rooms.Count);
	}

	[Test]
	public async Task ShouldReturnResults_WithCapacityAndFreeDesksFilter()
	{
		//given
		var pagedOptions = new RoomsQueryFilterDto>()
		{
			Filters = new RoomsQueryFilterDto()
			{
				CapacityRange = new CapacityRangeFilterDto()
				{
					Min = 1,
					Max = 4
				},
				FreeDesksRange = new FreeDesksRangeFilterDto()
				{
					Min = 1,
					Max = 4
				}
			}
		};

		var query = new GetFilteredRoomsQuery(pagedOptions, _entityQueries);

		var queryHandler = new GetFilteredRoomsHandler();

		// when
		var queryResult = await queryHandler.HandleAsync(query);

		// then
		Assert.AreEqual(1, queryResult.Rooms.Payload!.Count());
		Assert.AreEqual(1, queryResult.Rooms.Count);
	}

	[Test]
	public void ShouldThrowException_WithCapacityRangeFilterSetIncorrectly()
	{
		//given
		var pagedOptions = new RoomsQueryFilterDto>()
		{
			Filters = new RoomsQueryFilterDto()
			{
				CapacityRange = new CapacityRangeFilterDto()
				{
					Min = 2,
					Max = 1
				}
			}
		};

		var query = new GetFilteredRoomsQuery(pagedOptions, _entityQueries);

		var queryHandler = new GetFilteredRoomsHandler();

		// when
		var exception = Assert.ThrowsAsync<InvalidArgumentException>(async () => await queryHandler.HandleAsync(query));

		// then
		exception.ShouldNotBeNull();
		exception.Code.ShouldBe(4);
		exception.Status.ShouldBe("invalid_argument_exception");
		exception.Message.ShouldBe(ExceptionMessage.GetMessage(ExceptionMessage.InvalidArgument_IncorrectDeskCapacityRangeValues));
	}

	[Test]
	public void ShouldThrowException_WithOccupiedDeskRangeFilterSetIncorrectly()
	{
		//given
		var pagedOptions = new RoomsQueryFilterDto>()
		{
			Filters = new RoomsQueryFilterDto()
			{
				FreeDesksRange = new FreeDesksRangeFilterDto()
				{
					Min = 2,
					Max = 1
				}
			}
		};

		var query = new GetFilteredRoomsQuery(pagedOptions, _entityQueries);

		var queryHandler = new GetFilteredRoomsHandler();

		// when
		var exception = Assert.ThrowsAsync<InvalidArgumentException>(async () => await queryHandler.HandleAsync(query));

		// then
		exception.ShouldNotBeNull();
		exception.Code.ShouldBe(4);
		exception.Status.ShouldBe("invalid_argument_exception");
		exception.Message.ShouldBe(ExceptionMessage.GetMessage(ExceptionMessage.InvalidArgument_IncorrectFreeDeskRangeValues));
	}
}
