using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using TeamsAllocationManager.Infrastructure.Options;

namespace TeamsAllocationManager.Infrastructure.Authentication;

public class AuthenticationProvider : IAuthenticationProvider
{
	private readonly AzureAdSettings _adSettings;

	public AuthenticationProvider(AzureAdSettings adSettings)
	{
		_adSettings = adSettings;
	}

	public async Task AuthenticateRequestAsync(HttpRequestMessage request)
	{
		var clientApplication = ConfidentialClientApplicationBuilder.Create(_adSettings.ClientId)
			                                                        .WithClientSecret(_adSettings.ClientSecret)
			                                                        .WithClientId(_adSettings.ClientId)
			                                                        .WithTenantId(_adSettings.TenantId)
			                                                        .Build();

		var result = await clientApplication.AcquireTokenForClient(new[] { ".default" }).ExecuteAsync();

		request.Headers.Add("Authorization", result.CreateAuthorizationHeader());
	}
}
