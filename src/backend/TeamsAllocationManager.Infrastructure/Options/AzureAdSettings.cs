namespace TeamsAllocationManager.Infrastructure.Options;

public class AzureAdSettings
{
	public string TenantId { get; set; } = null!;
	public string ClientId { get; set; } = null!;
	public string ClientSecret { get; set; } = null!;
	public string UserObjectId { get; set; } = null!;
}
