using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class RemoveUnnecessaryProperties : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "PeopleCount",
            table: "Projects");

        migrationBuilder.DropColumn(
            name: "PhoneNumber",
            table: "Employees");

        migrationBuilder.DropColumn(
            name: "PersonFullname",
            table: "Desks");

        migrationBuilder.AddColumn<Guid>(
            name: "EmployeeEntityId",
            table: "Desks",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Desks_EmployeeEntityId",
            table: "Desks",
            column: "EmployeeEntityId");

        migrationBuilder.AddForeignKey(
            name: "FK_Desks_Employees_EmployeeEntityId",
            table: "Desks",
            column: "EmployeeEntityId",
            principalTable: "Employees",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Desks_Employees_EmployeeEntityId",
            table: "Desks");

        migrationBuilder.DropIndex(
            name: "IX_Desks_EmployeeEntityId",
            table: "Desks");

        migrationBuilder.DropColumn(
            name: "EmployeeEntityId",
            table: "Desks");

        migrationBuilder.AddColumn<int>(
            name: "PeopleCount",
            table: "Projects",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<string>(
            name: "PhoneNumber",
            table: "Employees",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "PersonFullname",
            table: "Desks",
            type: "nvarchar(max)",
            nullable: true);
    }
}
