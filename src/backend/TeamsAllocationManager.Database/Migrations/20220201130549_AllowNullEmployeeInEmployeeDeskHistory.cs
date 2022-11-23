using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace TeamsAllocationManager.Database.Migrations;

public partial class AllowNullEmployeeInEmployeeDeskHistory : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<DateTime>(
			name: "From",
			table: "EmployeeDeskHistory",
			type: "datetime2",
			nullable: true,
			oldClrType: typeof(DateTime));
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<DateTime>(
			name: "From",
			table: "EmployeeDeskHistory",
			nullable: false,
			oldClrType: typeof(DateTime),
			oldType: "datetime2",
			oldNullable: true);
	}
}

