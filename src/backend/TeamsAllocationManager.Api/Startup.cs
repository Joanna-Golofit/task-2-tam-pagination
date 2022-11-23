using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
using TeamsAllocationManager.Contracts.Base;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Base.Queries;
using TeamsAllocationManager.Contracts.EntityQueries;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories.GenericRepository;
using TeamsAllocationManager.Infrastructure;
using TeamsAllocationManager.Infrastructure.Authentication;
using TeamsAllocationManager.Infrastructure.EntityQueries;
using TeamsAllocationManager.Infrastructure.Extensions;
using TeamsAllocationManager.Infrastructure.Options;
using TeamsAllocationManager.Infrastructure.Services;
using TeamsAllocationManager.Infrastructure.Services.Interfaces;
using TeamsAllocationManager.Integrations.FutureDatabase.Clients;
using TeamsAllocationManager.Mapper;

[assembly: FunctionsStartup(typeof(TeamsAllocationManager.Api.Startup))]
namespace TeamsAllocationManager.Api;

public class Startup : FunctionsStartup
{
	public override void Configure(IFunctionsHostBuilder builder)
	{
		IConfiguration config = builder.GetContext().Configuration;

		// Currently we handle only Polish localization on backend
		CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pl-PL");

		string? connectionString = config.GetConnectionString("SqlConnectionString");

		builder.Services.AddOptions<AzureAdSettings>().Bind(config.GetSection("AzureAd"));
		builder.Services.AddOptions<EnvironmentSettings>().Bind(config.GetSection("Environment"));
		builder.Services.AddOptions<AutoImportSettings>()
			    .Bind(config.GetSection(nameof(AutoImportSettings)))
			    .Configure(x =>
				    x.AutoAdminProjectExternalIds = config.GetSection($"{nameof(AutoImportSettings)}:{nameof(AutoImportSettings.AutoAdminProjectExternalIds)}").Value
				                            .DeserializeOrDefault<IEnumerable<int>>(new int[] { }));
		builder.Services.AddSingleton(AutoMapperConfiguration.Build());
		AzureAdSettings azureAdSettings = config.GetSection("AzureAd").Get<AzureAdSettings>();
		builder.Services.AddDbContext<ApplicationDbContext>(options =>
			SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString,
							opt => opt.EnableRetryOnFailure(
				Configs.EfDatabaseConnectionMaxRetryCount,
				TimeSpan.FromSeconds(Configs.EfDatabaseConnectionMaxRetryDelay),
				null)));

		builder.Services.AddAuthentication(options => { })
			// Workaround for production environment of Azure Functions
			// The runtime might be unreachable whe authentication scheme is being overriden.
			// See: https://github.com/AzureAD/microsoft-identity-web/issues/916
			.AddArmToken()
			.AddScriptAuthLevel()
			.AddMicrosoftIdentityWebApi(config)
			.EnableTokenAcquisitionToCallDownstreamApi()
				.AddMicrosoftGraph()
				.AddDownstreamWebApi("FutureDatabase", config.GetSection("FutureDatabaseApi"))
				.AddInMemoryTokenCaches();

		builder.Services.AddAuthorization(options =>
		{
			options.AddScriptPolicies();
		});

		builder.Services.AddAuthLevelAuthorizationHandler()
			.AddNamedAuthLevelAuthorizationHandler()
			.AddFunctionAuthorizationHandler();

		builder.Services.AddSingleton<IDispatcher, Dispatcher>();
		builder.Services.AddScoped(_ => new GraphServiceClient(new AuthenticationProvider(azureAdSettings)));
		builder.Services.AddScoped<IFutureDatabaseApiClient, FutureDatabaseApiClient>();
		builder.Services.AddScoped<IEntityQuery, RoomEntityQuery>();

		builder.Services.AddScoped<MailSenderService>();
		builder.Services.AddScoped<TestMailSenderService>();

		builder.Services.AddScoped<IMailSenderService>(provider =>
			(provider.GetService<IOptions<EnvironmentSettings>>()!.Value.EnvironmentType switch
			{
				EnvironmentType.Production => provider.GetService<MailSenderService>(),
				_ => provider.GetService<TestMailSenderService>(),
			})!
		);

		builder.Services.AddScoped(typeof(IMailComposer<>), typeof(MailComposer<>));
		builder.Services.AddMailFormatters();
		builder.Services.AddRepositories();

		var handlers = new Type[]
		{
			typeof(IAsyncQueryHandler<,>),
			typeof(IAsyncCommandHandler<,>),
			typeof(IAsyncCommandHandler<>),
			typeof(IQueryHandler<,>),
			typeof(ICommandHandler<,>),
			typeof(ICommandHandler<>),
		};
		builder.Services.AddHandlers(handlers);
	}
}
