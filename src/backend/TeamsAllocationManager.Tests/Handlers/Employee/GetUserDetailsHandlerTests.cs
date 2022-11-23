using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TeamsAllocationManager.Contracts.Employee.Queries;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Domain.Enums;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Dtos.Employee;
using TeamsAllocationManager.Infrastructure.Handlers.Employee;
using TeamsAllocationManager.Mapper.Profiles;

namespace TeamsAllocationManager.Tests.Handlers.Employee;


[TestFixture]
public class GetUserDetailsHandlerTests
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly EmployeesRepository _employeeRepository;

	public GetUserDetailsHandlerTests()
	{
		DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: GetType().Name)
			.Options;
		_context = new ApplicationDbContext(options);
		_mapper = new MapperConfiguration(mc => mc.AddProfile(new EntityDtoProfile())).CreateMapper();
		_employeeRepository = new EmployeesRepository(_context);
	}

	[SetUp]
	public void SetupBeforeEachTest()
	{
		_context.ClearDatabase();
		_context.SaveChangesAsync();
	}

	[Test]
	public async Task ExecuteAsync_UserExists_ReturnsUserDetails()
	{
		// given
		var floor1 = new FloorEntity
		{
			Id = Guid.NewGuid(),
			Building = new BuildingEntity
			{
				Id = Guid.NewGuid(),
				Name = "budynek1"
			}
		};

		var teamLeaderRole = RoleEntity.CreateRole(RoleEntity.TeamLeader);

		var teamLeader1 = new EmployeeEntity { Id = Guid.NewGuid(), Name = "Jan", Surname = "Kowalski", Email = "jkowalski@fp.pl", ExternalId = 1, UserRoles = new List<UserRoleEntity> { new UserRoleEntity { Id = Guid.NewGuid(), Role = teamLeaderRole } }, UserLogin = "jkowalski" };
		var teamLeader2 = new EmployeeEntity { Id = Guid.NewGuid(), Name = "Marcin", Surname = "Kisiel", Email = "mkisiel@fp.pl", ExternalId = 2, UserRoles = new List<UserRoleEntity> { new UserRoleEntity { Id = Guid.NewGuid(), Role = teamLeaderRole } }, UserLogin = "mkisiel" };
		var teamLeader3 = new EmployeeEntity { Id = Guid.NewGuid(), Name = "Wioletta", Surname = "Willas", Email = "wwillas@fp.pl", ExternalId = 3, UserRoles = new List<UserRoleEntity> { new UserRoleEntity { Id = Guid.NewGuid(), Role = teamLeaderRole } }, UserLogin = "wwillas" };

		var employee1 = new EmployeeEntity
		{
			Id = Guid.NewGuid(), Name = "Ryszard", Surname = "B¹k", Email = "rbak@fp.pl", ExternalId = 4, UserLogin = "rbak",
			WorkspaceType = WorkspaceType.Remote,
			Projects = new List<EmployeeProjectEntity>
		{
			new EmployeeProjectEntity
			{
				Project = new ProjectEntity
				{
					Id = Guid.NewGuid(),
					Name = "projekt1",
					ExternalId = 1,
					Employees = {	new EmployeeProjectEntity { Employee = teamLeader1, IsTeamLeaderProjectRole = true },
									new EmployeeProjectEntity { Employee = teamLeader2, IsTeamLeaderProjectRole = true }
								}
				},
			},
			new EmployeeProjectEntity {
				Project = new ProjectEntity
				{
					Id = Guid.NewGuid(),
					Name = "projekt2",
					ExternalId = 2,
					Employees = { new EmployeeProjectEntity { Employee = teamLeader3, IsTeamLeaderProjectRole = true } }
				},
			}
		},
			UserRoles = new List<UserRoleEntity> { new UserRoleEntity { Id = Guid.NewGuid(), Role = teamLeaderRole } }
		};

		var employee2 = new EmployeeEntity { Id = Guid.NewGuid(), Name = "Piotr", Surname = "Ryczek", Email = "pryczek@fp.pl", ExternalId = 5, UserLogin = "pryczek" };

		_context.Employees.AddRange(employee1, employee2);
		_context.SaveChanges();

		var query = new GetUserDetailsQuery(employee1.Id);
		var queryHandler = new GetUserDetailsHandler(_employeeRepository, _mapper);

		// when
		UserDetailsDto result = await queryHandler.HandleAsync(query);

		// then
		Assert.AreEqual(employee1.Id, result.Id);
		Assert.AreEqual(employee1.Email, result.Email);
		Assert.AreEqual((int)employee1.WorkspaceType, result.WorkspaceType);
		Assert.AreEqual(1, result.EmployeeType);
		Assert.AreEqual(employee1.Projects.Count, result.Projects.Count);

		var employeeProjects = employee1.Projects.ToList();
		Assert.AreEqual(employeeProjects[0].ProjectId, result.Projects[0].Id);
		Assert.AreEqual(employeeProjects[1].ProjectId, result.Projects[1].Id);
		Assert.AreEqual(employeeProjects[0].Project.Name, result.Projects[0].Name);
		Assert.AreEqual(employeeProjects[1].Project.Name, result.Projects[1].Name);

		var projectOneTeamleaders = employeeProjects[0].Project.TeamLeaders.OrderBy(tl => tl.Surname).ToList();
		var dtoProjectOneTeamleaders = result.Projects[0].TeamLeadersNames.ToList();
			
		Assert.AreEqual($"{projectOneTeamleaders[0].Name} {projectOneTeamleaders[0].Surname}", dtoProjectOneTeamleaders[0]);
		Assert.AreEqual($"{projectOneTeamleaders[1].Name} {projectOneTeamleaders[1].Surname}", dtoProjectOneTeamleaders[1]);

		var projectTwoTeamleaders = employeeProjects[1].Project.TeamLeaders.OrderBy(tl => tl.Surname).ToList();
		var dtoProjectTwoTeamleaders = result.Projects[1].TeamLeadersNames.ToList();
		Assert.AreEqual($"{projectTwoTeamleaders[0].Name} {projectTwoTeamleaders[0].Surname}", dtoProjectTwoTeamleaders[0]);

		var locations = result.Locations.ToList();
	}
}
