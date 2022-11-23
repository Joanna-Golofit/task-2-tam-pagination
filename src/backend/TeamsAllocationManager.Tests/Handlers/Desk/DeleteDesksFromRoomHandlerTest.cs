using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Desks.Commands;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Domain.Enums;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Infrastructure.Handlers.Desk;

namespace TeamsAllocationManager.Tests.Handlers.Desk;

[TestFixture]
public class DeleteDesksFromRoomHandlerTest
{
	private readonly ApplicationDbContext _context;
	private readonly DesksRepository _desksRepository;

	public DeleteDesksFromRoomHandlerTest()
	{
		DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: GetType().Name)
			.Options;
		_context = new ApplicationDbContext(options);

		_desksRepository = new DesksRepository(_context);
	}

	[SetUp]
	public void SetupBeforeEachTest()
	{
		_context.ClearDatabase();

		var teamLeader1 = new EmployeeEntity { Name = "Jan", Surname = "Kowalski", Email = "jkowalski@fp.pl", ExternalId = 1 };
		var teamLeader2 = new EmployeeEntity { Name = "Anna", Surname = "Maria", Email = "amaria@fp.pl", ExternalId = 2 };
		var employee = new EmployeeEntity { Name = "Adam", Surname = "Nowak", Email = "anowak@fp.pl", ExternalId = 3 };


		var floor = new FloorEntity { Building = new BuildingEntity { Name = "F1" }, FloorNumber = 0 };
		var room1 = new RoomEntity { Area = 22.5m, Name = "001", Floor = floor };
		_context.Desks.AddRange(
			new DeskEntity { Room = room1, Number = 1 },
			new DeskEntity { Room = room1, Number = 2 },
			new DeskEntity { Room = room1, Number = 3 },
			new DeskEntity { Room = room1, Number = 4 },
			new DeskEntity { Room = room1, Number = 5});

		_context.SaveChanges();
	}

	[Test]
	public async Task ShouldDeleteDesks()
	{
		// given
		int numberOfDesksToDelete = 2;

		RoomEntity roomEntity = _context.Rooms.Include(r => r.Desks).First();
		int expectedInRoom = roomEntity.Desks.Count - numberOfDesksToDelete;
		int expectedInDb = _context.Desks.Count() - numberOfDesksToDelete;
		var deskIdsToDelete = roomEntity.Desks.Select(d => d.Id).Take(numberOfDesksToDelete).ToList();

		var command = new DeleteDesksFromRoomCommand(roomEntity.Id, deskIdsToDelete);

		var commandHandler = new DeleteDesksFromRoomHandler(_desksRepository);

		// when
		bool result = await commandHandler.HandleAsync(command);

		// then
		Assert.IsTrue(result);
		Assert.AreEqual(expectedInRoom, roomEntity.Desks.Count);
		Assert.AreEqual(expectedInDb, _context.Desks.Count());
		Assert.IsFalse(_context.Desks.Any(d => deskIdsToDelete.Contains(d.Id)));
	}

	[Test]
	public async Task ShouldNotDeleteDesk()
	{
		//given
		var command = new DeleteDesksFromRoomCommand(Guid.NewGuid(), new List<Guid> { Guid.NewGuid() });

		var commandHandler = new DeleteDesksFromRoomHandler(_desksRepository);

		// when
		bool result = await commandHandler.HandleAsync(command);

		// then
		Assert.IsFalse(result);
	}
}
