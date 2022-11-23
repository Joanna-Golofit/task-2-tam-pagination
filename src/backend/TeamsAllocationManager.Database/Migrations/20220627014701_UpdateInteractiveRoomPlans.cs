using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace TeamsAllocationManager.Database.Migrations
{
    public partial class UpdateInteractiveRoomPlans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			//Updating F1 Building Room Plans

			var plan_f1_001 = Encoding.UTF8.GetString(SvgResources.SvgResources.F1_001, 0, SvgResources.SvgResources.F1_001.Length);
			var plan_f1_002 = Encoding.UTF8.GetString(SvgResources.SvgResources.F1_002, 0, SvgResources.SvgResources.F1_002.Length);
			var plan_f1_101 = Encoding.UTF8.GetString(SvgResources.SvgResources.F1_101, 0, SvgResources.SvgResources.F1_101.Length);
			var plan_f1_102 = Encoding.UTF8.GetString(SvgResources.SvgResources.F1_102, 0, SvgResources.SvgResources.F1_102.Length);
			var plan_f1_103 = Encoding.UTF8.GetString(SvgResources.SvgResources.F1_103, 0, SvgResources.SvgResources.F1_103.Length);
			var plan_f1_104 = Encoding.UTF8.GetString(SvgResources.SvgResources.F1_104, 0, SvgResources.SvgResources.F1_104.Length);
			var plan_f1_105 = Encoding.UTF8.GetString(SvgResources.SvgResources.F1_105, 0, SvgResources.SvgResources.F1_105.Length);
			var plan_f1_106 = Encoding.UTF8.GetString(SvgResources.SvgResources.F1_106, 0, SvgResources.SvgResources.F1_106.Length);
			var plan_f1_110 = Encoding.UTF8.GetString(SvgResources.SvgResources.F1_110, 0, SvgResources.SvgResources.F1_110.Length);
			var plan_f1_111 = Encoding.UTF8.GetString(SvgResources.SvgResources.F1_111, 0, SvgResources.SvgResources.F1_111.Length);

			migrationBuilder.Sql(
				@$"DECLARE @F1_Floor0_Id UNIQUEIDENTIFIER = 
				(SELECT F.[Id] FROM [dbo].[Floors] F
				JOIN [dbo].[Buildings] B 
				ON B.[Name] = 'F1' 
				WHERE F.[BuildingId] = B.[Id] AND FloorNumber = 0)
				IF @F1_Floor0_Id IS NOT NULL
				BEGIN
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f1_001}' WHERE [Name] = '001' AND [FloorId] = @F1_Floor0_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f1_002}' WHERE [Name] = '002' AND [FloorId] = @F1_Floor0_Id;
				END;

				DECLARE @F1_Floor1_Id UNIQUEIDENTIFIER = 
				(SELECT F.[Id] FROM [dbo].[Floors] F
				JOIN [dbo].[Buildings] B 
				ON B.[Name] = 'F1' 
				WHERE F.[BuildingId] = B.[Id] AND FloorNumber = 1)

				IF @F1_Floor1_Id IS NOT NULL
				BEGIN
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f1_101}' WHERE [Name] = '101' AND [FloorId] = @F1_Floor1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f1_102}' WHERE [Name] = '102' AND [FloorId] = @F1_Floor1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f1_103}' WHERE [Name] = '103' AND [FloorId] = @F1_Floor1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f1_104}' WHERE [Name] = '104' AND [FloorId] = @F1_Floor1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f1_105}' WHERE [Name] = '105' AND [FloorId] = @F1_Floor1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f1_106}' WHERE [Name] = '106' AND [FloorId] = @F1_Floor1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f1_110}' WHERE [Name] = '110' AND [FloorId] = @F1_Floor1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f1_111}' WHERE [Name] = '111' AND [FloorId] = @F1_Floor1_Id;
				END;");

			//Updating F2 Building Room Plans

			var plan_f2_102 = Encoding.UTF8.GetString(SvgResources.SvgResources.F2_102, 0, SvgResources.SvgResources.F2_102.Length);
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
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f2_102}' WHERE [Name] = '102' AND [FloorId] = @F2_Floor_1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f2_104_105}' WHERE [Name] = '104-105' AND [FloorId] = @F2_Floor_1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f2_106_107}' WHERE [Name] = '106-107' AND [FloorId] = @F2_Floor_1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f2_110_111}' WHERE [Name] = '110-111' AND [FloorId] = @F2_Floor_1_Id;
				END");

			//Updating F3 Building Room Plans

			var plan_f3_001 = Encoding.UTF8.GetString(SvgResources.SvgResources.F3_001, 0, SvgResources.SvgResources.F3_001.Length);
			var plan_f3_002 = Encoding.UTF8.GetString(SvgResources.SvgResources.F3_002, 0, SvgResources.SvgResources.F3_002.Length);
			var plan_f3_012 = Encoding.UTF8.GetString(SvgResources.SvgResources.F3_012, 0, SvgResources.SvgResources.F3_012.Length);
			var plan_f3_101 = Encoding.UTF8.GetString(SvgResources.SvgResources.F3_101, 0, SvgResources.SvgResources.F3_101.Length);
			var plan_f3_201 = Encoding.UTF8.GetString(SvgResources.SvgResources.F3_201, 0, SvgResources.SvgResources.F3_201.Length);
			var plan_f3_204 = Encoding.UTF8.GetString(SvgResources.SvgResources.F3_204, 0, SvgResources.SvgResources.F3_204.Length);

			migrationBuilder.Sql(
				$@"DECLARE @F3_Floor_0_Id UNIQUEIDENTIFIER = (SELECT F.[Id] FROM [dbo].[Floors] F
				JOIN [dbo].[Buildings] B 
				ON B.[Name] = 'F3' 
				WHERE F.[BuildingId] = B.[Id] AND FloorNumber = 0)

				IF @F3_Floor_0_Id IS NOT NULL
				BEGIN
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f3_001}' WHERE [Name] = '001' AND [FloorId] = @F3_Floor_0_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f3_002}' WHERE [Name] = '002' AND [FloorId] = @F3_Floor_0_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f3_012}' WHERE [Name] = '012' AND [FloorId] = @F3_Floor_0_Id;
				END;

				DECLARE @F3_Floor_1_Id UNIQUEIDENTIFIER = (SELECT F.[Id] FROM [dbo].[Floors] F
				JOIN [dbo].[Buildings] B 
				ON B.[Name] = 'F3' 
				WHERE F.[BuildingId] = B.[Id] AND FloorNumber = 1)

				IF @F3_Floor_1_Id IS NOT NULL
				BEGIN
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f3_101}' WHERE [Name] = '101' AND [FloorId] = @F3_Floor_1_Id;
				END;

				DECLARE @F3_Floor_2_Id UNIQUEIDENTIFIER = (SELECT F.[Id] FROM [dbo].[Floors] F
				JOIN [dbo].[Buildings] B 
				ON B.[Name] = 'F3' 
				WHERE F.[BuildingId] = B.[Id] AND FloorNumber = 2)

				IF @F3_Floor_2_Id IS NOT NULL
				BEGIN
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f3_201}' WHERE [Name] = '201' AND [FloorId] = @F3_Floor_2_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f3_204}' WHERE [Name] = '204' AND [FloorId] = @F3_Floor_2_Id;
				END;");

			//Updating F4 Building Room Plans

			var plan_f4_004 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_004, 0, SvgResources.SvgResources.F4_004.Length);
			var plan_f4_005 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_005, 0, SvgResources.SvgResources.F4_005.Length);
			var plan_f4_007 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_007, 0, SvgResources.SvgResources.F4_007.Length);
			var plan_f4_009 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_009, 0, SvgResources.SvgResources.F4_009.Length);
			var plan_f4_010 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_010, 0, SvgResources.SvgResources.F4_010.Length);
			var plan_f4_012 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_012, 0, SvgResources.SvgResources.F4_012.Length);
			var plan_f4_014 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_014, 0, SvgResources.SvgResources.F4_014.Length);
			var plan_f4_101 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_101, 0, SvgResources.SvgResources.F4_101.Length);
			var plan_f4_102 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_102, 0, SvgResources.SvgResources.F4_102.Length);
			var plan_f4_104 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_104, 0, SvgResources.SvgResources.F4_104.Length);
			var plan_f4_105 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_105, 0, SvgResources.SvgResources.F4_105.Length);
			var plan_f4_112 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_112, 0, SvgResources.SvgResources.F4_112.Length);
			var plan_f4_113 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_113, 0, SvgResources.SvgResources.F4_113.Length);
			var plan_f4_114 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_114, 0, SvgResources.SvgResources.F4_114.Length);
			var plan_f4_126 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_126, 0, SvgResources.SvgResources.F4_126.Length);
			var plan_f4_127 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_127, 0, SvgResources.SvgResources.F4_127.Length);
			var plan_f4_128 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_128, 0, SvgResources.SvgResources.F4_128.Length);
			var plan_f4_129 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_129, 0, SvgResources.SvgResources.F4_129.Length);
			var plan_f4_133 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_133, 0, SvgResources.SvgResources.F4_133.Length);
			var plan_f4_136 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_136, 0, SvgResources.SvgResources.F4_136.Length);
			var plan_f4_201 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_201, 0, SvgResources.SvgResources.F4_201.Length);
			var plan_f4_203 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_203, 0, SvgResources.SvgResources.F4_203.Length);
			var plan_f4_204 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_204, 0, SvgResources.SvgResources.F4_204.Length);
			var plan_f4_208 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_208, 0, SvgResources.SvgResources.F4_208.Length);
			var plan_f4_209 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_209, 0, SvgResources.SvgResources.F4_209.Length);
			var plan_f4_211 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_211, 0, SvgResources.SvgResources.F4_211.Length);
			var plan_f4_214 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_214, 0, SvgResources.SvgResources.F4_214.Length);
			var plan_f4_215 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4_215, 0, SvgResources.SvgResources.F4_215.Length);

			migrationBuilder.Sql(
				$@"DECLARE @F4_Floor_0_Id UNIQUEIDENTIFIER = (SELECT F.[Id] FROM [dbo].[Floors] F
				JOIN [dbo].[Buildings] B 
				ON B.[Name] = 'F4' 
				WHERE F.[BuildingId] = B.[Id] AND FloorNumber = 0)

				IF @F4_Floor_0_Id IS NOT NULL
				BEGIN
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_004}' WHERE [Name] = '004' AND [FloorId] = @F4_Floor_0_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_005}' WHERE [Name] = '005' AND [FloorId] = @F4_Floor_0_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_007}' WHERE [Name] = '007' AND [FloorId] = @F4_Floor_0_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_009}' WHERE [Name] = '009' AND [FloorId] = @F4_Floor_0_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_010}' WHERE [Name] = '010' AND [FloorId] = @F4_Floor_0_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_012}' WHERE [Name] = '012' AND [FloorId] = @F4_Floor_0_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_014}' WHERE [Name] = '014' AND [FloorId] = @F4_Floor_0_Id;
				END;

				DECLARE @F4_Floor_1_Id UNIQUEIDENTIFIER = (SELECT F.[Id] FROM [dbo].[Floors] F
				JOIN [dbo].[Buildings] B 
				ON B.[Name] = 'F4' 
				WHERE F.[BuildingId] = B.[Id] AND FloorNumber = 1)

				IF @F4_Floor_1_Id IS NOT NULL
				BEGIN
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_101}' WHERE [Name] = '101' AND [FloorId] = @F4_Floor_1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_102}' WHERE [Name] = '102' AND [FloorId] = @F4_Floor_1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_104}' WHERE [Name] = '104' AND [FloorId] = @F4_Floor_1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_105}' WHERE [Name] = '105' AND [FloorId] = @F4_Floor_1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_112}' WHERE [Name] = '112' AND [FloorId] = @F4_Floor_1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_113}' WHERE [Name] = '113' AND [FloorId] = @F4_Floor_1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_114}' WHERE [Name] = '114' AND [FloorId] = @F4_Floor_1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_126}' WHERE [Name] = '126' AND [FloorId] = @F4_Floor_1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_127}' WHERE [Name] = '127' AND [FloorId] = @F4_Floor_1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_128}' WHERE [Name] = '128' AND [FloorId] = @F4_Floor_1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_129}' WHERE [Name] = '129' AND [FloorId] = @F4_Floor_1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_133}' WHERE [Name] = '133' AND [FloorId] = @F4_Floor_1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_136}' WHERE [Name] = '136' AND [FloorId] = @F4_Floor_1_Id;
				END;

				DECLARE @F4_Floor_2_Id UNIQUEIDENTIFIER = (SELECT F.[Id] FROM [dbo].[Floors] F
				JOIN [dbo].[Buildings] B 
				ON B.[Name] = 'F4' 
				WHERE F.[BuildingId] = B.[Id] AND FloorNumber = 2)

				IF @F4_Floor_2_Id IS NOT NULL
				BEGIN
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_201}' WHERE [Name] = '201' AND [FloorId] = @F4_Floor_2_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_203}' WHERE [Name] = '203' AND [FloorId] = @F4_Floor_2_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_204}' WHERE [Name] = '204' AND [FloorId] = @F4_Floor_2_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_208}' WHERE [Name] = '208' AND [FloorId] = @F4_Floor_2_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_209}' WHERE [Name] = '209' AND [FloorId] = @F4_Floor_2_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_211}' WHERE [Name] = '211' AND [FloorId] = @F4_Floor_2_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_214}' WHERE [Name] = '214' AND [FloorId] = @F4_Floor_2_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4_215}' WHERE [Name] = '215' AND [FloorId] = @F4_Floor_2_Id;
				END;");

			//Updating F4C Building Room Plans

			var plan_f4c_016 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4C_016, 0, SvgResources.SvgResources.F4C_016.Length);
			var plan_f4c_017 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4C_017, 0, SvgResources.SvgResources.F4C_017.Length);
			var plan_f4c_020 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4C_020, 0, SvgResources.SvgResources.F4C_020.Length);
			var plan_f4c_022 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4C_022, 0, SvgResources.SvgResources.F4C_022.Length);
			var plan_f4c_023 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4C_023, 0, SvgResources.SvgResources.F4C_023.Length);
			var plan_f4c_116 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4C_116, 0, SvgResources.SvgResources.F4C_116.Length);
			var plan_f4c_117 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4C_117, 0, SvgResources.SvgResources.F4C_117.Length);
			var plan_f4c_118 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4C_118, 0, SvgResources.SvgResources.F4C_118.Length);
			var plan_f4c_121 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4C_121, 0, SvgResources.SvgResources.F4C_121.Length);
			var plan_f4c_123 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4C_123, 0, SvgResources.SvgResources.F4C_123.Length);
			var plan_f4c_217 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4C_217, 0, SvgResources.SvgResources.F4C_217.Length);
			var plan_f4c_218 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4C_218, 0, SvgResources.SvgResources.F4C_218.Length);
			var plan_f4c_220 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4C_220, 0, SvgResources.SvgResources.F4C_220.Length);
			var plan_f4c_221 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4C_221, 0, SvgResources.SvgResources.F4C_221.Length);
			var plan_f4c_222 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4C_222, 0, SvgResources.SvgResources.F4C_222.Length);
			var plan_f4c_225 = Encoding.UTF8.GetString(SvgResources.SvgResources.F4C_225, 0, SvgResources.SvgResources.F4C_225.Length);


			migrationBuilder.Sql(
				$@"DECLARE @F4C_Floor_0_Id UNIQUEIDENTIFIER = (SELECT F.[Id] FROM [dbo].[Floors] F
				JOIN [dbo].[Buildings] B 
				ON B.[Name] = 'F4C' 
				WHERE F.[BuildingId] = B.[Id] AND FloorNumber = 0)

				IF @F4C_Floor_0_Id IS NOT NULL
				BEGIN
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4c_016}' WHERE [Name] = '016' AND [FloorId] = @F4C_Floor_0_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4c_017}' WHERE [Name] = '017' AND [FloorId] = @F4C_Floor_0_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4c_020}' WHERE [Name] = '020' AND [FloorId] = @F4C_Floor_0_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4c_022}' WHERE [Name] = '022' AND [FloorId] = @F4C_Floor_0_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4c_023}' WHERE [Name] = '023' AND [FloorId] = @F4C_Floor_0_Id;
				END;

				DECLARE @F4C_Floor_1_Id UNIQUEIDENTIFIER = (SELECT F.[Id] FROM [dbo].[Floors] F
				JOIN [dbo].[Buildings] B 
				ON B.[Name] = 'F4C' 
				WHERE F.[BuildingId] = B.[Id] AND FloorNumber = 1)

				IF @F4C_Floor_1_Id IS NOT NULL
				BEGIN
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4c_116}' WHERE [Name] = '116' AND [FloorId] = @F4C_Floor_1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4c_117}' WHERE [Name] = '117' AND [FloorId] = @F4C_Floor_1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4c_118}' WHERE [Name] = '118' AND [FloorId] = @F4C_Floor_1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4c_121}' WHERE [Name] = '121' AND [FloorId] = @F4C_Floor_1_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4c_123}' WHERE [Name] = '123' AND [FloorId] = @F4C_Floor_1_Id;
				END;

				DECLARE @F4C_Floor_2_Id UNIQUEIDENTIFIER = (SELECT F.[Id] FROM [dbo].[Floors] F
				JOIN [dbo].[Buildings] B 
				ON B.[Name] = 'F4C' 
				WHERE F.[BuildingId] = B.[Id] AND FloorNumber = 2)

				IF @F4C_Floor_2_Id IS NOT NULL
				BEGIN
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4c_217}' WHERE [Name] = '217' AND [FloorId] = @F4C_Floor_2_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4c_218}' WHERE [Name] = '218' AND [FloorId] = @F4C_Floor_2_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4c_220}' WHERE [Name] = '220' AND [FloorId] = @F4C_Floor_2_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4c_221}' WHERE [Name] = '221' AND [FloorId] = @F4C_Floor_2_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4c_222}' WHERE [Name] = '222' AND [FloorId] = @F4C_Floor_2_Id;
					UPDATE [dbo].[Rooms] SET [RoomPlanInfo] = '{plan_f4c_225}' WHERE [Name] = '225' AND [FloorId] = @F4C_Floor_2_Id;
				END;");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
