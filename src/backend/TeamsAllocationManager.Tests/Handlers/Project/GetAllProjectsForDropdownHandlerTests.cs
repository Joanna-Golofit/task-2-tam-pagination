using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TeamsAllocationManager.Contracts.Project.Queries;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Domain.Enums;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Project;
using TeamsAllocationManager.Infrastructure.Handlers.Project;

namespace TeamsAllocationManager.Tests.Handlers.Project;

[TestFixture]
public class GetAllProjectsForDropdownHandlerTests
{
	private readonly ApplicationDbContext _context;

	public GetAllProjectsForDropdownHandlerTests()
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

		var teamLeader2 = new EmployeeEntity { Name = "Mariusz", Surname = "Szczepañski", Email = "mszczepanski@fp.pl", UserLogin = "mszczepanski" };

		var project3 = new ProjectEntity
		{
			ExternalId = 3, Name = "Project 3", EndDate = DateTime.Now.AddDays(100), Employees = new List<EmployeeProjectEntity> {
				new EmployeeProjectEntity { Employee = teamLeader2, IsTeamLeaderProjectRole = true },
			}
		};
		var anotherProject = new ProjectEntity
		{
			ExternalId = 4, Name = "Another project", EndDate = DateTime.Now.AddDays(100)
		};
		_context.Projects.AddRange(project3, anotherProject);

		_context.SaveChanges();
	}

	[Test]
	public void ShouldReturnProperlyProjects()
	{
		// given
		int expectedProjectCount = _context.Projects.Count();
		ProjectEntity existingProject = _context.Projects.First();

		var query = new GetAllProjectsForDropdownQuery(null);
		var handler = new GetAllProjectsForDropdownHandler(_context);

		// when
		IEnumerable<ProjectForDropdownDto> projectsResult = handler.HandleAsync(query).Result;

		// then
		Assert.AreEqual(expectedProjectCount, projectsResult.Count());
		Assert.IsTrue(projectsResult.Any(p => p.Name == existingProject.Name));
		Assert.IsTrue(projectsResult.Any(p => p.Id == existingProject.Id));
		Assert.IsTrue(projectsResult.Count(p => p.Id == existingProject.Id) == 1);
	}

	[Test]
	public void ExecuteAsync_WithQuery_ReturnsMatchingProjects()
	{
		// given
		ProjectEntity matchingProject = _context.Projects.First(p => p.ExternalId == 4);
		string searchString = "Nothe";

		var query = new GetAllProjectsForDropdownQuery(searchString);
		var handler = new GetAllProjectsForDropdownHandler(_context);

		// when
		IEnumerable<ProjectForDropdownDto> projectsResult = handler.HandleAsync(query).Result;

		// then
		Assert.AreEqual(1, projectsResult.Count());
		Assert.IsTrue(projectsResult.Any(p => p.Name == matchingProject.Name));
		Assert.IsTrue(projectsResult.Any(p => p.Id == matchingProject.Id));
		Assert.IsTrue(projectsResult.Count(p => p.Id == matchingProject.Id) == 1);
	}
}
