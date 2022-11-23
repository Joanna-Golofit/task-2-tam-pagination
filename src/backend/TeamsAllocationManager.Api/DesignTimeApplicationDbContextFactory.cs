using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TeamsAllocationManager.Database;

namespace TeamsAllocationManager.Api;

public class DesignTimeApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
	public ApplicationDbContext CreateDbContext(string[] args)
	{
		IConfigurationRoot config = new ConfigurationBuilder()
			.AddJsonFile("local.settings.json", true)
			.AddEnvironmentVariables()
			.Build();

		var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
		optionsBuilder.UseSqlServer(config.GetConnectionString("SqlConnectionString"));

		return new ApplicationDbContext(optionsBuilder.Options);
	}
}
