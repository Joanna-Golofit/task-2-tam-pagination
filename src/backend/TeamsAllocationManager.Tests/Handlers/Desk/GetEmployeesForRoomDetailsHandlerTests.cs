using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Room.Queries;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Domain.Enums;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Room;
using TeamsAllocationManager.Infrastructure.Handlers.Room;

namespace TeamsAllocationManager.Tests.Handlers.Desk;

[TestFixture]
public class GetEmployeesForRoomDetailsHandlerTests
{
	private readonly ApplicationDbContext _context;

	public GetEmployeesForRoomDetailsHandlerTests()
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

		var teamLeader1 = new EmployeeEntity { Name = "Jan", Surname = "Kowalski", Email = "jkowalski@fp.pl", ExternalId = 1, UserLogin = "jkowalski", WorkspaceType = WorkspaceType.Office };
		var employee1 = new EmployeeEntity { Name = "Katarzyna", Surname = "Nowak", Email = "knowak@fp.pl", ExternalId = 2, UserLogin = "knowak", WorkspaceType = WorkspaceType.Office };
		var employee2 = new EmployeeEntity { Name = "Katarzyna", Surname = "Nowak", Email = "knowak@fp.pl", ExternalId = 3, UserLogin = "knowak", WorkspaceType = null };

		var project1 = new ProjectEntity
		{
			Name = "Project 1", EndDate = DateTime.Now.AddDays(100), ExternalId = 1,
			Employees = new List<EmployeeProjectEntity> {
				new EmployeeProjectEntity { Employee = teamLeader1, IsTeamLeaderProjectRole = true },
				new EmployeeProjectEntity { Employee = employee1 },
				new EmployeeProjectEntity { Employee = employee2 }
			}
		};
		_context.Projects.Add(project1);
		_context.SaveChanges();
	}

	[Test]
	public async Task Get_ProjectEmployees_ReturnsProjectEmployees()
	{
		// given
		Guid projectId = _context.Projects
			.Select(p => p.Id)
			.First();

		EmployeeEntity? employee = _context.Employees.First();
		var query = new GetEmployeesForRoomDetailsQuery(projectId, Guid.NewGuid());
		var queryHandler = new GetEmployeesForRoomDetailsHandler(_context);

		// when
		IEnumerable<EmployeeForRoomDetailsDto>? result = await queryHandler.HandleAsync(query);

		// then
		Assert.AreEqual(2, result.Count());
	}
	[Test]
	public async Task Get_FalseProjectId_ReturnsNoProjectEmployees()
	{
		// given
		var query = new GetEmployeesForRoomDetailsQuery(Guid.NewGuid(), Guid.NewGuid());
		var queryHandler = new GetEmployeesForRoomDetailsHandler(_context);

		// when
		IEnumerable<EmployeeForRoomDetailsDto>? result = await queryHandler.HandleAsync(query);

		// then
		Assert.AreEqual(0, result.Count());
	}
}
