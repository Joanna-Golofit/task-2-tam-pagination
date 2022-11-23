using Microsoft.Identity.Web;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using TeamsAllocationManager.Integrations.Exceptions;

namespace TeamsAllocationManager.Integrations.Clients;

public abstract class ApiClient : IApiClient
{
	private readonly IDownstreamWebApi _downstreamWebApi;
	private readonly string _serviceName;

	public ApiClient(IDownstreamWebApi downstreamWebApi, string serviceName)
	{
		_downstreamWebApi = downstreamWebApi;
		_serviceName = serviceName;
	}

	public async Task<TOutput?> GetAsync<TOutput>(string relativePath) where TOutput : class
	{
		HttpResponseMessage response = await _downstreamWebApi.CallWebApiForAppAsync(
			_serviceName,
			options =>
			{
				options.RelativePath = relativePath;
			});

		if (response.IsSuccessStatusCode)
		{
			string responseString = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<TOutput?>(responseString);
		}

		throw new ApiClientException($"An unexpected error occurred calling external API. service:{_serviceName}; code:{response.StatusCode}; message (optional):{response.ReasonPhrase}");
	}
}
