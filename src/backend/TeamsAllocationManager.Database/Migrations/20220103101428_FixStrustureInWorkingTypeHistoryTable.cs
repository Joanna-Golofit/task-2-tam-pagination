using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class FixStrustureInWorkingTypeHistoryTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<Guid>(
            name: "EmployeeId",
            table: "EmployeeWorkingTypeHistory",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<Guid>(
            name: "EmployeeId",
            table: "EmployeeWorkingTypeHistory",
            type: "uniqueidentifier",
            nullable: false,
            oldClrType: typeof(Guid),
            oldNullable: true);
    }
}
