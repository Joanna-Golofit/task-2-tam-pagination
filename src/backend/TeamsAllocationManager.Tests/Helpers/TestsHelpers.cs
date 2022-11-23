using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TeamsAllocationManager.Database;

namespace TeamsAllocationManager.Tests.Helpers;

internal static class TestsHelpers
{
	public static ApplicationDbContext CreateDbContextInMemory(object testClassIntance)
	{
		DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: testClassIntance.GetType().Name)
			.Options;
		return new ApplicationDbContext(options);
	}
}
