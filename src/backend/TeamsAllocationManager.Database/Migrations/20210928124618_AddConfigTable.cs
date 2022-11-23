using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class AddConfigTable : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "Configs",
			columns: table => new
			{
				Id = table.Column<Guid>(nullable: false),
				Created = table.Column<DateTime>(nullable: false),
				Updated = table.Column<DateTime>(nullable: false),
				Key = table.Column<string>(maxLength: 30, nullable: false),
				Data = table.Column<string>(nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_Configs", x => x.Id);
			});

		migrationBuilder.CreateIndex(
			name: "IX_Configs_Key",
			table: "Configs",
			column: "Key",
			unique: true);

		migrationBuilder.UsePostDeploymentScript(this);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "Configs");
	}
}
