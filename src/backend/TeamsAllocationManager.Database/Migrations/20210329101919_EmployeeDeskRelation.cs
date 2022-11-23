using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class EmployeeDeskRelation : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
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
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "EmployeeEntityId",
            table: "Desks",
            type: "uniqueidentifier",
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
}
