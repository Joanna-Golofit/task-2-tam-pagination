using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TeamsAllocationManager.Database;
using TeamsAllocationManager.Database.Repositories.GenericRepository;
using TeamsAllocationManager.Infrastructure.Services.EmailFormatters;

namespace TeamsAllocationManager.Api;

public static class HandlersHelper
{
	public static void AddHandlers(this IServiceCollection services, params Type[] handlerInterfaces)
	{
		IEnumerable<Type> handlers = AppDomain.CurrentDomain.GetAssemblies()
						.SelectMany(s => s.GetTypes())
						.Where(t => t.GetInterfaces()
										.Any(i => i.IsGenericType &&
											handlerInterfaces.Any(h => h == i.GetGenericTypeDefinition())));

		foreach (Type handler in handlers)
		{
			services.AddScoped(handler.GetInterfaces().First(i => i.IsGenericType &&
				handlerInterfaces.Any(h => h == i.GetGenericTypeDefinition())), handler);
		}
	}

	public static void AddMailFormatters(this IServiceCollection services)
	{
		var formatters = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(s => s.GetTypes())
			.Where(t =>
				!t.IsAbstract &&
				t.IsClass &&
				t.GetInterface(typeof(IMailMessageFormatter<>).Name)?.GetGenericTypeDefinition() == typeof(IMailMessageFormatter<>));

		foreach (var formatter in formatters)
		{
			services.Add(new ServiceDescriptor(formatter.GetInterface(typeof(IMailMessageFormatter<>).Name)!, formatter, ServiceLifetime.Scoped));
		}
	}

	public static void AddRepositories(this IServiceCollection services)
	{
		var repositories = Assembly.GetAssembly(typeof(ApplicationDbContext))?
			.GetTypes()
			.Where(t => t.IsClass && t.Name.Contains("Repository") && !t.Name.Contains("RepositoryBase")).ToList();

		if (repositories == null)
		{
			throw new ApplicationException("Repositories not found.");
		}

		var typesList = repositories.Select(r => new { Repository = r, Types = r.GetInterfaces() }).ToList();

		foreach (var type in typesList)
		{
			foreach (var repositoryType in type.Types)
			{
				services.Add(new ServiceDescriptor(repositoryType, type.Repository, ServiceLifetime.Scoped));
			}
		}
	}
}
