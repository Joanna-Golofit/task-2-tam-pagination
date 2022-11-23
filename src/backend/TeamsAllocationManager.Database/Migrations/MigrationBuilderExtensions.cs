using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TeamsAllocationManager.Database.Migrations;

internal static class MigrationBuilderExtensions
{
	public static void UsePreDeploymentScript(this MigrationBuilder builder, Migration currentMigration)
	{
		string sqlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
			?? string.Empty, @"..\..\..\..\..\..\deployment\database\pre-deployment-scripts", currentMigration.GetType().Name + ".sql");
		builder.Sql($@"EXEC ('{File.ReadAllText(sqlFile).Replace("'", "''", StringComparison.OrdinalIgnoreCase)}')");
	}

	public static void UsePostDeploymentScript(this MigrationBuilder builder, Migration currentMigration)
	{
		string sqlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
			?? string.Empty, @"..\..\..\..\..\..\deployment\database\post-deployment-scripts", currentMigration.GetType().Name + ".sql");
		builder.Sql($@"EXEC ('{File.ReadAllText(sqlFile).Replace("'", "''", StringComparison.OrdinalIgnoreCase)}')");
	}
}
