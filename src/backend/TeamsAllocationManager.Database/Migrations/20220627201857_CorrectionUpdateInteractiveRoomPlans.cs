using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace TeamsAllocationManager.Database.Migrations
{
    public partial class CorrectionUpdateInteractiveRoomPlans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			//Fixing bug, Room F3 012 was incorrectly setup on Floor 1, setting up to Floor 0.

			migrationBuilder.Sql(
				$@"DECLARE @NewFloorId UNIQUEIDENTIFIER = (SELECT F.Id FROM dbo.Floors F
				JOIN dbo.Buildings B
				ON B.Name = 'F3' AND F.FloorNumber = '0'
				WHERE B.Id = F.BuildingId)

				DECLARE @OldFloorId UNIQUEIDENTIFIER = (SELECT F.Id FROM dbo.Floors F
				JOIN dbo.Buildings B
				ON B.Name = 'F3' AND F.FloorNumber = '1'
				WHERE B.Id = F.BuildingId)

				UPDATE [dbo].[Rooms] SET FloorId = @NewFloorId WHERE FloorId = @OldFloorId AND [Name] = '012'");

			//Updating svg for F3_012 once bug is fixed.

			var plan_f3_012 = Encoding.UTF8.GetString(SvgResources.SvgResources.F3_012, 0, SvgResources.SvgResources.F3_012.Length);

			migrationBuilder.Sql(
				$@"DECLARE @F3_Floor_0_Id UNIQUEIDENTIFIER = (SELECT F.[Id] FROM [dbo].[Floors] F
				JOIN [dbo].[Buildings] B 
				ON B.[Name] = 'F3' 
				WHERE F.[BuildingId] = B.[Id] AND FloorNumber = 0)

				IF @F3_Floor_0_Id IS NOT NULL
				BEGIN
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f3_012}' WHERE [Name] = '012' AND [FloorId] = @F3_Floor_0_Id;
				END;");

			//Correcting previous F1 Building Room Plans Update

			var plan_f1_103 = Encoding.UTF8.GetString(SvgResources.SvgResources.F1_103, 0, SvgResources.SvgResources.F1_103.Length);

			migrationBuilder.Sql(
				@$"DECLARE @F1_Floor1_Id UNIQUEIDENTIFIER = 
				(SELECT F.[Id] FROM [dbo].[Floors] F
				JOIN [dbo].[Buildings] B 
				ON B.[Name] = 'F1' 
				WHERE F.[BuildingId] = B.[Id] AND FloorNumber = 1)

				IF @F1_Floor1_Id IS NOT NULL
				BEGIN
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f1_103}' WHERE [Name] = '103' AND [FloorId] = @F1_Floor1_Id;
				END;");

			//Correcting previous F2 Building Room Plans Update

			var plan_f2_104_105 = Encoding.UTF8.GetString(SvgResources.SvgResources.F2_104_105, 0, SvgResources.SvgResources.F2_104_105.Length);
			var plan_f2_106_107 = Encoding.UTF8.GetString(SvgResources.SvgResources.F2_106_107, 0, SvgResources.SvgResources.F2_106_107.Length);
			var plan_f2_110_111 = Encoding.UTF8.GetString(SvgResources.SvgResources.F2_110_111, 0, SvgResources.SvgResources.F2_110_111.Length);

			migrationBuilder.Sql(
				$@"DECLARE @F2_Floor_1_Id UNIQUEIDENTIFIER =
				(SELECT F.[Id] FROM [dbo].[Floors] F
				JOIN [dbo].[Buildings] B 
				ON B.[Name] = 'F2' 
				WHERE F.[BuildingId] = B.[Id] AND FloorNumber = 1)

				IF @F2_Floor_1_Id IS NOT NULL
				BEGIN
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f2_104_105}' WHERE [Name] = '104 - 105' AND [FloorId] = @F2_Floor_1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f2_106_107}' WHERE [Name] = '106 - 107' AND [FloorId] = @F2_Floor_1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f2_110_111}' WHERE [Name] = '110 - 111' AND [FloorId] = @F2_Floor_1_Id;
				END");

			//Correcting previous F4 Building Room Plans

			var plan_f4_reception = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_Reception, 0, SvgResources.SvgResources.F4_Reception.Length);
			var plan_f4_104 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_104, 0, SvgResources.SvgResources.F4_104.Length);
			var plan_f4_105 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_105, 0, SvgResources.SvgResources.F4_105.Length);
			var plan_f4_204 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_204, 0, SvgResources.SvgResources.F4_204.Length);

			migrationBuilder.Sql(
				$@"
				DECLARE @F4_Floor_0_Id UNIQUEIDENTIFIER = (SELECT F.[Id] FROM [dbo].[Floors] F
				JOIN [dbo].[Buildings] B 
				ON B.[Name] = 'F4' 
				WHERE F.[BuildingId] = B.[Id] AND FloorNumber = 0)

				IF @F4_Floor_0_Id IS NOT NULL
				BEGIN
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_reception}' WHERE [Name] = 'Recepcja' AND [FloorId] = @F4_Floor_0_Id;
				END;

				DECLARE @F4_Floor_1_Id UNIQUEIDENTIFIER = (SELECT F.[Id] FROM [dbo].[Floors] F
				JOIN [dbo].[Buildings] B 
				ON B.[Name] = 'F4' 
				WHERE F.[BuildingId] = B.[Id] AND FloorNumber = 1)

				IF @F4_Floor_1_Id IS NOT NULL
				BEGIN
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_104}' WHERE [Name] = '104' AND [FloorId] = @F4_Floor_1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_105}' WHERE [Name] = '105' AND [FloorId] = @F4_Floor_1_Id
				END;

				DECLARE @F4_Floor_2_Id UNIQUEIDENTIFIER = (SELECT F.[Id] FROM [dbo].[Floors] F
				JOIN [dbo].[Buildings] B 
				ON B.[Name] = 'F4' 
				WHERE F.[BuildingId] = B.[Id] AND FloorNumber = 2)

				IF @F4_Floor_2_Id IS NOT NULL
				BEGIN
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_204}' WHERE [Name] = '204' AND [FloorId] = @F4_Floor_2_Id;
				END;");

			//Correcting previous F4C Building Room Plans

			var plan_f4c_018 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4C_018, 0, SvgResources.SvgResources.F4C_018.Length);
			var plan_f4c_224 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4C_224, 0, SvgResources.SvgResources.F4C_224.Length);
			var plan_f4c_225 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4C_225, 0, SvgResources.SvgResources.F4C_225.Length);

			migrationBuilder.Sql(
				@$"DECLARE @F4C_Floor_0_Id UNIQUEIDENTIFIER = (SELECT F.[Id] FROM [dbo].[Floors] F
				JOIN [dbo].[Buildings] B 
				ON B.[Name] = 'F4C' 
				WHERE F.[BuildingId] = B.[Id] AND FloorNumber = 0)

				IF @F4C_Floor_0_Id IS NOT NULL
				BEGIN
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4c_018}' WHERE [Name] = '016' AND [FloorId] = @F4C_Floor_0_Id;
				END;

				DECLARE @F4C_Floor_2_Id UNIQUEIDENTIFIER = (SELECT F.[Id] FROM [dbo].[Floors] F
				JOIN [dbo].[Buildings] B 
				ON B.[Name] = 'F4C' 
				WHERE F.[BuildingId] = B.[Id] AND FloorNumber = 2)

				IF @F4C_Floor_2_Id IS NOT NULL
				BEGIN
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4c_224}' WHERE [Name] = '224' AND [FloorId] = @F4C_Floor_2_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4c_225}' WHERE [Name] = '225' AND [FloorId] = @F4C_Floor_2_Id;
				END;");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
