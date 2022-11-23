using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class UserRoleAddEmployeeEntity : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
		migrationBuilder.UsePreDeploymentScript(this);

        migrationBuilder.DropIndex(
            name: "IX_UserRoles_RoleId_Username",
            table: "UserRoles");

        migrationBuilder.DropColumn(
            name: "Username",
            table: "UserRoles");

        migrationBuilder.DropColumn(
            name: "Type",
            table: "Employees");

        migrationBuilder.AddColumn<Guid>(
            name: "EmployeeId",
            table: "UserRoles",
            nullable: false);

        migrationBuilder.CreateIndex(
            name: "IX_UserRoles_EmployeeId",
            table: "UserRoles",
            column: "EmployeeId");

        migrationBuilder.CreateIndex(
            name: "IX_UserRoles_RoleId_EmployeeId",
            table: "UserRoles",
            columns: new[] { "RoleId", "EmployeeId" },
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_UserRoles_Employees_EmployeeId",
            table: "UserRoles",
            column: "EmployeeId",
            principalTable: "Employees",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_UserRoles_Employees_EmployeeId",
            table: "UserRoles");

        migrationBuilder.DropIndex(
            name: "IX_UserRoles_EmployeeId",
            table: "UserRoles");

        migrationBuilder.DropIndex(
            name: "IX_UserRoles_RoleId_EmployeeId",
            table: "UserRoles");

        migrationBuilder.DropColumn(
            name: "EmployeeId",
            table: "UserRoles");

        migrationBuilder.AddColumn<string>(
            name: "Username",
            table: "UserRoles",
            type: "nvarchar(450)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<int>(
            name: "Type",
            table: "Employees",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateIndex(
            name: "IX_UserRoles_RoleId_Username",
            table: "UserRoles",
            columns: new[] { "RoleId", "Username" },
            unique: true);
    }
}
