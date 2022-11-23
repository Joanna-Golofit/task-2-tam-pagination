using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.LoggedUser.Queries;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Infrastructure.Handlers.LoggedUser;
using TeamsAllocationManager.Mapper.Profiles;

namespace TeamsAllocationManager.Tests.Handlers.Employee;

[TestFixture]
public class GetLoggedUserDataHandlerTests
{
	private readonly ApplicationDbContext _applicationDbContext;
	private readonly GetLoggedUserDataHandler _handler;
	private readonly IMapper _mapper;

	public GetLoggedUserDataHandlerTests()
	{
		DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
						.UseInMemoryDatabase(databaseName: GetType().Name)
						.Options;
		_applicationDbContext = new ApplicationDbContext(options);
		_mapper = new MapperConfiguration(mc => mc.AddProfile(new EntityDtoProfile())).CreateMapper();
		_handler = new GetLoggedUserDataHandler(new EmployeesRepository(_applicationDbContext), _mapper);
	}

	[SetUp]
	public void SetupBeforeEachTest()
	{
		_applicationDbContext.ClearDatabase();
		_applicationDbContext.SaveChangesAsync();
	}

	[Test]
	public async Task ShouldGetLoggedUserData()
	{
		var loggedUserEmail = "jkowalski@fp.pl";

		var teamLeaderRole = RoleEntity.CreateRole(RoleEntity.TeamLeader);
		var adminRole = RoleEntity.CreateRole(RoleEntity.Admin);

		var employee1 = new EmployeeEntity
		{
			Id = Guid.NewGuid(), Name = "Jan", Surname = "Kowalski", Email = "jkowalski@fp.pl", ExternalId = 1, UserLogin = "jkowalski",
			UserRoles = new List<UserRoleEntity> {
				new UserRoleEntity { Id = Guid.NewGuid(), Role = teamLeaderRole },
				new UserRoleEntity { Id = Guid.NewGuid(), Role = adminRole }
			}
		};
		var employee2 = new EmployeeEntity
		{
			Id = Guid.NewGuid(), Name = "Marcin", Surname = "Kisiel", Email = "mkisiel@fp.pl", ExternalId = 2, UserLogin = "mkisiel",
			UserRoles = new List<UserRoleEntity> { new UserRoleEntity { Id = Guid.NewGuid(), Role = teamLeaderRole } }
		};

		_applicationDbContext.Employees.AddRange(employee1, employee2);
		_applicationDbContext.SaveChanges();

		var query = new GetLoggedUserDataQuery(loggedUserEmail, employee1.Email);

		// when
		var result = await _handler.HandleAsync(query);

		// then
		result.ShouldNotBeNull();
		result.Id.ShouldBe(employee1.Id);
		result.Email.ShouldBe(employee1.Email);
		result.RoleNames.Count().ShouldBe(employee1.UserRoles.Count);
		for (int i = 0; i < employee1.UserRoles.Count; i++)
		{
			result.RoleNames.ShouldContain(employee1.UserRoles[i].Role.Name);
		}
	}

	// TODO: temporary commented to not block functionality
	//[Test]
	//public void ShouldThrowForIncorrectUser()
	//{
	//	var loggedUserEmail = "jkowalski@fp.pl";

	//	var teamLeaderRole = RoleEntity.CreateRole(RoleEntity.TeamLeader);
	//	var adminRole = RoleEntity.CreateRole(RoleEntity.Admin);

	//	var employee1 = new EmployeeEntity
	//	{
	//		EmployeeId = Guid.NewGuid(), Name = "Jan", Surname = "Kowalski", Email = "incorrect@fp.pl", ExternalId = 1,
	//		UserRoles = new List<UserRoleEntity> {
	//			new UserRoleEntity { EmployeeId = Guid.NewGuid(), Role = teamLeaderRole },
	//			new UserRoleEntity { EmployeeId = Guid.NewGuid(), Role = adminRole }
	//		}
	//	};
	//	var employee2 = new EmployeeEntity
	//	{
	//		EmployeeId = Guid.NewGuid(), Name = "Marcin", Surname = "Kisiel", Email = "mkisiel@fp.pl", ExternalId = 2,
	//		UserRoles = new List<UserRoleEntity> { new UserRoleEntity { EmployeeId = Guid.NewGuid(), Role = teamLeaderRole } }
	//	};

	//	_applicationDbContext.Employees.AddRange(employee1, employee2);
	//	_applicationDbContext.SaveChanges();

	//	var query = new GetLoggedUserDataQuery(loggedUserEmail, employee1.Email);

	//	// when-then
	//	var aggregateException = Should.Throw<AggregateException>(() => _handler.HandleAsync(query).Result);
	//	aggregateException.InnerExceptions.Count.ShouldBe(1);
	//	aggregateException.InnerException.ShouldBeOfType<UnauthorizedAccessException>();
	//	aggregateException.InnerException.Message.ShouldNotBeNullOrWhiteSpace();
	//}
}
