using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class repairHistory : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_EmployeeEquipments_Equipments_EquipmentEntityId",
            table: "EmployeeEquipments");

        migrationBuilder.DropIndex(
            name: "IX_EmployeeEquipments_EquipmentEntityId",
            table: "EmployeeEquipments");

        migrationBuilder.DropColumn(
            name: "EquipmentEntityId",
            table: "EmployeeEquipments");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "EquipmentEntityId",
            table: "EmployeeEquipments",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_EmployeeEquipments_EquipmentEntityId",
            table: "EmployeeEquipments",
            column: "EquipmentEntityId");

        migrationBuilder.AddForeignKey(
            name: "FK_EmployeeEquipments_Equipments_EquipmentEntityId",
            table: "EmployeeEquipments",
            column: "EquipmentEntityId",
            principalTable: "Equipments",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
