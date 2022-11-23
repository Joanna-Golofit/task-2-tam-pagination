using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Project.Commands;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Project;
using TeamsAllocationManager.Infrastructure.Handlers.Project;

namespace TeamsAllocationManager.Tests.Handlers.Employee;

[TestFixture]
public class RemoveEmployeesFromDesksHandlerTests
{
	private readonly ApplicationDbContext _context;
	private readonly DesksRepository _desksRepository;

	public RemoveEmployeesFromDesksHandlerTests()
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
		var floor = new FloorEntity { Building = new BuildingEntity { Name = "F1" }, FloorNumber = 0 };
		var room1 = new RoomEntity { Area = 22.5m, Name = "001", Floor = floor };
		_context.Desks.Add(new DeskEntity { Room = room1, Number = 2 });

		_context.SaveChangesAsync();
	}

	[Test]
	public async Task ExecuteAsync_AssignsEmployeeToDesk_DtoIdsEqualToDbIds()
	{
		// given
		var dto = new RemoveEmployeesFromDesksDto()
		{
			RoomId = _context.Desks.First().RoomId,
			DeskIds = new List<Guid> {
					_context.Desks.First().Id,
			},
		};

		var command = new RemoveEmployeesFromDesksCommand(dto);

		var commandHandler = new RemoveEmployeesFromDesksHandler(_desksRepository);

		// when
		Assert.DoesNotThrowAsync(async () => await commandHandler.HandleAsync(command));

		// then
		DeskEntity? desk = await _context.Desks.FirstAsync();
	}
}
