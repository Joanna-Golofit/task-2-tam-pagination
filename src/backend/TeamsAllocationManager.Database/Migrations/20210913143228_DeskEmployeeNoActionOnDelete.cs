using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class DeskEmployeeNoActionOnDelete : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Desks_Employees_EmployeeId",
            table: "Desks");

        migrationBuilder.DropIndex(
            name: "IX_Employees_Email",
            table: "Employees");

        migrationBuilder.DropIndex(
            name: "IX_Employees_UserLogin",
            table: "Employees");

        migrationBuilder.AlterColumn<string>(
            name: "UserLogin",
            table: "Employees",
            maxLength: 255,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(255)",
            oldMaxLength: 255,
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Email",
            table: "Employees",
            maxLength: 255,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(255)",
            oldMaxLength: 255,
            oldNullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Employees_Email",
            table: "Employees",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Employees_UserLogin",
            table: "Employees",
            column: "UserLogin",
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_Desks_Employees_EmployeeId",
            table: "Desks",
            column: "EmployeeId",
            principalTable: "Employees",
            principalColumn: "Id");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Desks_Employees_EmployeeId",
            table: "Desks");

        migrationBuilder.DropIndex(
            name: "IX_Employees_Email",
            table: "Employees");

        migrationBuilder.DropIndex(
            name: "IX_Employees_UserLogin",
            table: "Employees");

        migrationBuilder.AlterColumn<string>(
            name: "UserLogin",
            table: "Employees",
            type: "nvarchar(255)",
            maxLength: 255,
            nullable: true,
            oldClrType: typeof(string),
            oldMaxLength: 255);

        migrationBuilder.AlterColumn<string>(
            name: "Email",
            table: "Employees",
            type: "nvarchar(255)",
            maxLength: 255,
            nullable: true,
            oldClrType: typeof(string),
            oldMaxLength: 255);

        migrationBuilder.CreateIndex(
            name: "IX_Employees_Email",
            table: "Employees",
            column: "Email",
            unique: true,
            filter: "[Email] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_Employees_UserLogin",
            table: "Employees",
            column: "UserLogin",
            unique: true,
            filter: "[UserLogin] IS NOT NULL");

        migrationBuilder.AddForeignKey(
            name: "FK_Desks_Employees_EmployeeId",
            table: "Desks",
            column: "EmployeeId",
            principalTable: "Employees",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
