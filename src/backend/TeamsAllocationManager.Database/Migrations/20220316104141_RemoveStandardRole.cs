using System;
using Microsoft.EntityFrameworkCore.Migrations;
using TeamsAllocationManager.Domain.Models;

namespace TeamsAllocationManager.Database.Migrations;

public partial class RemoveStandardRole : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DeleteData(
			table: "Roles",
			keyColumn: nameof(RoleEntity.Name),
			keyValue: "Standard");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.InsertData(
			table: "Roles",
			columns: new string[] { nameof(RoleEntity.Id), nameof(RoleEntity.Name), nameof(RoleEntity.Created), nameof(RoleEntity.Updated) },
			values: new object[] { Guid.NewGuid(), "Standard", DateTime.Now, DateTime.Now });
	}
}
