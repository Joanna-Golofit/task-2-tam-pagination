using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class ExternalIdType : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
		migrationBuilder.DropColumn(
			name: "ExternalId",
			table: "Projects");

		migrationBuilder.AddColumn<int>(
            name: "ExternalId",
            table: "Projects",
            nullable: false);

        migrationBuilder.DropColumn(
            name: "ExternalId",
            table: "Employees");

        migrationBuilder.AddColumn<int>(
            name: "ExternalId",
            table: "Employees",
            nullable: false);

        migrationBuilder.CreateIndex(
            name: "IX_Projects_ExternalId",
            table: "Projects",
            column: "ExternalId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Employees_ExternalId",
            table: "Employees",
            column: "ExternalId",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Projects_ExternalId",
            table: "Projects");

        migrationBuilder.DropIndex(
            name: "IX_Employees_ExternalId",
            table: "Employees");

		migrationBuilder.DropColumn(
			name: "ExternalId",
			table: "Projects");

		migrationBuilder.AddColumn<Guid>(
			name: "ExternalId",
			table: "Projects",
			type: "uniqueidentifier",
			nullable: false);

		migrationBuilder.DropColumn(
			name: "ExternalId",
			table: "Employees");

		migrationBuilder.AddColumn<Guid>(
			name: "ExternalId",
			table: "Employees",
			type: "uniqueidentifier",
			nullable: false);
    }
}
