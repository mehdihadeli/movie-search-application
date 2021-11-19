using System.Threading.Tasks;

namespace BuildingBlocks.Security.ApiKey
{
    public interface IGetApiKeyQuery
    {
        Task<ApiKey> Execute(string providedApiKey);
    }
}