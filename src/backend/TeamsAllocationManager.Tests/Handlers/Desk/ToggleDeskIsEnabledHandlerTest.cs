using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using TeamsAllocationManager.Contracts.Desks.Commands;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Desk;
using TeamsAllocationManager.Infrastructure.Handlers.Desk;
using TeamsAllocationManager.Tests.Helpers;

namespace TeamsAllocationManager.Tests.Handlers.Desk;

[TestFixture]
public class ToggleDeskIsEnabledHandlerTest
{
	private readonly ApplicationDbContext _context;
	private readonly Mock<IDesksRepository> _desksRepository = new Mock<IDesksRepository>();

	public ToggleDeskIsEnabledHandlerTest()
	{
		DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: GetType().Name)
			.Options;
		_context = new ApplicationDbContext(options);
	}

	[SetUp]
	public void SetupBeforeEachTest()
	{
		_context.ClearDatabase();

		var employee = new EmployeeEntity { Name = "Katarzyna", Surname = "Nowak", Email = "knowak@fp.pl", ExternalId = 2, UserLogin = "knowak" };
		_context.Employees.AddRange(employee);

		var floor = new FloorEntity { Building = new BuildingEntity { Name = "F1" }, FloorNumber = 0 };
		var room = new RoomEntity { Area = 22.5m, Name = "001", Floor = floor, Id = new Guid("c805d770-d005-4f3b-ab5d-36166b4eef56") };

		var desk = DeskHelpers.CreateDeskWithReservation(room, 2, employee);
		desk.IsEnabled = true;
		desk.Id = new Guid("e2f2cfe5-bff7-4681-920a-21dbd25d9016");

		_context.Desks.AddRange(
			new DeskEntity { Room = room, Number = 1 },
			desk);

		_context.SaveChanges();
	}

	[Test]
	public void ShouldToggleIsEnabledSingleDesk()
	{
		// given
		DeskEntity toToggleEnable = _context.Desks.First(d => d.DeskReservations.Any(dr => dr.IsSchedule));
		ToggleDeskIsEnabledDto dto = new ToggleDeskIsEnabledDto();
		List<Guid> DesksIdsToEnableList = new List<Guid>();
		DesksIdsToEnableList.Add(new Guid("e2f2cfe5-bff7-4681-920a-21dbd25d9016"));
		dto.DesksIds = DesksIdsToEnableList;
		_desksRepository
			.Setup(r => r.GetDesks(It.IsAny<IEnumerable<Guid>>()))
			.ReturnsAsync(new DeskEntity[] { toToggleEnable });

		var command = new ToggleDeskIsEnabledCommand(dto);
		var commandHandler = new ToggleDeskIsEnabledHandler(_desksRepository.Object);

		// when
		Assert.DoesNotThrowAsync(async () => await commandHandler.HandleAsync(command));

		// then
		Assert.IsTrue(_context.Desks.First(d => d.Id == toToggleEnable.Id).IsEnabled == false);
		Assert.IsTrue(_context.Desks.First(d => d.Id == toToggleEnable.Id).DeskReservations.Where(dr => dr.IsSchedule).Count() == 0);
	}
}
