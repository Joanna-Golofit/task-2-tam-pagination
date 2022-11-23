using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class AddEmployeeEquipment : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "EmployeeEquipments",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Created = table.Column<DateTime>(nullable: false),
                Updated = table.Column<DateTime>(nullable: false),
                EmployeeId = table.Column<Guid>(nullable: false),
                EquipmentId = table.Column<Guid>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EmployeeEquipments", x => x.Id);
                table.ForeignKey(
                    name: "FK_EmployeeEquipments_Employees_EmployeeId",
                    column: x => x.EmployeeId,
                    principalTable: "Employees",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_EmployeeEquipments_Equipments_EmployeeId",
                    column: x => x.EmployeeId,
                    principalTable: "Equipments",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_EmployeeEquipments_EmployeeId_EquipmentId",
            table: "EmployeeEquipments",
            columns: new[] { "EmployeeId", "EquipmentId" },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "EmployeeEquipments");
    }
}
