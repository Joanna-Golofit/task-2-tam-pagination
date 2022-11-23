using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Project.Queries;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Domain.Enums;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Common;
using TeamsAllocationManager.Dtos.Project;
using TeamsAllocationManager.Infrastructure.Handlers.Project;
using TeamsAllocationManager.Mapper.Profiles;
using TeamsAllocationManager.Tests.Helpers;

namespace TeamsAllocationManager.Tests.Handlers.Project;

[TestFixture]
public class GetProjectsHandlerTests
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetProjectsHandlerTests()
	{
		DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: GetType().Name)
			.Options;
		_context = new ApplicationDbContext(options);
		_mapper = new MapperConfiguration(mc => mc.AddProfile(new EntityDtoProfile())).CreateMapper();
	}

	[SetUp]
	public void SetupBeforeEachTest()
	{
		_context.ClearDatabase();
	}

	[Test]
	public async Task ShouldReturnProperlySetProjects()
	{
		// given
		var teamLeader1 = new EmployeeEntity { Name = "Jan", Surname = "Kowalski", Email = "jkowalski@fp.pl", ExternalId = 1, WorkspaceType = WorkspaceType.Office, UserLogin = "jkowalski" };
		var teamLeader2 = new EmployeeEntity { Name = "Mariusz", Surname = "Szczepañski", Email = "mszczepanski@fp.pl", ExternalId = 2, WorkspaceType = WorkspaceType.Office, UserLogin = "mszczepanski" };

		var employee1 = new EmployeeEntity { Name = "Katarzyna", Surname = "Nowak", Email = "knowak@fp.pl", ExternalId = 3, WorkspaceType = WorkspaceType.Remote, UserLogin = "knowak" };
		var employee2 = new EmployeeEntity { Name = "Marian", Surname = "Drzewiecki", Email = "mdrzewiecki@fp.pl", ExternalId = 4, WorkspaceType = WorkspaceType.Remote, UserLogin = "mdrzewiecki" };
		var employee3 = new EmployeeEntity { Name = "£ukasz", Surname = "Francuz", Email = "lfrancuz@fp.pl", ExternalId = 5, WorkspaceType = WorkspaceType.Hybrid, UserLogin = "lfrancuz" };
		var employee4 = new EmployeeEntity { Name = "Piotr", Surname = "Ryczek", Email = "pryczek@fp.pl", ExternalId = 6, WorkspaceType = WorkspaceType.Hybrid, UserLogin = "pryczek" };
		var employee5 = new EmployeeEntity { Name = "Maria", Surname = "Dobra", Email = "mdobra@fp.pl", ExternalId = 7, WorkspaceType = WorkspaceType.Office, UserLogin = "mdobra" };
		var employee6 = new EmployeeEntity { Name = "Klaudia", Surname = "Lepsza", Email = "klepsza@fp.pl", ExternalId = 8, WorkspaceType = WorkspaceType.Office, UserLogin = "klepsza" };
		var employee7 = new EmployeeEntity { Name = "Wojciech", Surname = "Wojciechowski", Email = "wwojciechowski@fp.pl", ExternalId = 9, WorkspaceType = WorkspaceType.Remote, UserLogin = "wwojciechowski" };
		var employee8 = new EmployeeEntity { Name = "Krystyna", Surname = "Dzwon", Email = "kdzwon@fp.pl", ExternalId = 10, WorkspaceType = WorkspaceType.Office, UserLogin = "kdzwon" };
		var employee9 = new EmployeeEntity { Name = "Krystyna2", Surname = "Dzwon2", Email = "kdzwon2@fp.pl", ExternalId = 11, WorkspaceType = null, UserLogin = "kdzwon2" };

		var project1 = new ProjectEntity
		{
			Name = "Project 1", EndDate = DateTime.Now.AddDays(100), ExternalId = 1,
			Employees = new List<EmployeeProjectEntity>
			{
				new EmployeeProjectEntity { Employee = teamLeader1, IsTeamLeaderProjectRole = true },
				new EmployeeProjectEntity { Employee = employee1 },
				new EmployeeProjectEntity { Employee = employee2 },
				new EmployeeProjectEntity { Employee = employee3 },
				new EmployeeProjectEntity { Employee = employee4 },
				new EmployeeProjectEntity { Employee = employee5 },
				new EmployeeProjectEntity { Employee = employee6 },
				new EmployeeProjectEntity { Employee = employee9 }
			}
		};
		var project2 = new ProjectEntity
		{
			Name = "Project 2", EndDate = DateTime.Now.AddDays(100), ExternalId = 2,
			Employees = new List<EmployeeProjectEntity>
			{
				new EmployeeProjectEntity { Employee = teamLeader2, IsTeamLeaderProjectRole = true },
				new EmployeeProjectEntity { Employee = employee4 },
				new EmployeeProjectEntity { Employee = employee5 },
				new EmployeeProjectEntity { Employee = employee6 },
				new EmployeeProjectEntity { Employee = employee7 },
				new EmployeeProjectEntity { Employee = employee8 }
			}
		};
		var project3 = new ProjectEntity
		{
			Name = "Project 3", EndDate = DateTime.Now.AddDays(100), ExternalId = 3,
			Employees = new List<EmployeeProjectEntity>
			{
				new EmployeeProjectEntity { Employee = teamLeader2, IsTeamLeaderProjectRole = true }
			}
		};
		_context.Projects.AddRange(project1, project2, project3);

		var building1 = new BuildingEntity { Name = "F1" };
		var floor1 = new FloorEntity { Building = building1, FloorNumber = 0 };
		var room1 = new RoomEntity { Area = 22.5m, Name = "001", Floor = floor1 };

		_context.Desks.AddRange(
			DeskHelpers.CreateDeskWithReservation(room1, 1, teamLeader1),
			DeskHelpers.CreateDeskWithReservation(room1, 2, employee1),
			DeskHelpers.CreateDeskWithReservation(room1, 3, employee2),
			new DeskEntity { Room = room1, Number = 4 },
			new DeskEntity { Room = room1, Number = 5 });

		var floor2 = new FloorEntity { Building = building1, FloorNumber = 1 };
		var room2 = new RoomEntity { Area = 12.8m, Name = "002", Floor = floor2 };

		_context.Desks.AddRange(
			DeskHelpers.CreateDeskWithReservation(room2, 1, employee3),
			new DeskEntity { Room = room2, Number = 2 },
			DeskHelpers.CreateDeskWithReservation(room2, 3, employee5),
			new DeskEntity { Room = room2, Number = 4 },
			new DeskEntity { Room = room2, Number = 5 });

		var building2 = new BuildingEntity { Name = "F2" };
		var floor3 = new FloorEntity { Building = building2, FloorNumber = 2 };
		var room3 = new RoomEntity { Area = 37.2m, Name = "001", Floor = floor3 };

		_context.Desks.AddRange(
			new DeskEntity { Room = room3, Number = 1 },
			DeskHelpers.CreateDeskWithReservation(room3, 2, teamLeader2),
			new DeskEntity { Room = room3, Number = 3 },
			DeskHelpers.CreateDeskWithReservation(room3, 4, employee7),
			new DeskEntity { Room = room3, Number = 5 });

		var floor4 = new FloorEntity { Building = building2, FloorNumber = 1 };
		var room4 = new RoomEntity { Area = 44.3m, Name = "002", Floor = floor4 };
		_context.Desks.AddRange(new DeskEntity { Room = room4, Number = 1 },
			new DeskEntity { Room = room4, Number = 2 },
			new DeskEntity { Room = room4, Number = 3 },
			new DeskEntity { Room = room4, Number = 4 },
			new DeskEntity { Room = room4, Number = 5 });

		var floor5 = new FloorEntity { Building = building2, FloorNumber = 0 };
		var room5 = new RoomEntity { Area = 34.9m, Name = "003", Floor = floor5 };
		_context.Desks.AddRange(new DeskEntity { Room = room5, Number = 1 },
			new DeskEntity { Room = room5, Number = 2 },
			new DeskEntity { Room = room5, Number = 3 },
			new DeskEntity { Room = room5, Number = 4 },
			new DeskEntity { Room = room5, Number = 5 });
		_context.SaveChanges();

		var query = new GetProjectsQuery(new ProjectsFiltersDto> { PageSize = 100, PageNumber = 0 });
		var handler = new GetProjectsHandler(_context, _mapper);

		// when
		PagedListResultDto<IEnumerable<ProjectDto>> pagedResult = await handler.HandleAsync(query);
		IEnumerable<ProjectDto> result = pagedResult.Payload!;

		// then
		Assert.AreEqual(3, result.Count());
		ProjectDto projectResult = result.Single(p => p.Name == project1.Name);
		Assert.AreEqual(5, projectResult.AssignedPeopleCount);
		Assert.AreEqual(2, projectResult.UnassignedMembersCount);
		Assert.AreEqual(1, projectResult.NotSetMembersCount);
		Assert.Contains(teamLeader1.Email, projectResult.TeamLeaders.Select(tl => tl.Email).ToList());

		projectResult = result.Single(p => p.Name == project2.Name);
		Assert.AreEqual(3, projectResult.AssignedPeopleCount);
		Assert.AreEqual(6, projectResult.PeopleCount);
		Assert.AreEqual(3, projectResult.UnassignedMembersCount);
		Assert.AreEqual(0, projectResult.NotSetMembersCount);
		Assert.Contains(teamLeader2.Email, projectResult.TeamLeaders.Select(tl => tl.Email).ToList());

		projectResult = result.Single(p => p.Name == project3.Name);
		Assert.AreEqual(1, projectResult.AssignedPeopleCount);
		Assert.AreEqual(1, projectResult.PeopleCount);
		Assert.AreEqual(0, projectResult.UnassignedMembersCount);
		Assert.AreEqual(0, projectResult.NotSetMembersCount);
		Assert.Contains(teamLeader2.Email, projectResult.TeamLeaders.Select(tl => tl.Email).ToList());
	}

	[Test]
	public async Task ShouldReturnEmptyResponseWhenDbIsEmpty()
	{
		// given
		var query = new GetProjectsQuery(new ProjectsFiltersDto> { PageSize = 100, PageNumber = 0 });
		var handler = new GetProjectsHandler(_context, _mapper);

		// when
		PagedListResultDto<IEnumerable<ProjectDto>> pagedResult = await handler.HandleAsync(query);
		IEnumerable<ProjectDto> result = pagedResult.Payload!;

		// then 
		Assert.Zero(result.Count());
	}

	[Test]
	public async Task ExecuteAsync_MultiplePages_ReturnsPagedProjects()
	{
		// given
		int projectsCount = 123;
		int pageSize = 10;

		var employee1 = new EmployeeEntity { Name = "Katarzyna", Surname = "Nowak", Email = "knowak@fp.pl", ExternalId = 3, WorkspaceType = WorkspaceType.Remote, UserLogin = "knowak" };
		var employee2 = new EmployeeEntity { Name = "Marian", Surname = "Drzewiecki", Email = "mdrzewiecki@fp.pl", ExternalId = 4, WorkspaceType = WorkspaceType.Remote, UserLogin = "mdrzewiecki" };
		var employee3 = new EmployeeEntity { Name = "£ukasz", Surname = "Francuz", Email = "lfrancuz@fp.pl", ExternalId = 5, WorkspaceType = WorkspaceType.Hybrid, UserLogin = "lfrancuz" };
		var employee4 = new EmployeeEntity { Name = "Piotr", Surname = "Ryczek", Email = "pryczek@fp.pl", ExternalId = 6, WorkspaceType = WorkspaceType.Hybrid, UserLogin = "pryczek" };
		var employee5 = new EmployeeEntity { Name = "Maria", Surname = "Dobra", Email = "mdobra@fp.pl", ExternalId = 7, WorkspaceType = WorkspaceType.Office, UserLogin = "mdobra" };
		var employee6 = new EmployeeEntity { Name = "Klaudia", Surname = "Lepsza", Email = "klepsza@fp.pl", ExternalId = 8, WorkspaceType = WorkspaceType.Office, UserLogin = "klepsza" };

		for (int i = 1; i <= projectsCount; ++i)
		{
			_context.Projects.Add(new ProjectEntity
			{
				ExternalId = i,
				Name = $"Project{i}",
				Employees = new List<EmployeeProjectEntity> {
					new EmployeeProjectEntity { Employee = employee1 },
					new EmployeeProjectEntity { Employee = employee2 },
					new EmployeeProjectEntity { Employee = employee3 },
					new EmployeeProjectEntity { Employee = employee4 },
					new EmployeeProjectEntity { Employee = employee5 },
					new EmployeeProjectEntity { Employee = employee6 }
				}
			});
		}

		_context.SaveChanges();

		var query1 = new GetProjectsQuery(new ProjectsFiltersDto>
		{
			PageNumber = 0,
			PageSize = pageSize
		});
		var query2 = new GetProjectsQuery(new ProjectsFiltersDto>
		{
			PageNumber = 12,
			PageSize = pageSize
		});
		var handler = new GetProjectsHandler(_context, _mapper);

		// when
		PagedListResultDto<IEnumerable<ProjectDto>> firstPageResult = await handler.HandleAsync(query1);
		PagedListResultDto<IEnumerable<ProjectDto>> lastPageResult = await handler.HandleAsync(query2);

		// then
		firstPageResult.Count.ShouldBe(projectsCount);
		firstPageResult.Payload!.Count().ShouldBe(pageSize);
		lastPageResult.Count.ShouldBe(projectsCount);
		lastPageResult.Payload!.Count().ShouldBe(3);
	}

	[Test]
	public async Task Get_WorkspaceTypeCountsInProject_ReturnsCounts()
	{
		var employee1 = new EmployeeEntity { Name = "Jan", Surname = "Kowalski", UserLogin = "jkowalski", Email = "jkowalski@fp.pl", ExternalId = 1, WorkspaceType = WorkspaceType.Office };
		var employee2 = new EmployeeEntity { Name = "Mariusz", Surname = "Szczepañski", UserLogin = "mszczepanski", Email = "mszczepanski@fp.pl", ExternalId = 2, WorkspaceType = WorkspaceType.Office};
		var employee3 = new EmployeeEntity { Name = "Katarzyna", Surname = "Nowak", UserLogin = "knowak", Email = "knowak@fp.pl", ExternalId = 3, WorkspaceType = WorkspaceType.Remote};
		var employee4 = new EmployeeEntity { Name = "Marian", Surname = "Drzewiecki", UserLogin = "mdrzewiecki", Email = "mdrzewiecki@fp.pl", ExternalId = 4, WorkspaceType = WorkspaceType.Remote};
		var employee5 = new EmployeeEntity { Name = "£ukasz", Surname = "Francuz", UserLogin = "lfrancuz", Email = "lfrancuz@fp.pl", ExternalId = 5, WorkspaceType = WorkspaceType.Office };
		var employee6 = new EmployeeEntity { Name = "Piotr", Surname = "Ryczek", UserLogin = "pryczek", Email = "pryczek@fp.pl", ExternalId = 6, WorkspaceType = WorkspaceType.Hybrid };
		var employee7 = new EmployeeEntity { Name = "Maria", Surname = "Dobra", UserLogin = "mdobra", Email = "mdobra@fp.pl", ExternalId = 7, WorkspaceType = WorkspaceType.Hybrid};
		var employee8 = new EmployeeEntity { Name = "Klaudia", Surname = "Lepsza", UserLogin = "klepsza", Email = "klepsza@fp.pl", ExternalId = 8, WorkspaceType = WorkspaceType.Office};
		var employee9 = new EmployeeEntity { Name = "Wojciech", Surname = "Wojciechowski", UserLogin = "wwojciechowski", Email = "wwojciechowski@fp.pl", ExternalId = 9, WorkspaceType = WorkspaceType.Hybrid};
		var employee10 = new EmployeeEntity { Name = "Krystyna", Surname = "Dzwon", UserLogin = "kdzwon", Email = "kdzwon@fp.pl", ExternalId = 10, WorkspaceType = WorkspaceType.Remote};
		var employee11 = new EmployeeEntity { Name = "Krystyna2", Surname = "Dzwon2", UserLogin = "kdzwon2", Email = "kdzwon2@fp.pl", ExternalId = 11, WorkspaceType = null};

		//Projects
		var project1 = new ProjectEntity { Name = "Project1", ExternalId = 0 };
		var project2 = new ProjectEntity { Name = "Project2", ExternalId = 1 };
		var project3 = new ProjectEntity { Name = "Project3", ExternalId = 2 };
		_context.Projects.AddRange(project1, project2, project3);

		//EmployeeProjects
		var employeeProject1 = new EmployeeProjectEntity { Employee = employee1, Project = project1, IsTeamLeaderProjectRole = true };
		var employeeProject2 = new EmployeeProjectEntity { Employee = employee2, Project = project2, IsTeamLeaderProjectRole = true };
		var employeeProject3 = new EmployeeProjectEntity { Employee = employee3, Project = project3, IsTeamLeaderProjectRole = true };
		var employeeProject4 = new EmployeeProjectEntity { Employee = employee4, Project = project1 };
		var employeeProject5 = new EmployeeProjectEntity { Employee = employee5, Project = project2 };
		var employeeProject6 = new EmployeeProjectEntity { Employee = employee6, Project = project3 };
		var employeeProject7 = new EmployeeProjectEntity { Employee = employee7, Project = project1 };
		var employeeProject8 = new EmployeeProjectEntity { Employee = employee8, Project = project2 };
		var employeeProject9 = new EmployeeProjectEntity { Employee = employee9, Project = project3 };
		var employeeProject10 = new EmployeeProjectEntity { Employee = employee10, Project = project1 };
		var employeeProject11 = new EmployeeProjectEntity { Employee = employee11, Project = project1 };
		_context.EmployeeProjects.AddRange(employeeProject1, employeeProject2, employeeProject3, employeeProject4,
			employeeProject5, employeeProject6, employeeProject7, employeeProject8, employeeProject9, employeeProject10, employeeProject11);

		_context.SaveChanges();

		var query = new GetProjectsQuery(new ProjectsFiltersDto> { PageSize = 100, PageNumber = 0 });
		var handler = new GetProjectsHandler(_context, _mapper);

		// when
		PagedListResultDto<IEnumerable<ProjectDto>> result = await handler.HandleAsync(query);
		ProjectDto[]? projects = result.Payload!.ToArray();

		//Project 1
		Assert.AreEqual(1, projects[0].OfficeEmployeesCount);
		Assert.AreEqual(2, projects[0].RemoteEmployeesCount);
		Assert.AreEqual(1, projects[0].HybridEmployeesCount);
		Assert.AreEqual(1, projects[0].NotSetMembersCount);

		//Project 2
		Assert.AreEqual(3, projects[1].OfficeEmployeesCount);
		Assert.AreEqual(0, projects[1].RemoteEmployeesCount);
		Assert.AreEqual(0, projects[1].HybridEmployeesCount);
		Assert.AreEqual(0, projects[1].NotSetMembersCount);

		//Project 3
		Assert.AreEqual(0, projects[2].OfficeEmployeesCount);
		Assert.AreEqual(1, projects[2].RemoteEmployeesCount);
		Assert.AreEqual(2, projects[2].HybridEmployeesCount);
		Assert.AreEqual(0, projects[2].NotSetMembersCount);
	}

	[Test]
	public async Task ExecuteAsync_PeopleMaxCountFilters_ReturnsFilteredProjects()
	{
		// given
		var employee1 = new EmployeeEntity { Name = "Jan", Surname = "Kowalski", Email = "jkowalski@fp.pl", ExternalId = 1, UserLogin = "jkowalski" };
		var employee2 = new EmployeeEntity { Name = "Mariusz", Surname = "Szczepañski", Email = "mszczepanski@fp.pl", ExternalId = 2, UserLogin = "mszczepanski" };
		var employee3 = new EmployeeEntity { Name = "Katarzyna", Surname = "Nowak", Email = "knowak@fp.pl", ExternalId = 3, UserLogin = "knowak" };
		var employee4 = new EmployeeEntity { Name = "Marian", Surname = "Drzewiecki", Email = "mdrzewiecki@fp.pl", ExternalId = 4, UserLogin = "mdrzewiecki" };
		var employee5 = new EmployeeEntity { Name = "£ukasz", Surname = "Francuz", Email = "lfrancuz@fp.pl", ExternalId = 5, UserLogin = "lfrancuz" };
		var employee6 = new EmployeeEntity { Name = "Piotr", Surname = "Ryczek", Email = "pryczek@fp.pl", ExternalId = 6, UserLogin = "pryczek" };
		var employee7 = new EmployeeEntity { Name = "Maria", Surname = "Dobra", Email = "mdobra@fp.pl", ExternalId = 7, UserLogin = "mdobra" };
		var employee8 = new EmployeeEntity { Name = "Klaudia", Surname = "Lepsza", Email = "klepsza@fp.pl", ExternalId = 8, UserLogin = "klepsza" };

		var project1 = new ProjectEntity
		{
			ExternalId = 1, Name = "Project1", Employees = {
				new EmployeeProjectEntity { Employee = employee1 },
				new EmployeeProjectEntity { Employee = employee2 },
				new EmployeeProjectEntity { Employee = employee3 },
				new EmployeeProjectEntity { Employee = employee4 },
				new EmployeeProjectEntity { Employee = employee5 },
				new EmployeeProjectEntity { Employee = employee6 },
				new EmployeeProjectEntity { Employee = employee7 },
				new EmployeeProjectEntity { Employee = employee8 }
			}
		};
		var project2 = new ProjectEntity
		{
			ExternalId = 2, Name = "Project2", Employees = {
				new EmployeeProjectEntity { Employee = employee1 },
				new EmployeeProjectEntity { Employee = employee2 },
				new EmployeeProjectEntity { Employee = employee3 },
				new EmployeeProjectEntity { Employee = employee4 },
				new EmployeeProjectEntity { Employee = employee5 },
				new EmployeeProjectEntity { Employee = employee6 }
			}
		};
		var project3 = new ProjectEntity
		{
			ExternalId = 3, Name = "Project3", Employees = {
				new EmployeeProjectEntity { Employee = employee1 },
				new EmployeeProjectEntity { Employee = employee2 },
				new EmployeeProjectEntity { Employee = employee3 }
			}
		};
		_context.Projects.AddRange(project1, project2, project3);
		_context.SaveChanges();

		int peopleMaxCount = 5;

		var query = new GetProjectsQuery(new ProjectsFiltersDto>
		{
			Filters =
			{
				PeopleMaxCount = peopleMaxCount
			},
			PageNumber = 0,
			PageSize = 100
		});
		var handler = new GetProjectsHandler(_context, _mapper);

		// when
		PagedListResultDto<IEnumerable<ProjectDto>> result = await handler.HandleAsync(query);

		// then
		result.Count.ShouldBe(1);
		result.Payload!.Single().Name.ShouldBe("Project3");
	}

	[Test]
	public async Task ExecuteAsync_PeopleMinCountFilters_ReturnsFilteredProjects()
	{
		// given
		var employee1 = new EmployeeEntity { Name = "Jan", Surname = "Kowalski", Email = "jkowalski@fp.pl", ExternalId = 1, UserLogin = "jkowalski" };
		var employee2 = new EmployeeEntity { Name = "Mariusz", Surname = "Szczepañski", Email = "mszczepanski@fp.pl", ExternalId = 2, UserLogin = "mszczepanski" };
		var employee3 = new EmployeeEntity { Name = "Katarzyna", Surname = "Nowak", Email = "knowak@fp.pl", ExternalId = 3, UserLogin = "knowak" };
		var employee4 = new EmployeeEntity { Name = "Marian", Surname = "Drzewiecki", Email = "mdrzewiecki@fp.pl", ExternalId = 4, UserLogin = "mdrzewiecki" };
		var employee5 = new EmployeeEntity { Name = "£ukasz", Surname = "Francuz", Email = "lfrancuz@fp.pl", ExternalId = 5, UserLogin = "lfrancuz" };
		var employee6 = new EmployeeEntity { Name = "Piotr", Surname = "Ryczek", Email = "pryczek@fp.pl", ExternalId = 6, UserLogin = "pryczek" };
		var employee7 = new EmployeeEntity { Name = "Maria", Surname = "Dobra", Email = "mdobra@fp.pl", ExternalId = 7, UserLogin = "mdobra" };
		var employee8 = new EmployeeEntity { Name = "Klaudia", Surname = "Lepsza", Email = "klepsza@fp.pl", ExternalId = 8, UserLogin = "klepsza" };

		var project1 = new ProjectEntity
		{
			ExternalId = 1, Name = "Project1", Employees = {
				new EmployeeProjectEntity { Employee = employee1 },
				new EmployeeProjectEntity { Employee = employee2 },
				new EmployeeProjectEntity { Employee = employee3 },
				new EmployeeProjectEntity { Employee = employee4 },
				new EmployeeProjectEntity { Employee = employee5 },
				new EmployeeProjectEntity { Employee = employee6 },
				new EmployeeProjectEntity { Employee = employee7 },
				new EmployeeProjectEntity { Employee = employee8 }
			}
		};
		var project2 = new ProjectEntity
		{
			ExternalId = 2, Name = "Project2", Employees = {
				new EmployeeProjectEntity { Employee = employee1 },
				new EmployeeProjectEntity { Employee = employee2 },
				new EmployeeProjectEntity { Employee = employee3 },
				new EmployeeProjectEntity { Employee = employee4 },
				new EmployeeProjectEntity { Employee = employee5 },
				new EmployeeProjectEntity { Employee = employee6 }
			}
		};
		var project3 = new ProjectEntity
		{
			ExternalId = 3, Name = "Project3", Employees = {
				new EmployeeProjectEntity { Employee = employee1 },
				new EmployeeProjectEntity { Employee = employee2 },
				new EmployeeProjectEntity { Employee = employee3 }
			}
		};
		_context.Projects.AddRange(project1, project2, project3);
		_context.SaveChanges();

		int peopleMinCount = 5;

		var query = new GetProjectsQuery(new ProjectsFiltersDto>
		{
			Filters =
			{
				PeopleMinCount = peopleMinCount
			},
			PageNumber = 0,
			PageSize = 100
		});
		var handler = new GetProjectsHandler(_context, _mapper);

		// when
		PagedListResultDto<IEnumerable<ProjectDto>> result = await handler.HandleAsync(query);

		// then
		result.Count.ShouldBe(2);
		result.Payload!.ShouldContain(p => p.Name == "Project1");
		result.Payload!.ShouldContain(p => p.Name == "Project2");
	}

	[Test]
	public async Task ExecuteAsync_UnassignedPeopleMaxCountFilters_ReturnsFilteredProjects()
	{
		// given
		var employee1 = new EmployeeEntity { Name = "Jan", Surname = "Kowalski", Email = "jkowalski@fp.pl", ExternalId = 1, UserLogin = "jkowalski" };
		var employee2 = new EmployeeEntity { Name = "Mariusz", Surname = "Szczepañski", Email = "mszczepanski@fp.pl", ExternalId = 2, UserLogin = "mszczepanski" };
		var employee3 = new EmployeeEntity { Name = "Katarzyna", Surname = "Nowak", Email = "knowak@fp.pl", ExternalId = 3, UserLogin = "knowak" };
		var employee4 = new EmployeeEntity { Name = "Marian", Surname = "Drzewiecki", Email = "mdrzewiecki@fp.pl", ExternalId = 4, UserLogin = "mdrzewiecki" };
		var employee5 = new EmployeeEntity { Name = "£ukasz", Surname = "Francuz", Email = "lfrancuz@fp.pl", ExternalId = 5, UserLogin = "lfrancuz" };
		var employee6 = new EmployeeEntity { Name = "Piotr", Surname = "Ryczek", Email = "pryczek@fp.pl", ExternalId = 6, UserLogin = "pryczek" };
		var employee7 = new EmployeeEntity { Name = "Maria", Surname = "Dobra", Email = "mdobra@fp.pl", ExternalId = 7, UserLogin = "mdobra" };
		var employee8 = new EmployeeEntity { Name = "Klaudia", Surname = "Lepsza", Email = "klepsza@fp.pl", ExternalId = 8, UserLogin = "klepsza" };

		var project1 = new ProjectEntity
		{
			ExternalId = 1, Name = "Project1", Employees = {
				new EmployeeProjectEntity { Employee = employee1 },
				new EmployeeProjectEntity { Employee = employee2 },
				new EmployeeProjectEntity { Employee = employee3 },
				new EmployeeProjectEntity { Employee = employee4 },
				new EmployeeProjectEntity { Employee = employee5 },
				new EmployeeProjectEntity { Employee = employee6 },
				new EmployeeProjectEntity { Employee = employee7 },
				new EmployeeProjectEntity { Employee = employee8 }
			}
		};
		var project2 = new ProjectEntity
		{
			ExternalId = 2, Name = "Project2", Employees = {
				new EmployeeProjectEntity { Employee = employee3 },
				new EmployeeProjectEntity { Employee = employee4 },
				new EmployeeProjectEntity { Employee = employee5 },
				new EmployeeProjectEntity { Employee = employee6 },
				new EmployeeProjectEntity { Employee = employee7 },
				new EmployeeProjectEntity { Employee = employee8 }
			}
		};
		var project3 = new ProjectEntity
		{
			ExternalId = 3, Name = "Project3", Employees = {
				new EmployeeProjectEntity { Employee = employee6 },
				new EmployeeProjectEntity { Employee = employee7 },
				new EmployeeProjectEntity { Employee = employee8 }
			}
		};
		_context.Projects.AddRange(project1, project2, project3);

		var building1 = new BuildingEntity { Name = "F1" };
		var floor1 = new FloorEntity { Building = building1, FloorNumber = 0 };
		var room1 = new RoomEntity { Area = 22.5m, Name = "001", Floor = floor1 };

		_context.Desks.AddRange(
			DeskHelpers.CreateDeskWithReservation(room1, 1, employee6),
			DeskHelpers.CreateDeskWithReservation(room1, 2, employee7),
			DeskHelpers.CreateDeskWithReservation(room1, 3, employee8),
			new DeskEntity { Room = room1, Number = 4 }
		);

		var floor2 = new FloorEntity { Building = building1, FloorNumber = 1 };
		var room2 = new RoomEntity { Area = 12.8m, Name = "002", Floor = floor2 };

		_context.Desks.AddRange(
			DeskHelpers.CreateDeskWithReservation(room2, 1, employee4),
			DeskHelpers.CreateDeskWithReservation(room2, 2, employee5)
		);

		_context.SaveChanges();

		int unassignedPeopleMaxCount = 2;

		var query = new GetProjectsQuery(new ProjectsFiltersDto>
		{
			Filters =
			{
				UnassignedPeopleMaxCount = unassignedPeopleMaxCount
			},
			PageNumber = 0,
			PageSize = 100
		});
		var handler = new GetProjectsHandler(_context, _mapper);

		// when
		PagedListResultDto<IEnumerable<ProjectDto>> result = await handler.HandleAsync(query);

		// then
		result.Count.ShouldBe(2);
		result.Payload!.ShouldContain(p => p.Name == "Project2");
		result.Payload!.ShouldContain(p => p.Name == "Project3");
	}

	[Test]
	public async Task ExecuteAsync_UnassignedPeopleMinCountFilters_ReturnsFilteredProjects()
	{
		// given
		var employee1 = new EmployeeEntity { Name = "Jan", Surname = "Kowalski", Email = "jkowalski@fp.pl", ExternalId = 1, UserLogin = ""};
		var employee2 = new EmployeeEntity { Name = "Mariusz", Surname = "Szczepañski", Email = "mszczepanski@fp.pl", ExternalId = 2, UserLogin = "mszczepanski" };
		var employee3 = new EmployeeEntity { Name = "Katarzyna", Surname = "Nowak", Email = "knowak@fp.pl", ExternalId = 3, UserLogin = "knowak" };
		var employee4 = new EmployeeEntity { Name = "Marian", Surname = "Drzewiecki", Email = "mdrzewiecki@fp.pl", ExternalId = 4, UserLogin = "mdrzewiecki" };
		var employee5 = new EmployeeEntity { Name = "£ukasz", Surname = "Francuz", Email = "lfrancuz@fp.pl", ExternalId = 5, UserLogin = "lfrancuz" };
		var employee6 = new EmployeeEntity { Name = "Piotr", Surname = "Ryczek", Email = "pryczek@fp.pl", ExternalId = 6, UserLogin = "pryczek" };
		var employee7 = new EmployeeEntity { Name = "Maria", Surname = "Dobra", Email = "mdobra@fp.pl", ExternalId = 7, UserLogin = "mdobra" };
		var employee8 = new EmployeeEntity { Name = "Klaudia", Surname = "Lepsza", Email = "klepsza@fp.pl", ExternalId = 8, UserLogin = "klepsza" };

		var project1 = new ProjectEntity
		{
			ExternalId = 1, Name = "Project1", Employees = {
				new EmployeeProjectEntity { Employee = employee1 },
				new EmployeeProjectEntity { Employee = employee2 },
				new EmployeeProjectEntity { Employee = employee3 },
				new EmployeeProjectEntity { Employee = employee4 },
				new EmployeeProjectEntity { Employee = employee5 },
				new EmployeeProjectEntity { Employee = employee6 },
				new EmployeeProjectEntity { Employee = employee7 },
			}
		};
		var project2 = new ProjectEntity
		{
			ExternalId = 2, Name = "Project2", Employees = {
				new EmployeeProjectEntity { Employee = employee1 },
				new EmployeeProjectEntity { Employee = employee2 },
				new EmployeeProjectEntity { Employee = employee3 },
				new EmployeeProjectEntity { Employee = employee4 },
				new EmployeeProjectEntity { Employee = employee5 },
				new EmployeeProjectEntity { Employee = employee6 }
			}
		};
		var project3 = new ProjectEntity
		{
			ExternalId = 3, Name = "Project3", Employees = {
				new EmployeeProjectEntity { Employee = employee6 },
				new EmployeeProjectEntity { Employee = employee7 },
				new EmployeeProjectEntity { Employee = employee8 }
			}
		};
		_context.Projects.AddRange(project1, project2, project3);

		var building1 = new BuildingEntity { Name = "F1" };
		var floor1 = new FloorEntity { Building = building1, FloorNumber = 0 };
		var room1 = new RoomEntity { Area = 22.5m, Name = "001", Floor = floor1 };

		_context.Desks.AddRange(
			DeskHelpers.CreateDeskWithReservation(room1, 1, employee1),
			DeskHelpers.CreateDeskWithReservation(room1, 2, employee2),
			DeskHelpers.CreateDeskWithReservation(room1, 3, employee3),
			DeskHelpers.CreateDeskWithReservation(room1, 4, employee4));

		var floor2 = new FloorEntity { Building = building1, FloorNumber = 1 };
		var room2 = new RoomEntity { Area = 12.8m, Name = "002", Floor = floor2 };

		_context.Desks.AddRange(
			DeskHelpers.CreateDeskWithReservation(room2, 1, employee5),
			new DeskEntity { Room = room2, Number = 2 }
		);

		_context.SaveChanges();

		int unassignedPeopleMinCount = 3;

		var query = new GetProjectsQuery(new ProjectsFiltersDto>
		{
			Filters =
			{
				UnassignedPeopleMinCount = unassignedPeopleMinCount
			},
			PageNumber = 0,
			PageSize = 100
		});
		var handler = new GetProjectsHandler(_context, _mapper);

		// when
		PagedListResultDto<IEnumerable<ProjectDto>> result = await handler.HandleAsync(query);

		// then
		result.Count.ShouldBe(1);
		result.Payload!.Single().Name.ShouldBe("Project3");
	}

	[Test]
	public async Task ExecuteAsync_TeamLeaderIdsFilters_ReturnsFilteredProjects()
	{
		// given
		var employeeId1 = new Guid("EF8D8328-1C42-4CC6-A34C-7A095B277468");
		var employeeId2 = new Guid("62BDA8C7-D0D5-4CF5-B5C4-A93B5AD17EE0");
		var employeeId3 = new Guid("81808F28-9610-490E-B631-DE83DCD7D84F");
		var employeeId4 = new Guid("ACA98F47-E1F5-4360-B92E-C4D0EA49A0EE");
		var employeeId5 = new Guid("814F56B4-56A7-4ADB-9F00-81FB08FD907C");

		var employee1 = new EmployeeEntity { Id = employeeId1, Name = "Jan", Surname = "Kowalski", Email = "jkowalski@fp.pl", ExternalId = 1, UserLogin = "jkowalski" };
		var employee2 = new EmployeeEntity { Id = employeeId2, Name = "Mariusz", Surname = "Szczepañski", Email = "mszczepanski@fp.pl", ExternalId = 2, UserLogin = "mszczepanski" };
		var employee3 = new EmployeeEntity { Id = employeeId3, Name = "Katarzyna", Surname = "Nowak", Email = "knowak@fp.pl", ExternalId = 3, UserLogin = "knowak" };
		var employee4 = new EmployeeEntity { Id = employeeId4, Name = "Marian", Surname = "Drzewiecki", Email = "mdrzewiecki@fp.pl", ExternalId = 4, UserLogin = "mdrzewiecki" };
		var employee5 = new EmployeeEntity { Id = employeeId5, Name = "£ukasz", Surname = "Francuz", Email = "lfrancuz@fp.pl", ExternalId = 5, UserLogin = "lfrancuz" };

		var project1 = new ProjectEntity
		{
			ExternalId = 1, Name = "Project1", Employees = {
				new EmployeeProjectEntity { Employee = employee1, IsTeamLeaderProjectRole = true }
			}
		};
		var project2 = new ProjectEntity
		{
			ExternalId = 2, Name = "Project2", Employees = {
				new EmployeeProjectEntity { Employee = employee1, IsTeamLeaderProjectRole = true },
				new EmployeeProjectEntity { Employee = employee2, IsTeamLeaderProjectRole = true },
				new EmployeeProjectEntity { Employee = employee3, IsTeamLeaderProjectRole = true },
			}
		};
		var project3 = new ProjectEntity
		{
			ExternalId = 3, Name = "Project3", Employees = {
				new EmployeeProjectEntity { Employee = employee1, IsTeamLeaderProjectRole = true },
				new EmployeeProjectEntity { Employee = employee2 },
				new EmployeeProjectEntity { Employee = employee3, IsTeamLeaderProjectRole = true }
			}
		};
		var project4 = new ProjectEntity
		{
			ExternalId = 4, Name = "Project4", Employees = {
				new EmployeeProjectEntity { Employee = employee4, IsTeamLeaderProjectRole = true },
				new EmployeeProjectEntity { Employee = employee5, IsTeamLeaderProjectRole = true },
			}
		};
		var project5 = new ProjectEntity
		{
			ExternalId = 5, Name = "Project5"
		};
		var project6 = new ProjectEntity
		{
			ExternalId = 6, Name = "Project6", Employees = {
				new EmployeeProjectEntity { Employee = employee2 },
				new EmployeeProjectEntity { Employee = employee5, IsTeamLeaderProjectRole = true },
			}
		};
		var project7 = new ProjectEntity
		{
			ExternalId = 7, Name = "Project7", Employees = {
				new EmployeeProjectEntity { Employee = employee2, IsTeamLeaderProjectRole = true },
				new EmployeeProjectEntity { Employee = employee4, IsTeamLeaderProjectRole = true },
			}
		};
		_context.Projects.AddRange(project1, project2, project3, project4, project5, project6, project7);
		_context.SaveChanges();

		var teamLeaderIds = new List<Guid> { employeeId1, employeeId2 };

		var query = new GetProjectsQuery(new ProjectsFiltersDto>
		{
			Filters =
			{
				TeamLeaderIds = teamLeaderIds
			},
			PageNumber = 0,
			PageSize = 100
		});
		var handler = new GetProjectsHandler(_context, _mapper);

		// when
		PagedListResultDto<IEnumerable<ProjectDto>> result = await handler.HandleAsync(query);

		// then
		result.Count.ShouldBe(4);
		result.Payload!.ShouldContain(p => p.Name == "Project1");
		result.Payload!.ShouldContain(p => p.Name == "Project2");
		result.Payload!.ShouldContain(p => p.Name == "Project3");
		result.Payload!.ShouldContain(p => p.Name == "Project7");
	}

	[Test]
	public async Task ExecuteAsync_ProjectIdsFilters_ReturnsFilteredProjects()
	{
		// given
		var projectId1 = new Guid("EF8D8328-1C42-4CC6-A34C-7A095B277468");
		var projectId2 = new Guid("62BDA8C7-D0D5-4CF5-B5C4-A93B5AD17EE0");
		var projectId3 = new Guid("81808F28-9610-490E-B631-DE83DCD7D84F");
		var projectId4 = new Guid("ACA98F47-E1F5-4360-B92E-C4D0EA49A0EE");
		var projectId5 = new Guid("814F56B4-56A7-4ADB-9F00-81FB08FD907C");

		var project1 = new ProjectEntity
		{
			Id = projectId1,
			ExternalId = 1,
			Name = "Project1",
		};
		var project2 = new ProjectEntity
		{
			Id = projectId2,
			ExternalId = 2,
			Name = "Project2",
		};
		var project3 = new ProjectEntity
		{
			Id = projectId3,
			ExternalId = 3,
			Name = "Project3",
		};
		var project4 = new ProjectEntity
		{
			Id = projectId4,
			ExternalId = 4,
			Name = "Project4",
		};
		var project5 = new ProjectEntity
		{
			Id = projectId5,
			ExternalId = 5,
			Name = "Project5",
		};

		_context.Projects.AddRange(project1, project2, project3, project4, project5);
		_context.SaveChanges();

		var projectIds = new List<Guid> { projectId1, projectId4, projectId5 };

		var query = new GetProjectsQuery(new ProjectsFiltersDto>
		{
			Filters =
			{
				ProjectIds = projectIds
			},
			PageNumber = 0,
			PageSize = 100
		});
		var handler = new GetProjectsHandler(_context, _mapper);

		// when
		PagedListResultDto<IEnumerable<ProjectDto>> result = await handler.HandleAsync(query);

		// then
		result.Count.ShouldBe(3);
		result.Payload!.ShouldContain(p => p.Name == "Project1");
		result.Payload!.ShouldContain(p => p.Name == "Project4");
		result.Payload!.ShouldContain(p => p.Name == "Project5");
	}

}
