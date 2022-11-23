using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Desks.Commands;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Desk;
using TeamsAllocationManager.Infrastructure.Handlers.Desk;

namespace TeamsAllocationManager.Tests.Handlers.Desk;

[TestFixture]
public class AddDesksHandlerTests
{
	private readonly ApplicationDbContext _context;
	private readonly DesksRepository _desksRepository;
	private readonly RoomRepository _roomRepository;

	public AddDesksHandlerTests()
	{
		DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: GetType().Name)
			.Options;
		_context = new ApplicationDbContext(options);
		_roomRepository = new RoomRepository(_context);
		_desksRepository = new DesksRepository(_context);
	}

	[SetUp]
	public void SetupBeforeEachTest()
	{
		_context.ClearDatabase();

		var floor = new FloorEntity { Building = new BuildingEntity { Name = "F1" }, FloorNumber = 0 };
		var room1 = new RoomEntity { Area = 22.5m, Name = "001", Floor = floor };
		_context.Desks.AddRange(
			new DeskEntity { Room = room1, Number = 2 },
			new DeskEntity { Room = room1, Number = 4 },
			new DeskEntity { Room = room1, Number = 7 },
			new DeskEntity { Room = room1, Number = 8 },
			new DeskEntity { Room = room1, Number = 9 });

		_context.SaveChanges();
	}

	[Test]
	public async Task ShouldAdd1DeskAtFirstFreeNumberCase1()
	{
		// given
		int expectedDesksCount = _context.Desks.Count() + 1;
		int expectedNumber = 1;

		var dto = new AddDesksDto
		{
			FirstDeskNumber = 1,
			NumberOfDesks = 1,
			RoomId = _context.Rooms.First().Id
		};

		var command = new AddDesksCommand(dto);

		var commandHandler = new AddDesksHandler(_roomRepository, _desksRepository);

		// when
		bool result = await commandHandler.HandleAsync(command);

		// then
		Assert.IsTrue(result);
		Assert.IsTrue(_context.Desks.Count() == expectedDesksCount);
		Assert.IsTrue(_context.Desks.GroupBy(d => d.Number).Select(gr => gr.Key).Count() == expectedDesksCount);
		Assert.IsTrue(_context.Desks.Any(d => d.Number == expectedNumber));
	}

	[Test]
	public async Task ShouldAdd1DeskAtFirstFreeNumberCase2()
	{
		// given
		int expectedDesksCount = _context.Desks.Count() + 1;
		int expectedNumber = 6;

		var dto = new AddDesksDto
		{
			FirstDeskNumber = 6,
			NumberOfDesks = 1,
			RoomId = _context.Rooms.First().Id
		};

		var command = new AddDesksCommand(dto);

		var commandHandler = new AddDesksHandler(_roomRepository, _desksRepository);

		// when
		bool result = await commandHandler.HandleAsync(command);

		// then
		Assert.IsTrue(result);
		Assert.IsTrue(_context.Desks.Count() == expectedDesksCount);
		Assert.IsTrue(_context.Desks.GroupBy(d => d.Number).Select(gr => gr.Key).Count() == expectedDesksCount);
		Assert.IsTrue(_context.Desks.Any(d => d.Number == expectedNumber));
	}

	[Test]
	public async Task ShouldAdd1DeskAtFirstFreeNumberCase3()
	{
		// given
		int expectedDesksCount = _context.Desks.Count() + 1;
		int expectedNumber = 10;

		var dto = new AddDesksDto
		{
			FirstDeskNumber = 7,
			NumberOfDesks = 1,
			RoomId = _context.Rooms.First().Id
		};

		var command = new AddDesksCommand(dto);

		var commandHandler = new AddDesksHandler(_roomRepository, _desksRepository);

		// when
		bool result = await commandHandler.HandleAsync(command);

		// then
		Assert.IsTrue(result);
		Assert.IsTrue(_context.Desks.Count() == expectedDesksCount);
		Assert.IsTrue(_context.Desks.GroupBy(d => d.Number).Select(gr => gr.Key).Count() == expectedDesksCount);
		Assert.IsTrue(_context.Desks.Any(d => d.Number == expectedNumber));
	}

	[Test]
	public async Task ShouldAddMultileDesksAtFirstFreeNumberCase1()
	{
		// given
		var expectedNumbers = new List<int> { 1, 3, 5, 6 };
		int numberOfDesksToAdd = expectedNumbers.Count;
		int expectedDesksCount = _context.Desks.Count() + numberOfDesksToAdd;

		var dto = new AddDesksDto
		{
			FirstDeskNumber = 1,
			NumberOfDesks = numberOfDesksToAdd,
			RoomId = _context.Rooms.First().Id
		};

		var command = new AddDesksCommand(dto);

		var commandHandler = new AddDesksHandler(_roomRepository, _desksRepository);

		// when
		bool result = await commandHandler.HandleAsync(command);

		// then
		Assert.IsTrue(result);
		Assert.IsTrue(_context.Desks.Count() == expectedDesksCount);
		Assert.IsTrue(_context.Desks.GroupBy(d => d.Number).Select(gr => gr.Key).Count() == expectedDesksCount);
		Assert.IsTrue(_context.Desks.Count(d => expectedNumbers.Contains(d.Number)) == numberOfDesksToAdd);
	}

	[Test]
	public async Task ShouldAddMultileDesksDeskAtFirstFreeNumberCase2()
	{
		// given
		var expectedNumbers = new List<int> { 5, 6, 10, 11 };
		int numberOfDesksToAdd = expectedNumbers.Count;
		int expectedDesksCount = _context.Desks.Count() + numberOfDesksToAdd;

		var dto = new AddDesksDto
		{
			FirstDeskNumber = 4,
			NumberOfDesks = numberOfDesksToAdd,
			RoomId = _context.Rooms.First().Id
		};

		var command = new AddDesksCommand(dto);

		var commandHandler = new AddDesksHandler(_roomRepository, _desksRepository);

		// when
		bool result = await commandHandler.HandleAsync(command);

		// then
		Assert.IsTrue(result);
		Assert.IsTrue(_context.Desks.Count() == expectedDesksCount);
		Assert.IsTrue(_context.Desks.GroupBy(d => d.Number).Select(gr => gr.Key).Count() == expectedDesksCount);
		Assert.IsTrue(_context.Desks.Count(d => expectedNumbers.Contains(d.Number)) == numberOfDesksToAdd);
	}

	[Test]
	public async Task ShouldAddMultileDesksDeskAtFirstFreeNumberCase3()
	{
		// given
		var expectedNumbers = new List<int> { 10, 11, 12, 13, 14 };
		int numberOfDesksToAdd = expectedNumbers.Count;
		int expectedDesksCount = _context.Desks.Count() + numberOfDesksToAdd;

		var dto = new AddDesksDto
		{
			FirstDeskNumber = 7,
			NumberOfDesks = numberOfDesksToAdd,
			RoomId = _context.Rooms.First().Id
		};

		var command = new AddDesksCommand(dto);

		var commandHandler = new AddDesksHandler(_roomRepository, _desksRepository);

		// when
		bool result = await commandHandler.HandleAsync(command);

		// then
		Assert.IsTrue(result);
		Assert.IsTrue(_context.Desks.Count() == expectedDesksCount);
		Assert.IsTrue(_context.Desks.GroupBy(d => d.Number).Select(gr => gr.Key).Count() == expectedDesksCount);
		Assert.IsTrue(_context.Desks.Count(d => expectedNumbers.Contains(d.Number)) == numberOfDesksToAdd);
	}

	[Test]
	public async Task ShouldAddMultileDesksDeskAtFirstFreeNumberCase4()
	{
		// given
		var expectedNumbers = new List<int> { 14, 15 };
		int numberOfDesksToAdd = expectedNumbers.Count;
		int expectedDesksCount = _context.Desks.Count() + numberOfDesksToAdd;

		var dto = new AddDesksDto
		{
			FirstDeskNumber = 14,
			NumberOfDesks = numberOfDesksToAdd,
			RoomId = _context.Rooms.First().Id
		};

		var command = new AddDesksCommand(dto);

		var commandHandler = new AddDesksHandler(_roomRepository, _desksRepository);

		// when
		bool result = await commandHandler.HandleAsync(command);

		// then
		Assert.IsTrue(result);
		Assert.IsTrue(_context.Desks.Count() == expectedDesksCount);
		Assert.IsTrue(_context.Desks.GroupBy(d => d.Number).Select(gr => gr.Key).Count() == expectedDesksCount);
		Assert.IsTrue(_context.Desks.Count(d => expectedNumbers.Contains(d.Number)) == numberOfDesksToAdd);
	}

	[Test]
	public async Task ShouldAddMultileDesksAtFirstFreeNumberCase5()
	{
		// given			
		var expectedNumbers = new List<int> { 3, 5, 6 };
		int numberOfDesksToAdd = expectedNumbers.Count;
		int expectedDesksCount = _context.Desks.Count() + numberOfDesksToAdd;

		var dto = new AddDesksDto
		{
			FirstDeskNumber = 2,
			NumberOfDesks = numberOfDesksToAdd,
			RoomId = _context.Rooms.First().Id
		};

		var command = new AddDesksCommand(dto);

		var commandHandler = new AddDesksHandler(_roomRepository, _desksRepository);

		// when
		bool result = await commandHandler.HandleAsync(command);

		// then
		Assert.IsTrue(result);
		Assert.IsTrue(_context.Desks.Count() == expectedDesksCount);
		Assert.IsTrue(_context.Desks.GroupBy(d => d.Number).Select(gr => gr.Key).Count() == expectedDesksCount);
		Assert.IsTrue(_context.Desks.Count(d => expectedNumbers.Contains(d.Number)) == numberOfDesksToAdd);
	}

	[Test]
	public async Task TestEdgeConditions()
	{
		// given
		var dto1 = new AddDesksDto { FirstDeskNumber = -1, NumberOfDesks = 1, RoomId = _context.Rooms.First().Id };
		var dto2 = new AddDesksDto { FirstDeskNumber = 1, NumberOfDesks = 0, RoomId = _context.Rooms.First().Id };
		var dto3 = new AddDesksDto { FirstDeskNumber = 1, NumberOfDesks = 1, RoomId = Guid.NewGuid() };


		var commandHandler = new AddDesksHandler(_roomRepository, _desksRepository);

		// when
		bool result1 = await commandHandler.HandleAsync(new AddDesksCommand(dto1));
		bool result2 = await commandHandler.HandleAsync(new AddDesksCommand(dto2));
		bool result3 = await commandHandler.HandleAsync(new AddDesksCommand(dto3));

		// then
		Assert.IsFalse(result1);
		Assert.IsFalse(result2);
		Assert.IsFalse(result3);
	}
}
