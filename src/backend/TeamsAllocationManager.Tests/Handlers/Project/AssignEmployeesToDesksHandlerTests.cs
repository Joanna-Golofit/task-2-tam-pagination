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
using TeamsAllocationManager.Dtos.Enums;
using TeamsAllocationManager.Dtos.Project;
using TeamsAllocationManager.Infrastructure.Handlers.Project;

namespace TeamsAllocationManager.Tests.Handlers.Employee;

[TestFixture]
public class AssignEmployeesToDesksHandlerTests
{
	private readonly ApplicationDbContext _context;
	private readonly DesksRepository _desksRepository;

	public AssignEmployeesToDesksHandlerTests()
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

		var project1 = new ProjectEntity
		{
			Id = Guid.NewGuid(),
			Name = "Project 1", EndDate = DateTime.Now.AddDays(100), ExternalId = 1,
		};
		_context.Projects.Add(project1);

		_context.SaveChangesAsync();
	}

	[Test]
	public async Task ExecuteAsync_AssignsEmployeeToDesk_DtoIdsEqualToDbIds()
	{
		// given
		var employeeId = Guid.NewGuid();
		_context.Employees.Add(new EmployeeEntity { Id = employeeId, Name = "Adam", Surname = "Nowak", Email = "anowak@fp.pl", ExternalId = 1, UserLogin = "anowak"});
		_context.SaveChanges();

		var dto = new AssignEmployeesToDesksDto()
		{
			ProjectId = _context.Projects.First().Id,
			RoomId = _context.Desks.First().RoomId,
			DeskEmployeeDtos = new List<DeskEmployeeDto>
			{
				new DeskEmployeeDto
				{
					DeskId = _context.Desks.First().Id,
					EmployeeId = employeeId,
				}
			}

		};

		var command = new AssignEmployeesToDesksCommand(dto);
		var commandHandler = new AssignEmployeesToDesksHandler(_context, _desksRepository);

		// when
		Assert.DoesNotThrowAsync(async() => await commandHandler.HandleAsync(command));
			
		// then
		DeskEntity? desk = await _context.Desks.FirstAsync();
		Assert.AreEqual(employeeId, dto.DeskEmployeeDtos.Single().EmployeeId);
		Assert.AreEqual(desk.RoomId, dto.RoomId);
	}
}
