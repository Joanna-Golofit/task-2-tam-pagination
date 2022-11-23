using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Desks.Commands;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Infrastructure.Handlers.Desk;
using TeamsAllocationManager.Tests.Helpers;

namespace TeamsAllocationManager.Tests.Handlers.Desk;

[TestFixture]
public class ReleaseEmployeesDesksCommandHandlerTests
{
	private readonly ApplicationDbContext _context;
	private readonly IEmployeesRepository _employeesRepository;
	private readonly IDesksRepository _desksRepository;

	public ReleaseEmployeesDesksCommandHandlerTests()
	{
		DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: GetType().Name)
			.Options;
		_context = new ApplicationDbContext(options);
		_employeesRepository = new EmployeesRepository(_context);
		_desksRepository = new DesksRepository(_context);
	}

	[SetUp]
	public void SetupBeforeEachTest()
	{
		_context.ClearDatabase();

		var employee1 = new EmployeeEntity
		{
			Id = Guid.NewGuid(),
			Name = "Katarzyna",
			Surname = "Nowak",
			Email = "knowak@fp.pl",
			ExternalId = 2,
			UserLogin = "knowak"
		};

		var employee2 = new EmployeeEntity
		{
			Id = Guid.NewGuid(),
			Name = "Andrzej",
			Surname = "Sztacheta",
			Email = "asztacheta@fp.pl",
			ExternalId = 3,
			UserLogin = "asztacheta"
		};

		_context.Employees.AddRange(employee1, employee2);

		var floor = new FloorEntity { Building = new BuildingEntity { Name = "F1" }, FloorNumber = 0 };

		var room1 = new RoomEntity { Area = 22.5m, Name = "001", Floor = floor };

		_context.Desks.AddRange(
			DeskHelpers.CreateDeskWithReservation(room1, 1, employee1),
			DeskHelpers.CreateDeskWithReservation(room1, 2, employee1),
			DeskHelpers.CreateDeskWithReservation(room1, 2, employee2),
			new DeskEntity { Room = room1, Number = 3 }
		);

		_context.SaveChanges();
	}

	[Test]
	public async Task ShouldReleaseAllEmployeeDesks()
	{
		var employeeEntity = _context.Employees.First();

		var command = new ReleaseEmployeesDesksCommand(new[] { employeeEntity.Id });
		var commandHandler = new ReleaseEmployeesDesksCommandHandler(_desksRepository, _employeesRepository, _context);

		// Act
		await commandHandler.HandleAsync(command);

		// Assert
		employeeEntity = _context.Employees.First();
		Assert.IsEmpty(employeeEntity.EmployeeDeskReservations);
	}

	[Test]
	public async Task ShouldNotReleaseOtherEmployeesDesks()
	{
		var command = new ReleaseEmployeesDesksCommand(new[] { _context.Employees.First().Id });
		var commandHandler = new ReleaseEmployeesDesksCommandHandler(_desksRepository, _employeesRepository, _context);

		// Act
		await commandHandler.HandleAsync(command);

		// Assert
		Assert.IsNotEmpty(_context.Employees.Last().EmployeeDeskReservations);
	}
}
