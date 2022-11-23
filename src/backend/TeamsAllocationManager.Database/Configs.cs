using System;
using System.Globalization;
using System.Threading;

namespace TeamsAllocationManager.Database;

public static class Configs
{
	public static decimal AreaMinLevelPerPerson
	{
		get
		{
			NumberFormatInfo nfi = Thread.CurrentThread.CurrentCulture.NumberFormat;
			return decimal.Parse(Environment.GetEnvironmentVariable("AreaMinLevelPerPerson", EnvironmentVariableTarget.Process)?
				.Replace(".", nfi.NumberDecimalSeparator)
				?? "4");
		}
	}

	public static string StorageConnectionString
	{
		get
		{
			return Environment.GetEnvironmentVariable("StorageConnectionString", EnvironmentVariableTarget.Process) ?? "";
		}
	}

	public static string StorageRoomPlansContainerName
	{
		get
		{
			return "room-plans";
		}
	}

	public static int RoomPlansSasTokenExpiryTime
	{
		get
		{
			return 30;
		}
	}

	public static int EfDatabaseConnectionMaxRetryCount
	{
		get
		{
			return int.Parse(Environment.GetEnvironmentVariable("EfDatabaseConnectionMaxRetryCount", EnvironmentVariableTarget.Process) ?? "5");
		}
	}

	public static int EfDatabaseConnectionMaxRetryDelay
	{
		get
		{
			return int.Parse(Environment.GetEnvironmentVariable("EfDatabaseConnectionMaxRetryDelay", EnvironmentVariableTarget.Process) ?? "15");
		}
	}

	public static string? RoomPlansAzureContainerSasToken { get; set; }
}
