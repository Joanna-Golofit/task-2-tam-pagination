using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class EmployeeProjectsId : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_EmployeeProjects",
            table: "EmployeeProjects");

        migrationBuilder.AddColumn<Guid>(
            name: "Id",
            table: "EmployeeProjects",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<DateTime>(
            name: "Created",
            table: "EmployeeProjects",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<int>(
            name: "ExternalId",
            table: "EmployeeProjects",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<DateTime>(
            name: "Updated",
            table: "EmployeeProjects",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddPrimaryKey(
            name: "PK_EmployeeProjects",
            table: "EmployeeProjects",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_EmployeeProjects_EmployeeId",
            table: "EmployeeProjects",
            column: "EmployeeId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_EmployeeProjects",
            table: "EmployeeProjects");

        migrationBuilder.DropIndex(
            name: "IX_EmployeeProjects_EmployeeId",
            table: "EmployeeProjects");

        migrationBuilder.DropColumn(
            name: "Id",
            table: "EmployeeProjects");

        migrationBuilder.DropColumn(
            name: "Created",
            table: "EmployeeProjects");

        migrationBuilder.DropColumn(
            name: "ExternalId",
            table: "EmployeeProjects");

        migrationBuilder.DropColumn(
            name: "Updated",
            table: "EmployeeProjects");

        migrationBuilder.AddPrimaryKey(
            name: "PK_EmployeeProjects",
            table: "EmployeeProjects",
            columns: new[] { "EmployeeId", "ProjectId" });
    }
}
