using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Linq;
using TeamsAllocationManager.Database;

namespace TeamsAllocationManager.Infrastructure.Extensions;

public static class Helpers
{
	public static string GenerateSasTokenForRoomPlansContainer()
	{
		if (string.IsNullOrWhiteSpace(Configs.StorageConnectionString))
			return "";

		if (string.IsNullOrWhiteSpace(Configs.RoomPlansAzureContainerSasToken)
				|| ChcekIfSasTokenExpired(Configs.RoomPlansAzureContainerSasToken))
		{
			var storageAccount = CloudStorageAccount.Parse(Configs.StorageConnectionString);
			CloudBlobContainer container = storageAccount.CreateCloudBlobClient().GetContainerReference(Configs.StorageRoomPlansContainerName);

			var adHocPolicy = new SharedAccessBlobPolicy()
			{
				SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(Configs.RoomPlansSasTokenExpiryTime),
				Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write
			};

			string sasContainerToken = container.GetSharedAccessSignature(adHocPolicy, null);
			Configs.RoomPlansAzureContainerSasToken = sasContainerToken;
		}

		return Configs.RoomPlansAzureContainerSasToken;
	}

	private static bool ChcekIfSasTokenExpired(string sasToken)
	{
		try
		{
			string parsedExpiryTime = sasToken.Split('&').ToList().SingleOrDefault(x => x.StartsWith("se="))?.Replace("se=", "")?.Replace("%3A", ":") ?? "";

			return DateTimeOffset.Parse(parsedExpiryTime).UtcDateTime - DateTime.UtcNow < TimeSpan.FromMinutes(5);
		}
		catch
		{
			return true;
		}
	}

	/// <summary>
	/// Returns empty string for null object
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	public static string SerializeToJson(this object? obj)
	{
		if (obj == null)
			return "";

		return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
	}
}
