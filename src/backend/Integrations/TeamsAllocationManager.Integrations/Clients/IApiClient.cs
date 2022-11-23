using System.Threading.Tasks;

namespace TeamsAllocationManager.Integrations.Clients;

public interface IApiClient
{
	Task<TOutput?> GetAsync<TOutput>(string relativePath) where TOutput : class;
}
