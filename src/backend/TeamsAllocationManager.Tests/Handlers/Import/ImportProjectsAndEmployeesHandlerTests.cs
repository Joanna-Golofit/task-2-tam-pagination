using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TeamsAllocationManager.Contracts.Import.Commands;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Domain.Enums;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Domain.Models.ConfigDataModels;
using TeamsAllocationManager.Infrastructure.Handlers.Import;
using TeamsAllocationManager.Infrastructure.Options;
using TeamsAllocationManager.Integrations.FutureDatabase.Clients;
using TeamsAllocationManager.Integrations.FutureDatabase.Enums;
using TeamsAllocationManager.Integrations.FutureDatabase.Models;
using TeamsAllocationManager.Mapper.Profiles;

namespace TeamsAllocationManager.Tests.Handlers.Import;

[TestFixture]
public class ImportProjectsAndEmployeesHandlerTests
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly Mock<IFutureDatabaseApiClient> _futureDatabaseApiClientMock;
	private readonly ImportProjectsAndEmployeesHandler _handler;

	public ImportProjectsAndEmployeesHandlerTests()
	{
		DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: GetType().Name)
			.Options;
		_context = new ApplicationDbContext(options);
		_mapper = new MapperConfiguration(mc => mc.AddProfile(new FutureDatabaseEntityProfile())).CreateMapper();

		using var logFactory = LoggerFactory.Create(builder => { builder.ClearProviders(); });
		var logger = logFactory.CreateLogger<ImportProjectsAndEmployeesHandler>();
		var autoImportOptions = Options.Create(new AutoImportSettings());

		_futureDatabaseApiClientMock = new Mock<IFutureDatabaseApiClient>();

		_handler = new ImportProjectsAndEmployeesHandler(_context, _futureDatabaseApiClientMock.Object, logger, _mapper, autoImportOptions);
	}

	[SetUp]
	public void SetupBeforeEachTest()
	{
		_context.ClearDatabase();
		_context.Roles.Add(RoleEntity.CreateRole(RoleEntity.TeamLeader));
		var configIgnored = ConfigEntity.CreateIgnoredProjectsConfigEntity();
		var configDivs = ConfigEntity.CreateDivisionsConfigEntity();
		configDivs.Data = @"{""1000"":""Division A"",""2000"":""Division B""}";
		_context.Configs.AddRange(configIgnored, configDivs);
		_context.SaveChangesAsync();
		_context.AddAdminEmployee();
	}

	[Test]
	public async Task HandleAsync_NoExistingEmployees_AddsEmployeesWithProjectsOnly()
	{
		// given
		var fromDate = DateTime.Now.AddDays(-5).Date;
		var toDate = DateTime.Now.AddDays(5).Date;

		var user1 = new User { Id = 1, DomainUserLogin = "mnowak", FirstName = "Marcin", LastName = "Nowak", Email = "mnowak@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var user2 = new User { Id = 2, DomainUserLogin = "akowalska", FirstName = "Anna", LastName = "Kowalska", Email = "akowalska@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var user3 = new User { Id = 3, DomainUserLogin = "lwierzba", FirstName = "ㄆkasz", LastName = "Wierzba", Email = "lwierzba@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var importedUsers = new List<User> { user1, user2, user3 };

		var group1 = new Group { Id = 1, ParentGroupId = 1000, Assignments = { new Assignment { GroupId = 1, UserId = 1, RoleId = 1, FromDate = fromDate, ToDate = toDate } }, Name = "Project1"};
		var group2 = new Group { Id = 2, ParentGroupId = 1000, Assignments = { new Assignment { GroupId = 2, UserId = 2, RoleId = 1, FromDate = fromDate, ToDate = toDate } }, Name = "Project2" };
		var importedGroups = new List<Group> { group1, group2 };

		_futureDatabaseApiClientMock.Setup(x => x.GetUsersAsync())
			.ReturnsAsync(importedUsers);
		_futureDatabaseApiClientMock.Setup(x => x.GetGroupsAsync())
			.ReturnsAsync(importedGroups);

		var adminEmail = _context.Employees.First(x => x.UserRoles.Any(y => y.Role.Name == RoleEntity.Admin)).Email;

		var command = new ImportProjectsAndEmployeesCommand(adminEmail);

		// when
		await _handler.HandleAsync(command);

		// then
		_context.Employees.Count().ShouldBe(2);
		// TODO: Temporary solution
		_context.Employees.SingleOrDefault(e => e.ExternalId == user1.Id && e.UserLogin == user1.DomainUserLogin /*&& e.Name == user1.FirstName && e.Surname == user1.LastName*/ && e.Email == user1.Email)
			.ShouldNotBeNull();
		_context.Employees.SingleOrDefault(e => e.ExternalId == user2.Id && e.UserLogin == user2.DomainUserLogin /*&& e.Name == user2.FirstName && e.Surname == user2.LastName*/ && e.Email == user2.Email)
			.ShouldNotBeNull();
	}

	[Test]
	public async Task HandleAsync_NoExistingEmployees_AddsNewEmployees()
	{
		// given
		var fromDate = DateTime.Now.AddDays(-5).Date;
		var toDate = DateTime.Now.AddDays(5).Date;

		var user1 = new User { Id = 1, DomainUserLogin = "mnowak", FirstName = "Marcin", LastName = "Nowak", Email = "mnowak@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var user2 = new User { Id = 2, DomainUserLogin = "akowalska", FirstName = "Anna", LastName = "Kowalska", Email = "akowalska@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var user3 = new User { Id = 3, DomainUserLogin = "lwierzba", FirstName = "ㄆkasz", LastName = "Wierzba", Email = "lwierzba@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var importedUsers = new List<User> { user1, user2, user3 };

		var group1 = new Group { Id = 1, ParentGroupId = 1000, Assignments = { new Assignment { GroupId = 1, UserId = 1, RoleId = 1, FromDate = fromDate, ToDate = toDate } }, Name = "Project1" };
		var group2 = new Group { Id = 2, ParentGroupId = 1000, Assignments = { new Assignment { GroupId = 2, UserId = 2, RoleId = 1, FromDate = fromDate, ToDate = toDate } }, Name = "Project2" };
		var group3 = new Group { Id = 3, ParentGroupId = 1000, Assignments = { new Assignment { GroupId = 3, UserId = 3, RoleId = 1, FromDate = fromDate, ToDate = toDate } }, Name = "Project3" };
		var importedGroups = new List<Group> { group1, group2, group3 };

		_futureDatabaseApiClientMock.Setup(x => x.GetUsersAsync())
			.ReturnsAsync(importedUsers);
		_futureDatabaseApiClientMock.Setup(x => x.GetGroupsAsync())
			.ReturnsAsync(importedGroups);

		var adminEmail = _context.Employees.First(x => x.UserRoles.Any(y => y.Role.Name == RoleEntity.Admin)).Email;

		var command = new ImportProjectsAndEmployeesCommand(adminEmail);

		// when
		await _handler.HandleAsync(command);

		// then
		_context.Employees.Count().ShouldBe(3);

		// TODO: Temporary solution
		_context.Employees.SingleOrDefault(e => e.ExternalId == user1.Id && e.UserLogin == user1.DomainUserLogin /*&& e.Name == user1.FirstName && e.Surname == user1.LastName*/ && e.Email == user1.Email)
			.ShouldNotBeNull();
		_context.Employees.SingleOrDefault(e => e.ExternalId == user2.Id && e.UserLogin == user2.DomainUserLogin /*&& e.Name == user2.FirstName && e.Surname == user2.LastName*/ && e.Email == user2.Email)
			.ShouldNotBeNull();
		_context.Employees.SingleOrDefault(e => e.ExternalId == user3.Id && e.UserLogin == user3.DomainUserLogin /*&& e.Name == user3.FirstName && e.Surname == user3.LastName*/ && e.Email == user3.Email)
			.ShouldNotBeNull();
	}

	[Test]
	public async Task HandleAsync_EmployeesAlreadyExist_AddsNewAndUpdatesExistingEmployees()
	{
		// given
		var fromDate = DateTime.Now.AddDays(-5).Date;
		var toDate = DateTime.Now.AddDays(5).Date;

		var employee1 = new EmployeeEntity { ExternalId = 1, UserLogin = "mnowak", Name = "Marcin", Surname = "Nowak", Email = "mnowak@fp.pl" };
		var employee2 = new EmployeeEntity { ExternalId = 2, UserLogin = "akowalska", Name = "Anna", Surname = "Kowalska", Email = "akowalska@fp.pl" };
		var employee3 = new EmployeeEntity { ExternalId = 3, UserLogin = "lwierzba", Name = "ㄆkasz", Surname = "Wierzba", Email = "lwierzba@fp.pl" };

		var user2 = new User { Id = 2, DomainUserLogin = "jmlynarska", FirstName = "Joanna", LastName = "M造narska", Email = "jmlynarska@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var user3 = new User { Id = 3, DomainUserLogin = "lwierzba", FirstName = "ㄆkasz", LastName = "Wierzba", Email = "lwierzba@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var user4 = new User { Id = 4, DomainUserLogin = "bstolik", FirstName = "Bogdan", LastName = "Stolik", Email = "bstolik@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var user5 = new User { Id = 5, DomainUserLogin = "mkrawczyk", FirstName = "Maria", LastName = "Krawczyk", Email = "mkrawczyk@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };

		var group1 = new Group
		{
			Id = 1, ParentGroupId = 1000, 
			Assignments = 
			{
				new Assignment { GroupId = 1, UserId = 1, RoleId = 1, FromDate = fromDate, ToDate = toDate },
				new Assignment { GroupId = 1, UserId = 4, RoleId = 1, FromDate = fromDate, ToDate = toDate }
			},
			Name = "Project1"
		};
		var group2 = new Group
		{
			Id = 2, ParentGroupId = 1000, 
			Assignments = 
			{
				new Assignment { GroupId = 2, UserId = 2, RoleId = 1, FromDate = fromDate, ToDate = toDate },
				new Assignment { GroupId = 2, UserId = 5, RoleId = 1, FromDate = fromDate, ToDate = toDate }
			},
			Name = "Project2"
		};
		var group3 = new Group
		{
			Id = 3, ParentGroupId = 1000, 
			Assignments =
			{
				new Assignment { GroupId = 3, UserId = 3, RoleId = 1, FromDate = fromDate, ToDate = toDate }
			},
			Name = "Project3"
		};

		var employeeProjectEntity1 = new EmployeeProjectEntity { Employee = employee1, Project = new ProjectEntity { Id = Guid.NewGuid(), ExternalId = 1, Name = "Project1" } };
		var employeeProjectEntity2 = new EmployeeProjectEntity { Employee = employee2, Project = new ProjectEntity { Id = Guid.NewGuid(), ExternalId = 2, Name = "Project2" } };
		var employeeProjectEntity3 = new EmployeeProjectEntity { Employee = employee3, Project = new ProjectEntity { Id = Guid.NewGuid(), ExternalId = 3, Name = "Project3" } };

		_context.Employees.AddRange(employee1, employee2, employee3);
		_context.EmployeeProjects.AddRange(employeeProjectEntity1, employeeProjectEntity2, employeeProjectEntity3);
		await _context.SaveChangesAsync();

		var importedUsers = new List<User> { user2, user3, user4, user5 };
		var importedGroups = new List<Group> { group1, group2, group3 };

		_futureDatabaseApiClientMock.Setup(x => x.GetUsersAsync())
			.ReturnsAsync(importedUsers);
		_futureDatabaseApiClientMock.Setup(x => x.GetGroupsAsync())
			.ReturnsAsync(importedGroups);

		var adminEmail = _context.Employees.First(x => x.UserRoles.Any(y => y.Role.Name == RoleEntity.Admin)).Email;

		var command = new ImportProjectsAndEmployeesCommand(adminEmail);

		// when
		await _handler.HandleAsync(command);

		// then
		_context.Employees.Count().ShouldBe(5);

		// TODO: Temporary solution
		_context.Employees.SingleOrDefault(e => e.ExternalId == employee2.ExternalId && e.UserLogin == user2.DomainUserLogin /*&& e.Name == user2.FirstName && e.Surname == user2.LastName*/ && e.Email == user2.Email)
			.ShouldNotBeNull();
		_context.Employees.SingleOrDefault(e => e.ExternalId == user3.Id && e.UserLogin == user3.DomainUserLogin /*&& e.Name == user3.FirstName && e.Surname == user3.LastName*/ && e.Email == user3.Email)
			.ShouldNotBeNull();
		_context.Employees.SingleOrDefault(e => e.ExternalId == user4.Id && e.UserLogin == user4.DomainUserLogin /*&& e.Name == user4.FirstName && e.Surname == user4.LastName*/ && e.Email == user4.Email)
			.ShouldNotBeNull();
		_context.Employees.SingleOrDefault(e => e.ExternalId == user5.Id && e.UserLogin == user5.DomainUserLogin /*&& e.Name == user5.FirstName && e.Surname == user5.LastName*/ && e.Email == user5.Email)
			.ShouldNotBeNull();
	}

	[Test]
	public async Task HandleAsync_ImportGroupsUsersAndAssignments_AddsEmployeeProjects()
	{
		// given
		var fromDate = DateTime.Now.AddDays(-5).Date;
		var toDate = DateTime.Now.AddDays(5).Date;

		var user1 = new User
		{
			Id = 1, DomainUserLogin = "mnowak", FirstName = "Marcin", LastName = "Nowak", Email = "mnowak@fp.pl", UserTypeId = (int)UserTypeFDB.Contractor,
			AssignmentUsers = new List<Assignment>
			{
				new Assignment { RoleId = (int)RoleFDB.TeamLeader, FromDate = fromDate, ToDate = toDate },
				new Assignment { RoleId = (int)RoleFDB.TeamLeader, FromDate = fromDate, ToDate = toDate },
				new Assignment { RoleId = (int)RoleFDB.QA, FromDate = fromDate, ToDate = toDate }
			}
		};
		var user2 = new User
		{
			Id = 2, DomainUserLogin = "akowalska", FirstName = "Anna", LastName = "Kowalska", Email = "akowalska@fp.pl", UserTypeId = (int)UserTypeFDB.Employee,
			AssignmentUsers = new List<Assignment>
			{
				new Assignment { RoleId = (int)RoleFDB.Developer, FromDate = fromDate, ToDate = toDate },
				new Assignment { RoleId = (int)RoleFDB.Developer, FromDate = fromDate, ToDate = toDate },
				new Assignment { RoleId = (int)RoleFDB.Developer, FromDate = fromDate, ToDate = toDate },
				new Assignment { RoleId = (int)RoleFDB.TeamLeader, FromDate = fromDate, ToDate = toDate }
			}
		};
		var user3 = new User
		{
			Id = 3, DomainUserLogin = "lwierzba", FirstName = "ㄆkasz", LastName = "Wierzba", Email = "lwierzba@fp.pl", UserTypeId = (int)UserTypeFDB.Employee,
			AssignmentUsers = new List<Assignment>
			{
				new Assignment { RoleId = (int)RoleFDB.Developer, FromDate = fromDate, ToDate = toDate }
			}
		};

		// user 4 does not exist intentionally 

		var user5 = new User
		{
			Id = 5, DomainUserLogin = "user5", FirstName = "user", LastName = "user5", Email = "user5@fp.pl", UserTypeId = (int)UserTypeFDB.Employee,
			AssignmentUsers = new List<Assignment>
			{
				new Assignment { RoleId = (int)RoleFDB.ProjectManager, FromDate = fromDate, ToDate = toDate }
			}
		};

		// user exisiting in db as Admin
		var user6 = new User
		{
			Id = 6, DomainUserLogin = "user6", FirstName = "user", LastName = "user6", Email = "user6@fp.pl", UserTypeId = (int)UserTypeFDB.Employee,
			AssignmentUsers = new List<Assignment>
			{
				new Assignment { RoleId = (int)RoleFDB.ProjectManager, FromDate = fromDate, ToDate = toDate }
			}
		};

		var importedUsers = new List<User> { user1, user2, user3, user5, user6 };

		// TODO: Restore ids
		var group1 = new Group
		{
			Id = 10001, Name = "Project One", ParentGroupId = 1000, Assignments = new List<Assignment>
			{
				new Assignment { Id = 1, GroupId = 10001, UserId = 1, RoleId = (int)RoleFDB.TeamLeader, FromDate = fromDate, ToDate = toDate},
				new Assignment { Id = 2, GroupId = 10001, UserId = 2, RoleId = (int)RoleFDB.Developer, FromDate = fromDate, ToDate = toDate}
			}
		};
		var group2 = new Group
		{
			Id = 10002, Name = "Project Two", ParentGroupId = 1000, Assignments = new List<Assignment>
			{
				new Assignment { Id = 3, GroupId = 10002, UserId = 2, RoleId = (int)RoleFDB.Developer, FromDate = fromDate, ToDate = toDate},
				new Assignment { Id = 4, GroupId = 10002, UserId = 3, RoleId = (int)RoleFDB.Developer, FromDate = fromDate, ToDate = toDate},
				new Assignment { Id = 5, GroupId = 10002, UserId = 3, RoleId = (int)RoleFDB.Developer, FromDate = fromDate, ToDate = toDate}
			}
		};
		var group3 = new Group { Id = 10003, Name = "Project Three" };
		var group4 = new Group
		{
			Id = 10004, Name = "Project Four", ParentGroupId = 1000, Assignments = new List<Assignment>
			{
				new Assignment { Id = 6, GroupId = 10004, UserId = 1, RoleId = (int)RoleFDB.TeamLeader, FromDate = fromDate, ToDate = toDate},
				new Assignment { Id = 7, GroupId = 10004, UserId = 2, RoleId = (int)RoleFDB.TeamManager, FromDate = fromDate, ToDate = toDate},
				new Assignment { Id = 8, GroupId = 10004, UserId = 5, RoleId = (int)RoleFDB.ProjectManager, FromDate = fromDate, ToDate = toDate},
				new Assignment { Id = 9, GroupId = 10004, UserId = 3, RoleId = (int)RoleFDB.Developer, FromDate = fromDate, ToDate = toDate}
			}
		};
		var group5 = new Group
		{
			Id = 10005, Name = "Project Five", ParentGroupId = 1000, Assignments = new List<Assignment>
			{
				new Assignment { Id = 10, GroupId = 10005, UserId = 1, RoleId = (int)RoleFDB.QA, FromDate = fromDate, ToDate = toDate},
				new Assignment { Id = 11, GroupId = 10005, UserId = 4, RoleId = (int)RoleFDB.Member, FromDate = fromDate, ToDate = toDate},
				new Assignment { Id = 12, GroupId = 10005, UserId = 6, RoleId = (int)RoleFDB.Developer, FromDate = fromDate, ToDate = toDate}
			}
		};
		var importedGroups = new List<Group> { group1, group2, group3, group4, group5 };


		var employee6 = new EmployeeEntity { UserLogin = "user6", Name = "user", Surname = "user6", Email = "user6@fp.pl", ExternalId = 6, WorkspaceType = WorkspaceType.Office };
		employee6.UserRoles.Add(new UserRoleEntity { Role = _context.Roles.Single(x => x.Name == RoleEntity.Admin) });
		_context.Add(employee6);
		_context.SaveChanges();

		_futureDatabaseApiClientMock.Setup(x => x.GetUsersAsync())
			.ReturnsAsync(importedUsers);
		_futureDatabaseApiClientMock.Setup(x => x.GetGroupsAsync())
			.ReturnsAsync(importedGroups);

		var adminEmail = _context.Employees.First(x => x.UserRoles.Any(y => y.Role.Name == RoleEntity.Admin)).Email;

		var command = new ImportProjectsAndEmployeesCommand(adminEmail);

		// when
		await _handler.HandleAsync(command);

		// then
		command.IsAutoImport.ShouldBeFalse();

		_context.Projects.Count(p => p.DivisionExternalId == null).ShouldBe(0);

		_context.Employees.Count().ShouldBe(5);
		_context.Employees.Where(x => x.WorkspaceType == null).Count().ShouldBe(4);
		_context.Employees.Count(x => x.WorkspaceType == WorkspaceType.Office).ShouldBe(1);

		EmployeeEntity? employee1 = _context.Employees.Include(e => e.Projects).SingleOrDefault(e => e.ExternalId == user1.Id);
		string employeeRole1 = _context.UserRoles.Where(ur => ur.Employee.Email == employee1!.Email).Select(ur => ur.Role).Single().Name;
		EmployeeEntity? employee2 = _context.Employees.Include(e => e.Projects).SingleOrDefault(e => e.ExternalId == user2.Id);
		string employeeRole2 = _context.UserRoles.Where(ur => ur.Employee.Email == employee2!.Email).Select(ur => ur.Role).Single().Name;
		EmployeeEntity? employee3 = _context.Employees.Include(e => e.Projects).SingleOrDefault(e => e.ExternalId == user3.Id);
		EmployeeEntity? employee4 = _context.Employees.Include(e => e.Projects).SingleOrDefault(e => e.ExternalId == 4);
		EmployeeEntity? employee5 = _context.Employees.Include(e => e.Projects).SingleOrDefault(e => e.ExternalId == 5);
		string employeeRole5 = _context.UserRoles.Where(ur => ur.Employee.Email == employee5!.Email).Select(ur => ur.Role).Single().Name;

		employee1.ShouldNotBeNull();
		employee1.Projects.Count.ShouldBe(3);
		employee1.LedProjects.Count.ShouldBe(2);
		employee1.IsContractor.ShouldBeTrue();
		employeeRole1.ShouldBe(RoleEntity.TeamLeader);

		employee2.ShouldNotBeNull();
		employee2.Projects.Count.ShouldBe(3);
		employee2.LedProjects.Count.ShouldBe(1);
		employee2.IsContractor.ShouldBeFalse();
		employeeRole2.ShouldBe(RoleEntity.TeamLeader);

		employee3.ShouldNotBeNull();
		employee3.Projects.Count.ShouldBe(2);
		employee3.LedProjects.Count.ShouldBe(0);
		employee3.IsContractor.ShouldBeFalse();

		employee4.ShouldBeNull();

		employee5.ShouldNotBeNull();
		employee5.Projects.Count.ShouldBe(1);
		employee5.LedProjects.Count.ShouldBe(1);
		employee5.IsContractor.ShouldBeFalse();
		employeeRole5.ShouldBe(RoleEntity.TeamLeader);

		_context.Projects.Count().ShouldBe(4);
		ProjectEntity? project1 = _context.Projects.Include(e => e.Employees).SingleOrDefault(e => e.ExternalId == group1.Id && e.Name == "Project One");
		ProjectEntity? project2 = _context.Projects.Include(e => e.Employees).SingleOrDefault(e => e.ExternalId == group2.Id && e.Name == "Project Two");
		ProjectEntity? project3 = _context.Projects.Include(e => e.Employees).SingleOrDefault(e => e.ExternalId == group3.Id && e.Name == "Project Three");
		ProjectEntity? project4 = _context.Projects.Include(e => e.Employees).SingleOrDefault(e => e.ExternalId == group4.Id && e.Name == "Project Four");
		ProjectEntity? project5 = _context.Projects.Include(e => e.Employees).SingleOrDefault(e => e.ExternalId == group5.Id && e.Name == "Project Five");

		project1.ShouldNotBeNull();
		project1.Employees.Count.ShouldBe(2);
		project1.TeamLeaders.Count.ShouldBe(1);

		project2.ShouldNotBeNull();
		project2.Employees.Count.ShouldBe(2);
		project2.TeamLeaders.Count.ShouldBe(0);

		project3.ShouldBeNull();

		project4.ShouldNotBeNull();
		project4.Employees.Count.ShouldBe(4);
		project4.TeamLeaders.Count.ShouldBe(3);

		project5.ShouldNotBeNull();
		project5.Employees.Count.ShouldBe(2);
		project5.TeamLeaders.Count.ShouldBe(0);

		_context.EmployeeProjects.Count().ShouldBe(10);

		_context.UserRoles.Count().ShouldBe(4);
		_context.UserRoles.Where(x => x.Role.Name == RoleEntity.Admin).Count().ShouldBe(1);
	}

	[Test]
	public async Task HandleAsync_UserRolesExist_RemovesAllAndSetsNewUserRoles()
	{
		// given
		var fromDate = DateTime.Now.AddDays(-5).Date;
		var toDate = DateTime.Now.AddDays(5).Date;

		var teamLeaderRole = await _context.Roles.SingleAsync(x => x.Name == RoleEntity.TeamLeader);

		var employee1 = new EmployeeEntity { Id = Guid.NewGuid(), ExternalId = 1, UserLogin = "mnowak", Name = "Marcin", Surname = "Nowak", Email = "mnowak@fp.pl" };
		var employee2 = new EmployeeEntity { Id = Guid.NewGuid(), ExternalId = 2, UserLogin = "akowalska", Name = "Anna", Surname = "Kowalska", Email = "akowalska@fp.pl" };
		var employee3 = new EmployeeEntity { Id = Guid.NewGuid(), ExternalId = 3, UserLogin = "lwierzba", Name = "ㄆkasz", Surname = "Wierzba", Email = "lwierzba@fp.pl" };

		var userRoles = new List<UserRoleEntity> {
			new UserRoleEntity { Employee = employee1, Role = teamLeaderRole},
			new UserRoleEntity { Employee = employee2, Role = teamLeaderRole},
		};

		_context.Employees.AddRange(employee1, employee2, employee3);
		await _context.SaveChangesAsync();

		var user2 = new User { Id = 2, DomainUserLogin = "jmlynarska", FirstName = "Joanna", LastName = "M造narska", Email = "jmlynarska@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var user3 = new User { Id = 3, DomainUserLogin = "lwierzba", FirstName = "ㄆkasz", LastName = "Wierzba", Email = "lwierzba@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var user4 = new User { Id = 4, DomainUserLogin = "bstolik", FirstName = "Bogdan", LastName = "Stolik", Email = "bstolik@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var user5 = new User { Id = 5, DomainUserLogin = "mkrawczyk", FirstName = "Maria", LastName = "Krawczyk", Email = "mkrawczyk@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var user6 = new User { Id = 6, DomainUserLogin = "rraczka", FirstName = "Remigiusz", LastName = "R鉍zka", Email = "rraczka@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var importedUsers = new List<User> { user2, user3, user4, user5, user6 };

		var group1 = new Group
		{
			Id = 10001, Name = "Project One", ParentGroupId = 1000, Assignments = new List<Assignment>
			{
				new Assignment { Id = 1, GroupId = 10001, UserId = 1, RoleId = (int)RoleFDB.TeamLeader, FromDate = fromDate, ToDate = toDate},
				new Assignment { Id = 2, GroupId = 10001, UserId = 2, RoleId = (int)RoleFDB.Developer, FromDate = fromDate, ToDate = toDate}
			}
		};

		var group2 = new Group
		{
			Id = 10002, Name = "Project Two", ParentGroupId = 1000, Assignments = new List<Assignment>
			{
				new Assignment { Id = 3, GroupId = 10002, UserId = 3, RoleId = (int)RoleFDB.TeamManager, FromDate = fromDate, ToDate = toDate},
				new Assignment { Id = 4, GroupId = 10002, UserId = 4, RoleId = (int)RoleFDB.Developer, FromDate = fromDate, ToDate = toDate}
			}
		};

		var group3 = new Group
		{
			Id = 10003, Name = "Project Three", ParentGroupId = 1000, Assignments = new List<Assignment>
			{
				new Assignment { Id = 5, GroupId = 10003, UserId = 5, RoleId = (int)RoleFDB.ProjectManager, FromDate = fromDate, ToDate = toDate},
				new Assignment { Id = 6, GroupId = 10003, UserId = 6, RoleId = (int)RoleFDB.Developer, FromDate = fromDate, ToDate = toDate}
			}
		};
		var importedGroups = new List<Group> { group1, group2, group3 };

		_futureDatabaseApiClientMock.Setup(x => x.GetUsersAsync())
			.ReturnsAsync(importedUsers);
		_futureDatabaseApiClientMock.Setup(x => x.GetGroupsAsync())
			.ReturnsAsync(importedGroups);

		var adminEmail = _context.Employees.First(x => x.UserRoles.Any(y => y.Role.Name == RoleEntity.Admin)).Email;

		var command = new ImportProjectsAndEmployeesCommand(adminEmail);

		// when
		await _handler.HandleAsync(command);

		// then
		_context.UserRoles.Count().ShouldBe(3);
		_context.UserRoles.Any(ur => ur.Employee.UserLogin == "mnowak" && ur.RoleId == teamLeaderRole.Id).ShouldBe(true);
		_context.UserRoles.Any(ur => ur.Employee.UserLogin == "mkrawczyk" && ur.RoleId == teamLeaderRole.Id).ShouldBe(true);
		_context.UserRoles.Any(ur => ur.Employee.UserLogin == "lwierzba" && ur.RoleId == teamLeaderRole.Id).ShouldBe(true);
	}

	[Test]
	public async Task HandleAsync_ShouldApplyAssignmentsPeriod()
	{
		// given
		var user1 = new User { Id = 1, DomainUserLogin = "jmlynarska", FirstName = "Joanna", LastName = "M造narska", Email = "jmlynarska@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var user2 = new User { Id = 2, DomainUserLogin = "lwierzba", FirstName = "ㄆkasz", LastName = "Wierzba", Email = "lwierzba@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var user3 = new User { Id = 3, DomainUserLogin = "bstolik", FirstName = "Bogdan", LastName = "Stolik", Email = "bstolik@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var user4 = new User { Id = 4, DomainUserLogin = "mkrawczyk", FirstName = "Maria", LastName = "Krawczyk", Email = "mkrawczyk@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var user5 = new User { Id = 5, DomainUserLogin = "rraczka", FirstName = "Remigiusz", LastName = "R鉍zka", Email = "rraczka@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var user6 = new User { Id = 6, DomainUserLogin = "rraczka2", FirstName = "Remigiusz2", LastName = "R鉍zka2", Email = "rraczka2@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var user7 = new User { Id = 7, DomainUserLogin = "rraczka3", FirstName = "Remigiusz3", LastName = "R鉍zka3", Email = "rraczka3@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var importedUsers = new List<User> { user1, user2, user3, user4, user5, user6, user7 };

		var group1 = new Group
		{
			Id = 10001, Name = "Project One", ParentGroupId = 1000, Assignments = new List<Assignment>
			{
				new Assignment { Id = 1, GroupId = 10001, UserId = 1, RoleId = (int)RoleFDB.Developer, FromDate = DateTime.Now.AddDays(-5).Date, ToDate = DateTime.Now.AddDays(5).Date},
				new Assignment { Id = 2, GroupId = 10001, UserId = 2, RoleId = (int)RoleFDB.Developer, FromDate = DateTime.Now.AddDays(-5).Date, ToDate = null},
				new Assignment { Id = 3, GroupId = 10001, UserId = 3, RoleId = (int)RoleFDB.Developer, FromDate = DateTime.Now.AddDays(5).Date, ToDate = null},
				new Assignment { Id = 4, GroupId = 10001, UserId = 4, RoleId = (int)RoleFDB.Developer, FromDate = DateTime.Now.AddDays(-10).Date, ToDate = DateTime.Now.AddDays(-5).Date},
				new Assignment { Id = 5, GroupId = 10001, UserId = 5, RoleId = (int)RoleFDB.Developer, FromDate = DateTime.Now.AddDays(5).Date, ToDate = DateTime.Now.AddDays(10).Date},
				new Assignment { Id = 6, GroupId = 10001, UserId = 6, RoleId = (int)RoleFDB.Developer, FromDate = DateTime.Now.Date, ToDate = DateTime.Now.AddDays(10).Date},
				new Assignment { Id = 7, GroupId = 10001, UserId = 7, RoleId = (int)RoleFDB.Developer, FromDate = DateTime.Now.AddDays(-10).Date, ToDate = DateTime.Now.Date},
			}
		};

		var importedGroups = new List<Group> { group1 };

		_futureDatabaseApiClientMock.Setup(x => x.GetUsersAsync())
			.ReturnsAsync(importedUsers);
		_futureDatabaseApiClientMock.Setup(x => x.GetGroupsAsync())
			.ReturnsAsync(importedGroups);

		var adminEmail = _context.Employees.First(x => x.UserRoles.Any(y => y.Role.Name == RoleEntity.Admin)).Email;

		var command = new ImportProjectsAndEmployeesCommand(adminEmail);

		// when
		await _handler.HandleAsync(command);

		// then
		_context.Employees.Count().ShouldBe(4);
		_context.Employees.Count(e => e.ExternalId == 1).ShouldBe(1);
		_context.Employees.Count(e => e.ExternalId == 2).ShouldBe(1);
		_context.Employees.Count(e => e.ExternalId == 6).ShouldBe(1);
		_context.Employees.Count(e => e.ExternalId == 7).ShouldBe(1);
	}

	[Test]
	public async Task HandleAsync_RemoveIgnoredProjects()
	{
		// given
		var fromDate = DateTime.Now.AddDays(-5).Date;
		var toDate = DateTime.Now.AddDays(5).Date;

		var config = _context.Configs.Single(c => c.Key == DbConfigKey.IgnoredProjects);
		config.AddIgnoredProject(new IgnoredProjectConfigData { ExternalId = 10001, Name = "Project One" });
		_context.SaveChanges();

		var teamLeaderRole = await _context.Roles.SingleAsync(x => x.Name == RoleEntity.TeamLeader);

		var employee1 = new EmployeeEntity { Id = Guid.NewGuid(), ExternalId = 1, UserLogin = "mnowak", Name = "Marcin", Surname = "Nowak", Email = "mnowak@fp.pl" };
		var employee2 = new EmployeeEntity { Id = Guid.NewGuid(), ExternalId = 2, UserLogin = "akowalska", Name = "Anna", Surname = "Kowalska", Email = "akowalska@fp.pl" };
		var employee3 = new EmployeeEntity { Id = Guid.NewGuid(), ExternalId = 3, UserLogin = "lwierzba", Name = "ㄆkasz", Surname = "Wierzba", Email = "lwierzba@fp.pl" };

		var userRoles = new List<UserRoleEntity> {
			new UserRoleEntity { Employee = employee1, Role = teamLeaderRole},
			new UserRoleEntity { Employee = employee2, Role = teamLeaderRole},
		};

		_context.Employees.AddRange(employee1, employee2, employee3);
		await _context.SaveChangesAsync();

		var user2 = new User { Id = 2, DomainUserLogin = "jmlynarska", FirstName = "Joanna", LastName = "M造narska", Email = "jmlynarska@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var user3 = new User { Id = 3, DomainUserLogin = "lwierzba", FirstName = "ㄆkasz", LastName = "Wierzba", Email = "lwierzba@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var user4 = new User { Id = 4, DomainUserLogin = "bstolik", FirstName = "Bogdan", LastName = "Stolik", Email = "bstolik@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var user5 = new User { Id = 5, DomainUserLogin = "mkrawczyk", FirstName = "Maria", LastName = "Krawczyk", Email = "mkrawczyk@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var user6 = new User { Id = 6, DomainUserLogin = "rraczka", FirstName = "Remigiusz", LastName = "R鉍zka", Email = "rraczka@fp.pl", UserTypeId = (int)UserTypeFDB.Employee };
		var importedUsers = new List<User> { user2, user3, user4, user5, user6 };

		var group1 = new Group
		{
			Id = 10001, Name = "Project One", ParentGroupId = 1000, Assignments = new List<Assignment>
			{
				new Assignment { Id = 1, GroupId = 10001, UserId = 1, RoleId = (int)RoleFDB.TeamLeader, FromDate = fromDate, ToDate = toDate},
				new Assignment { Id = 2, GroupId = 10001, UserId = 2, RoleId = (int)RoleFDB.Developer, FromDate = fromDate, ToDate = toDate}
			}
		};

		var group2 = new Group
		{
			Id = 10002, Name = "Project Two", ParentGroupId = 1000, Assignments = new List<Assignment>
			{
				new Assignment { Id = 3, GroupId = 10002, UserId = 3, RoleId = (int)RoleFDB.TeamManager, FromDate = fromDate, ToDate = toDate},
				new Assignment { Id = 4, GroupId = 10002, UserId = 4, RoleId = (int)RoleFDB.Developer, FromDate = fromDate, ToDate = toDate}
			}
		};

		var group3 = new Group
		{
			Id = 10003, Name = "Project Three", ParentGroupId = 1000, Assignments = new List<Assignment>
			{
				new Assignment { Id = 5, GroupId = 10003, UserId = 5, RoleId = (int)RoleFDB.ProjectManager, FromDate = fromDate, ToDate = toDate},
				new Assignment { Id = 6, GroupId = 10003, UserId = 6, RoleId = (int)RoleFDB.Developer, FromDate = fromDate, ToDate = toDate}
			}
		};
		var importedGroups = new List<Group> { group1, group2, group3 };

		_futureDatabaseApiClientMock.Setup(x => x.GetUsersAsync())
			.ReturnsAsync(importedUsers);
		_futureDatabaseApiClientMock.Setup(x => x.GetGroupsAsync())
			.ReturnsAsync(importedGroups);

		var adminEmail = _context.Employees.First(x => x.UserRoles.Any(y => y.Role.Name == RoleEntity.Admin)).Email;

		var command = new ImportProjectsAndEmployeesCommand(adminEmail);

		// when
		await _handler.HandleAsync(command);

		// then
		_context.Projects.Count().ShouldBe(2);
		_context.Projects.Any(p => p.ExternalId == 10001).ShouldBeFalse();
		_context.Employees.Count().ShouldBe(4);
		_context.Employees.Any(e => e.UserLogin == "mnowak").ShouldBeFalse();
		_context.Employees.Any(e => e.UserLogin == "jmlynarska").ShouldBeFalse();
	}
}
