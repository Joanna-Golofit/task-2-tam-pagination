using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base;
using TeamsAllocationManager.Contracts.Import.Commands;
using TeamsAllocationManager.Dtos.Import;

namespace TeamsAllocationManager.Api.Functions;

public class ImportFunction : FunctionBase
{
	public ImportFunction(IDispatcher dispatcher) : base(dispatcher) { }

	[FunctionName("ImportFunction")]
	public override async Task<IActionResult> RunAsync(
		[HttpTrigger(AuthorizationLevel.Anonymous, Route = "Import/{*path}")]
		HttpRequest req, string? path, ILogger log)
		=> await base.RunAsync(req, path, log);

	[HttpPost]
	public async Task<ImportReportDto> ImportProjectsAndEmployees()
	{
		var command = new ImportProjectsAndEmployeesCommand(CurrentUsername);
		return await _dispatcher.DispatchAsync<ImportProjectsAndEmployeesCommand, ImportReportDto>(command);
	}

	[FunctionName("AutoImport")]
	public async Task AutoImport([TimerTrigger("0 0 3 * * *")] TimerInfo timer, ILogger logger)
	{
		using (logger.BeginScope(new Dictionary<string, object>
		{
			["AppVersion"] = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "-",
			["Action"] = "AutoImport",
		}))
		{
			try
			{
				logger.LogInformation($"Auto Import action started");

				var command = new ImportProjectsAndEmployeesCommand();
				await _dispatcher.DispatchAsync<ImportProjectsAndEmployeesCommand, ImportReportDto>(command);
			}
			catch (Exception ex)
			{
				LogException(logger, ex);
				throw;
			}
		}
	}
}

