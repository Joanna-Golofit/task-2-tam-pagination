using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TeamsAllocationManager.Database;

namespace TeamsAllocationManager.LocalDbSeeder;

internal class Program
{
	public static void LoadConfiguration(HostBuilderContext host, IConfigurationBuilder builder)
	{
		builder
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
	}

	private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
	{
		services
			.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseSqlServer(host.Configuration.GetConnectionString("SqlConnectionString"));
			}, ServiceLifetime.Singleton);
	}

	private static IHostBuilder CreateHostBuilder(string[] args) =>
		Host.CreateDefaultBuilder(args)
			.ConfigureAppConfiguration(LoadConfiguration)
			.ConfigureServices(ConfigureServices);

	private static void Main(string[] args)
	{
		var host = CreateHostBuilder(args).Build();
		ApiDataSeeder.EnsureSeed(host.Services, ImportBuildingsRoomsDesksFromCSV.Execute());
		host.Run();
	}
}
