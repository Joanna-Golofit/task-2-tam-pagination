using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.EmployeeDeskHistory.Commands;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Infrastructure.Handlers.EmployeeDeskHistory;

namespace TeamsAllocationManager.Tests.Handlers.EmployeeDeskHistory;

[TestFixture]
public class ClearOldEmployeeDeskHistoryRecordsHandlerTest
{
	private readonly ApplicationDbContext _context;
	private readonly EmployeeDeskHistoryRepository _employeeDeskHistoryRepository;

	public ClearOldEmployeeDeskHistoryRecordsHandlerTest()
	{
		DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: GetType().Name)
			.Options;
		_context = new ApplicationDbContext(options);
		_employeeDeskHistoryRepository = new EmployeeDeskHistoryRepository(_context);
	}

	[SetUp]
	public void SetupBeforeEachTest()
	{
		_context.ClearDatabase();

		var employeeDeskHistory1 = new EmployeeDeskHistoryEntity
		{
			To = new DateTime(2000, 01, 01)
		};
		var employeeDeskHistory2 = new EmployeeDeskHistoryEntity
		{
			To = DateTime.Now
		};

		_context.EmployeeDeskHistory.Add(employeeDeskHistory1);
		_context.EmployeeDeskHistory.Add(employeeDeskHistory2);

		_context.SaveChanges();
	}

	[Test]
	public async Task ShouldClearHistory()
	{
		// given
		int numberOfHistoryToClear = 1;
		int expectedInDatabase = _context.EmployeeDeskHistory.Count() - numberOfHistoryToClear;

		var command = new ClearOldEmployeeDeskHistoryRecordsCommand();

		var deletionDate = command.DeletionDate;
		var historyToDelete = _context.EmployeeDeskHistory.Select(d => d.Id).Take(numberOfHistoryToClear).ToList();

		var commandHandler = new ClearOldEmployeeDeskHistoryRecordsHandler(_employeeDeskHistoryRepository);

		// when
		bool result = await commandHandler.HandleAsync(command);

		// then
		Assert.IsTrue(result);
		Assert.AreEqual(expectedInDatabase, _context.EmployeeDeskHistory.Count());
		Assert.IsFalse(_context.EmployeeDeskHistory.Any(ewth => historyToDelete.Contains(ewth.Id)));
	}
}

