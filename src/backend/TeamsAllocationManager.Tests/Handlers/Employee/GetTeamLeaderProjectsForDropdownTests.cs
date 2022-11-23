using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Employee.Queries;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Employee;
using TeamsAllocationManager.Infrastructure.Handlers.Employee;

namespace TeamsAllocationManager.Tests.Handlers.Employee;

[TestFixture]
public class GetTeamLeaderProjectsForDropdownTests
{
	private readonly ApplicationDbContext _context;
	private readonly ProjectsRepository _projectRepository;

	public GetTeamLeaderProjectsForDropdownTests()
	{
		DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: GetType().Name)
			.Options;
		_context = new ApplicationDbContext(options);
		_projectRepository = new ProjectsRepository(_context);
	}

	[SetUp]
	public void SetupBeforeEachTest()
	{
		_context.ClearDatabase();
	}

	[Test]
	public async Task ExecuteAsync_TeamLeaderProjects_GetTeamLeaderProjects()
	{
		// given
		var teamLeader1 = new EmployeeEntity
		{
			Name = "Jan",
			Surname = "Kowalski",
			Email = "jkowalski@fp.pl",
			ExternalId = 1,
			UserLogin = "jkowalski"
		};

		var project1 = new ProjectEntity
		{
			Name = "Project 1", EndDate = DateTime.Now.AddDays(100), ExternalId = 1,
			Employees = new List<EmployeeProjectEntity> {
				new EmployeeProjectEntity { Employee = teamLeader1, IsTeamLeaderProjectRole = true },
			},
		};

		var project2 = new ProjectEntity
		{
			Name = "Project 2", EndDate = DateTime.Now.AddDays(100), ExternalId = 2,
			Employees = new List<EmployeeProjectEntity> {
				new EmployeeProjectEntity { Employee = teamLeader1, IsTeamLeaderProjectRole = true },
			},
		};

		var project3 = new ProjectEntity
		{
			Name = "Project 3", EndDate = DateTime.Now.AddDays(100), ExternalId = 3,
			Employees = new List<EmployeeProjectEntity> {
				new EmployeeProjectEntity { Employee = teamLeader1, IsTeamLeaderProjectRole = true },
			},
		};

		var project4 = new ProjectEntity
		{
			Name = "Project 4", EndDate = DateTime.Now.AddDays(100), ExternalId = 4,
			Employees = new List<EmployeeProjectEntity> {
				new EmployeeProjectEntity { Employee = teamLeader1, IsTeamLeaderProjectRole = false },
			},
		};


		_context.Projects.AddRange(project1, project2, project3, project4);
		await _context.SaveChangesAsync();

		var query = new GetTeamLeaderProjectsForDropdownQuery(teamLeader1.Id);
		var queryHandler = new GetTeamLeaderProjectsForDropdownHandler(_projectRepository);

		// when
		IEnumerable<TeamLeaderProjectForDropdownDto>? result = await queryHandler.HandleAsync(query);

		// then
		Assert.IsNotNull(result);
		Assert.AreEqual(3, result.Count());
		Assert.IsNotNull(result.SingleOrDefault(x => x.Id == project1.Id));
		Assert.IsNotNull(result.SingleOrDefault(x => x.Name == project1.Name));
		Assert.IsNotNull(result.SingleOrDefault(x => x.Id == project2.Id));
	}
}
