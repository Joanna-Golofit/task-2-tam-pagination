using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Employee.Commands;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Database.Repositories.Interfaces;
using TeamsAllocationManager.Domain.Enums;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Employee;
using TeamsAllocationManager.Infrastructure.Handlers.Employee;
using TeamsAllocationManager.Mapper.Profiles;
using TeamsAllocationManager.Tests.Helpers;

namespace TeamsAllocationManager.Tests.Handlers.Employee;

[TestFixture]
public class UpdateEmployeeWorkspaceTypeHandlerTests
{
	private readonly ApplicationDbContext _applicationDbContext;
	private readonly IMapper _mapper;
	private readonly EmployeeWorkingTypeHistoryRepository _employeeWorkingTypeHistoryRepository;
	private readonly EmployeesRepository _employeeRepository;
	private readonly IDesksRepository _desksRepository;

	public UpdateEmployeeWorkspaceTypeHandlerTests()
	{
		DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: GetType().Name)
			.Options;
		_applicationDbContext = new ApplicationDbContext(options);
		_mapper = new MapperConfiguration(mc => mc.AddProfile(new EntityDtoProfile())).CreateMapper();
		_employeeWorkingTypeHistoryRepository = new EmployeeWorkingTypeHistoryRepository(_applicationDbContext);
		_employeeRepository = new EmployeesRepository(_applicationDbContext);
		_desksRepository = new DesksRepository(_applicationDbContext);
	}

	[Test]
	public async Task ShouldUpdateEmployeeWorkspaceTypeWithHistory()
	{
		//given
		_applicationDbContext.ClearDatabase();

		var employee1 = new EmployeeEntity { Name = "Jan", Surname = "Kowalski", Email = "jkowalski@future-processing.com", WorkspaceType = WorkspaceType.Office, ExternalId = 0, UserLogin = "jkowalski" };
		var employee2 = new EmployeeEntity { Name = "Adam", Surname = "Nowak", Email = "anowak@future-processing.com", WorkspaceType = WorkspaceType.Remote, ExternalId = 1, UserLogin = "anowak" };

		_applicationDbContext.Employees.AddRange(employee1, employee2);
		_applicationDbContext.SaveChanges();

		var dto1 = new UpdateEmployeeWorkspaceTypeDto { EmployeeId = employee1.Id, WorkspaceType = Dtos.Enums.WorkspaceType.Remote };
		var dto2 = new UpdateEmployeeWorkspaceTypeDto { EmployeeId = employee2.Id, WorkspaceType = Dtos.Enums.WorkspaceType.Office };

		var command1 = new UpdateEmployeesWorkspaceTypesCommand(new List<UpdateEmployeeWorkspaceTypeDto> { dto1 });
		var command2 = new UpdateEmployeesWorkspaceTypesCommand(new List<UpdateEmployeeWorkspaceTypeDto> { dto2 });

		var commandHandler = new UpdateEmployeesWorkspaceTypesHandler(_employeeRepository, _employeeWorkingTypeHistoryRepository, _desksRepository, _mapper);

		//when
		bool result1 = await commandHandler.HandleAsync(command1);
		bool result2 = await commandHandler.HandleAsync(command2);

		//then
		Assert.IsTrue(result1);
		Assert.IsTrue(result2);
		var workspaceTypesHistory = _applicationDbContext.EmployeeWorkingTypeHistory.ToList();
		workspaceTypesHistory.ForEach(wth =>
		{
			Assert.IsNotNull(wth.From);
			Assert.IsNull(wth.To);
		});
		Assert.AreEqual(WorkspaceType.Remote, _applicationDbContext.Employees.First(e => e.Id == employee1.Id).WorkspaceType);
		Assert.AreEqual(WorkspaceType.Office, _applicationDbContext.Employees.First(e => e.Id == employee2.Id).WorkspaceType);
	}

	[Test]
	public async Task ShouldReturnFalseWhenDatabaseEmpty()
	{
		//given
		_applicationDbContext.ClearDatabase();
		_applicationDbContext.SaveChanges();

		var dto2 = new UpdateEmployeeWorkspaceTypeDto { EmployeeId = Guid.NewGuid(), WorkspaceType = Dtos.Enums.WorkspaceType.Office };

		var command = new UpdateEmployeesWorkspaceTypesCommand(new List<UpdateEmployeeWorkspaceTypeDto>{ dto2 });
		var commandHandler = new UpdateEmployeesWorkspaceTypesHandler(_employeeRepository, _employeeWorkingTypeHistoryRepository, _desksRepository, _mapper);

		//when
		bool result1 = await commandHandler.HandleAsync(command);

		//then
		Assert.IsFalse(result1);
	}

	[Test]
	public async Task ShouldReleaseEmployeeDesksWhenChangingWorkspaceTypeToRemote()
	{
		SetupTestData();

		var employeeEntity = _applicationDbContext.Employees.First();

		var command = new UpdateEmployeesWorkspaceTypesCommand(new List<UpdateEmployeeWorkspaceTypeDto>
		{
			new UpdateEmployeeWorkspaceTypeDto { EmployeeId = employeeEntity.Id, WorkspaceType = Dtos.Enums.WorkspaceType.Remote }
		});
		var commandHandler = new UpdateEmployeesWorkspaceTypesHandler(_employeeRepository, _employeeWorkingTypeHistoryRepository, _desksRepository, _mapper);

		// Act
		await commandHandler.HandleAsync(command);

		// Assert
		Assert.IsEmpty(_applicationDbContext.Employees.First().EmployeeDeskReservations);
	}

	[Test]
	public async Task ShouldNotReleaseEmployeeDesksWhenChangingWorkspaceTypeToPartiallyRemote()
	{
		SetupTestData();

		var employeeEntity = _applicationDbContext.Employees.First();

		var command = new UpdateEmployeesWorkspaceTypesCommand(new List<UpdateEmployeeWorkspaceTypeDto>
		{
			new UpdateEmployeeWorkspaceTypeDto { EmployeeId = employeeEntity.Id, WorkspaceType = Dtos.Enums.WorkspaceType.PartiallyRemote }
		});
		var commandHandler = new UpdateEmployeesWorkspaceTypesHandler(_employeeRepository, _employeeWorkingTypeHistoryRepository, _desksRepository, _mapper);

		// Act
		await commandHandler.HandleAsync(command);

		// Assert
		Assert.IsNotEmpty(_applicationDbContext.Employees.First().EmployeeDeskReservations);
	}

	[Test]
	public async Task ShouldNotReleaseOtherEmployeesDesksWhenChangingWorkspaceTypeForSelectedUser()
	{
		SetupTestData();

		var command = new UpdateEmployeesWorkspaceTypesCommand(new List<UpdateEmployeeWorkspaceTypeDto>
		{
			new UpdateEmployeeWorkspaceTypeDto { EmployeeId = _applicationDbContext.Employees.First().Id, WorkspaceType = Dtos.Enums.WorkspaceType.Remote }
		});
		var commandHandler = new UpdateEmployeesWorkspaceTypesHandler(_employeeRepository, _employeeWorkingTypeHistoryRepository, _desksRepository, _mapper);

		// Act
		await commandHandler.HandleAsync(command);

		// Assert
		Assert.IsNotEmpty(_applicationDbContext.Employees.Last().EmployeeDeskReservations);
	}

	private void SetupTestData()
	{
		_applicationDbContext.ClearDatabase();

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

		_applicationDbContext.Employees.AddRange(employee1, employee2);

		var floor = new FloorEntity { Building = new BuildingEntity { Name = "F1" }, FloorNumber = 0 };

		var room1 = new RoomEntity { Area = 22.5m, Name = "001", Floor = floor };

		_applicationDbContext.Desks.AddRange(
			DeskHelpers.CreateDeskWithReservation(room1, 1, employee1),
			DeskHelpers.CreateDeskWithReservation(room1, 2, employee1),
			DeskHelpers.CreateDeskWithReservation(room1, 2, employee2),
			new DeskEntity { Room = room1, Number = 3 }
		);

		_applicationDbContext.SaveChanges();
	}
}
