using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.EmployeeWorkingTypeHistory.Commands;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Infrastructure.Handlers.EmployeeWorkingTypeHistory;

namespace TeamsAllocationManager.Tests.Handlers.EmployeeWorkingTypeHistory;

[TestFixture]
public class ClearOldEmployeeWorkingTypeHistoryRecordsHandlerTest
{
	private readonly ApplicationDbContext _context;
	private readonly EmployeeWorkingTypeHistoryRepository _employeeWorkingTypeHistoryRepository;

	public ClearOldEmployeeWorkingTypeHistoryRecordsHandlerTest()
	{
		DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: GetType().Name)
			.Options;
		_context = new ApplicationDbContext(options);
		_employeeWorkingTypeHistoryRepository = new EmployeeWorkingTypeHistoryRepository(_context);
	}

	[SetUp]
	public void SetupBeforeEachTest()
	{
		_context.ClearDatabase();

		var employeeWorkingTypeHistory1 = new EmployeeWorkingTypeHistoryEntity
		{
			To = new DateTime(2000,01,01),
			EmployeeId = Guid.NewGuid()
		};
		var employeeWorkingTypeHistory2 = new EmployeeWorkingTypeHistoryEntity
		{
			To = DateTime.Now,
			EmployeeId = Guid.NewGuid()
		};

		_context.EmployeeWorkingTypeHistory.Add(employeeWorkingTypeHistory1);
		_context.EmployeeWorkingTypeHistory.Add(employeeWorkingTypeHistory2);

		_context.SaveChanges();
	}

	[Test]
	public async Task ShouldClearHistory()
	{
		// given
		int numberOfHistoryToClear = 1;
		int expectedInDatabase = _context.EmployeeWorkingTypeHistory.Count() - numberOfHistoryToClear;
			
		var command = new ClearOldEmployeeWorkingTypeHistoryRecordsCommand();

		var deletionDate = command.DeletionDate;
		var historyToDelete = _context.EmployeeDeskHistory.Select(d => d.Id).Take(numberOfHistoryToClear).ToList();

		var commandHandler = new ClearOldEmployeeWorkingTypeHistoryRecordsHandler(_employeeWorkingTypeHistoryRepository);

		// when
		bool result = await commandHandler.HandleAsync(command);

		// then
		Assert.IsTrue(result);
		Assert.AreEqual(expectedInDatabase, _context.EmployeeWorkingTypeHistory.Count());
		Assert.IsFalse(_context.EmployeeWorkingTypeHistory.Any(ewth => historyToDelete.Contains(ewth.Id)));
	}
}
	
