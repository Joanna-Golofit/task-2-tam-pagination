using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class updateHistoryEquipment : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_EmployeeEquipments_Employees_EmployeeEntityId",
            table: "EmployeeEquipments");

        migrationBuilder.DropIndex(
            name: "IX_EmployeeEquipments_EmployeeEntityId",
            table: "EmployeeEquipments");

        migrationBuilder.DropColumn(
            name: "EmployeeEntityId",
            table: "EmployeeEquipments");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "EmployeeEntityId",
            table: "EmployeeEquipments",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_EmployeeEquipments_EmployeeEntityId",
            table: "EmployeeEquipments",
            column: "EmployeeEntityId");

        migrationBuilder.AddForeignKey(
            name: "FK_EmployeeEquipments_Employees_EmployeeEntityId",
            table: "EmployeeEquipments",
            column: "EmployeeEntityId",
            principalTable: "Employees",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
