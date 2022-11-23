using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class EquipmentHistory : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_EmployeeEquipments_Equipments_EmployeeId",
            table: "EmployeeEquipments");

        migrationBuilder.AddColumn<int>(
            name: "Count",
            table: "Equipments",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<Guid>(
            name: "EmployeeEntityId",
            table: "EmployeeEquipments",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "EquipmentEntityId",
            table: "EmployeeEquipments",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "EmployeeEquipmentHistory",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Created = table.Column<DateTime>(nullable: false),
                Updated = table.Column<DateTime>(nullable: false),
                EmployeeId = table.Column<Guid>(nullable: true),
                From = table.Column<DateTime>(nullable: false),
                To = table.Column<DateTime>(nullable: true),
                EquipmentId = table.Column<Guid>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EmployeeEquipmentHistory", x => x.Id);
                table.ForeignKey(
                    name: "FK_EmployeeEquipmentHistory_Employees_EmployeeId",
                    column: x => x.EmployeeId,
                    principalTable: "Employees",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_EmployeeEquipmentHistory_Equipments_EquipmentId",
                    column: x => x.EquipmentId,
                    principalTable: "Equipments",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_EmployeeEquipments_EmployeeEntityId",
            table: "EmployeeEquipments",
            column: "EmployeeEntityId");

        migrationBuilder.CreateIndex(
            name: "IX_EmployeeEquipments_EquipmentEntityId",
            table: "EmployeeEquipments",
            column: "EquipmentEntityId");

        migrationBuilder.CreateIndex(
            name: "IX_EmployeeEquipments_EquipmentId",
            table: "EmployeeEquipments",
            column: "EquipmentId");

        migrationBuilder.CreateIndex(
            name: "IX_EmployeeEquipmentHistory_EmployeeId",
            table: "EmployeeEquipmentHistory",
            column: "EmployeeId");

        migrationBuilder.CreateIndex(
            name: "IX_EmployeeEquipmentHistory_EquipmentId",
            table: "EmployeeEquipmentHistory",
            column: "EquipmentId");

        migrationBuilder.AddForeignKey(
            name: "FK_EmployeeEquipments_Employees_EmployeeEntityId",
            table: "EmployeeEquipments",
            column: "EmployeeEntityId",
            principalTable: "Employees",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_EmployeeEquipments_Equipments_EquipmentEntityId",
            table: "EmployeeEquipments",
            column: "EquipmentEntityId",
            principalTable: "Equipments",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_EmployeeEquipments_Equipments_EquipmentId",
            table: "EmployeeEquipments",
            column: "EquipmentId",
            principalTable: "Equipments",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_EmployeeEquipments_Employees_EmployeeEntityId",
            table: "EmployeeEquipments");

        migrationBuilder.DropForeignKey(
            name: "FK_EmployeeEquipments_Equipments_EquipmentEntityId",
            table: "EmployeeEquipments");

        migrationBuilder.DropForeignKey(
            name: "FK_EmployeeEquipments_Equipments_EquipmentId",
            table: "EmployeeEquipments");

        migrationBuilder.DropTable(
            name: "EmployeeEquipmentHistory");

        migrationBuilder.DropIndex(
            name: "IX_EmployeeEquipments_EmployeeEntityId",
            table: "EmployeeEquipments");

        migrationBuilder.DropIndex(
            name: "IX_EmployeeEquipments_EquipmentEntityId",
            table: "EmployeeEquipments");

        migrationBuilder.DropIndex(
            name: "IX_EmployeeEquipments_EquipmentId",
            table: "EmployeeEquipments");

        migrationBuilder.DropColumn(
            name: "Count",
            table: "Equipments");

        migrationBuilder.DropColumn(
            name: "EmployeeEntityId",
            table: "EmployeeEquipments");

        migrationBuilder.DropColumn(
            name: "EquipmentEntityId",
            table: "EmployeeEquipments");

        migrationBuilder.AddForeignKey(
            name: "FK_EmployeeEquipments_Equipments_EmployeeId",
            table: "EmployeeEquipments",
            column: "EmployeeId",
            principalTable: "Equipments",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
