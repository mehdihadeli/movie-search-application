using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Security.ApiKey;
using BuildingBlocks.Security.ApiKey.Authorization;

namespace MovieSearch.Infrastructure.Security
{
    public class InMemoryGetApiKeyQuery : IGetApiKeyQuery
    {
        private readonly IDictionary<string, ApiKey> _apiKeys;

        public InMemoryGetApiKeyQuery()
        {
            var existingApiKeys = new List<ApiKey>
            {
                new ApiKey(1, "Customer1", "C5BFF7F0-B4DF-475E-A331-F737424F013C", new DateTime(2021, 01, 01),
                    new List<string>
                    {
                        Roles.Customer,
                    }),
                new ApiKey(2, "Admin1", "5908D47C-85D3-4024-8C2B-6EC9464398AD", new DateTime(2021, 01, 01),
                    new List<string>
                    {
                        Roles.Admin,
                        Roles.Customer,
                        Roles.ThirdParty,
                    }),
                new ApiKey(3, "Third Party1", "06795D9D-A770-44B9-9B27-03C6ABDB1BAE", new DateTime(2021, 01, 01),
                    new List<string>
                    {
                        Roles.ThirdParty,
                    }),
            };

            _apiKeys = existingApiKeys.ToDictionary(x => x.Key, x => x);
        }

        public Task<ApiKey> ExecuteAsync(string providedApiKey)
        {
            _apiKeys.TryGetValue(providedApiKey, out var key);
            return Task.FromResult(key);
        }
    }
}