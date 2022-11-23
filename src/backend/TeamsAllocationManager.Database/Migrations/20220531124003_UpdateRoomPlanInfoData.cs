

#nullable disable

using System;
using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;
namespace TeamsAllocationManager.Database.Migrations
{
    public partial class UpdateRoomPlanInfoData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        var plan_001 = Encoding.UTF8.GetString(SvgResources.SvgResources._001, 0, SvgResources.SvgResources._001.Length);
	        var plan_002 = Encoding.UTF8.GetString(SvgResources.SvgResources._002, 0, SvgResources.SvgResources._002.Length);
	        var plan_003 = Encoding.UTF8.GetString(SvgResources.SvgResources._003, 0, SvgResources.SvgResources._003.Length);
	        var plan_004 = Encoding.UTF8.GetString(SvgResources.SvgResources._004, 0, SvgResources.SvgResources._004.Length);
	        var plan_005 = Encoding.UTF8.GetString(SvgResources.SvgResources._005, 0, SvgResources.SvgResources._005.Length);
	        var plan_101 = Encoding.UTF8.GetString(SvgResources.SvgResources._101, 0, SvgResources.SvgResources._101.Length);
	        var plan_102 = Encoding.UTF8.GetString(SvgResources.SvgResources._102, 0, SvgResources.SvgResources._102.Length);
	        var plan_103 = Encoding.UTF8.GetString(SvgResources.SvgResources._103, 0, SvgResources.SvgResources._103.Length);
	        var plan_104 = Encoding.UTF8.GetString(SvgResources.SvgResources._104, 0, SvgResources.SvgResources._104.Length);
	        var plan_105 = Encoding.UTF8.GetString(SvgResources.SvgResources._105, 0, SvgResources.SvgResources._105.Length);
	        var plan_106 = Encoding.UTF8.GetString(SvgResources.SvgResources._106, 0, SvgResources.SvgResources._106.Length);
	        var plan_110 = Encoding.UTF8.GetString(SvgResources.SvgResources._110, 0, SvgResources.SvgResources._110.Length);
	        var plan_111 = Encoding.UTF8.GetString(SvgResources.SvgResources._111, 0, SvgResources.SvgResources._111.Length);

			migrationBuilder.Sql(
				$"UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_001}' WHERE Name = '001';" +
				$"UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_002}' WHERE Name = '002';" +
		        $"UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_003}' WHERE Name = '003';" +
				$"UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_004}' WHERE Name = '004';" +
				$"UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_005}' WHERE Name = '005';" +
				$"UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_101}' WHERE Name = '101';" +
				$"UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_102}' WHERE Name = '102';" +
				$"UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_103}' WHERE Name = '103';" +
				$"UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_104}' WHERE Name = '104';" +
				$"UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_105}' WHERE Name = '105';" +
				$"UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_106}' WHERE Name = '106';" +
				$"UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_110}' WHERE Name = '110';" +
				$"UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_111}' WHERE Name = '111';"
			);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
