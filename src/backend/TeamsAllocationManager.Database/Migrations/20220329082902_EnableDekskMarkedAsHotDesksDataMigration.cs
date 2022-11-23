using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class EnableDekskMarkedAsHotDesksDataMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
		migrationBuilder.Sql("UPDATE [dbo].[Desks] SET IsEnabled = 1 where IsHotDesk = 1");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
}
