using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.LoggedUser.Queries;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Infrastructure.Handlers.LoggedUser;

namespace TeamsAllocationManager.Tests.Handlers.Project;

[TestFixture]
public class GetIsUserAdminHandlerTests
{
	private readonly ApplicationDbContext _applicationDbContext;
	private readonly GetIsUserAdminHandler _handler;
	public GetIsUserAdminHandlerTests()
	{
		DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
						.UseInMemoryDatabase(databaseName: GetType().Name)
						.Options;
		_applicationDbContext = new ApplicationDbContext(options);

		_handler = new GetIsUserAdminHandler(new EmployeesRepository(_applicationDbContext));
	}

	[SetUp]
	public void SetupBeforeEachTest()
	{
		_applicationDbContext.ClearDatabase();

		var userRole = new UserRoleEntity
		{
			Role = RoleEntity.CreateRole(RoleEntity.Admin),
			Employee = new EmployeeEntity { Email = "abc@future-processing.com", UserLogin = "abc" }
		};

		_applicationDbContext.UserRoles.Add(userRole);
		_applicationDbContext.SaveChanges();
	}

	[Test]
	public async Task Should_Return_True()
	{
		// given
		var query = new GetIsUserAdminQuery("abc@future-processing.com");

		// when
		bool result = await _handler.HandleAsync(query);

		// then
		Assert.IsTrue(result);
	}

	[Test]
	public async Task Should_Return_False()
	{
		// given
		var query = new GetIsUserAdminQuery("nonadmin@future-processing.com");

		// when
		bool result = await _handler.HandleAsync(query);

		// then
		Assert.IsFalse(result);
	}

}
