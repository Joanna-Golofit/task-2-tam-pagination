using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
public class GetTeamLeadersHandlerTests
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly EmployeesRepository _employeeRepository;

	public GetTeamLeadersHandlerTests()
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
		_context.Roles.Add(RoleEntity.CreateRole(RoleEntity.TeamLeader));
		_context.SaveChangesAsync();
	}

	[Test]
	public async Task ExecuteAsync_EmptyDatabase_ReturnsEmptyList()
	{
		// given
		var query = new GetTeamLeadersQuery();
		var queryHandler = new GetTeamLeadersHandler(_employeeRepository, _mapper);

		// when
		IEnumerable<TeamLeaderDto> result = await queryHandler.HandleAsync(query);

		// then
		result.ShouldBeEmpty();
	}

	[Test]
	public async Task ExecuteAsync_TeamLeadersExist_ReturnsProperlySetTeamLeaders()
	{
		// given
		var teamLeader1 = new EmployeeEntity { Name = "Jan", Surname = "Kowalski", Email = "jkowalski@fp.pl", ExternalId = 1, UserLogin = "jkowalski" };
		var teamLeader2 = new EmployeeEntity { Name = "Mariusz", Surname = "Szczepañski", Email = "mszczepanski@fp.pl", ExternalId = 2, UserLogin = "mszczepanski" };
		var teamLeader3 = new EmployeeEntity { Name = "Katarzyna", Surname = "Nowak", Email = "knowak@fp.pl", ExternalId = 3, UserLogin = "knowak" };

		var employee1 = new EmployeeEntity { Name = "Piotr", Surname = "Ryczek", Email = "pryczek@fp.pl", ExternalId = 4, UserLogin = "pryczek" };

		var teamLeaderRole = _context.Roles.Where(r => r.Name.Equals(RoleEntity.TeamLeader)).Single();
		var userRole1 = new UserRoleEntity { Role = teamLeaderRole, RoleId = teamLeaderRole.Id, Employee = teamLeader1 };
		var userRole2 = new UserRoleEntity { Role = teamLeaderRole, RoleId = teamLeaderRole.Id, Employee = teamLeader2 };
		var userRole3 = new UserRoleEntity { Role = teamLeaderRole, RoleId = teamLeaderRole.Id, Employee = teamLeader3 };

		_context.UserRoles.AddRange(userRole1, userRole2, userRole3);
		_context.Employees.AddRange(teamLeader1, teamLeader2, teamLeader3, employee1);
		_context.SaveChanges();

		var query = new GetTeamLeadersQuery();
		var queryHandler = new GetTeamLeadersHandler(_employeeRepository, _mapper);

		// when
		IEnumerable<TeamLeaderDto> result = await queryHandler.HandleAsync(query);

		// then
		result.Count().ShouldBe(3);
		result.ShouldContain(tl => tl.Email == "jkowalski@fp.pl");
		result.ShouldContain(tl => tl.Email == "mszczepanski@fp.pl");
		result.ShouldContain(tl => tl.Email == "knowak@fp.pl");
	}
}
