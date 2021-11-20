using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BuildingBlocks.Mongo;
using BuildingBlocks.Test.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace Trill.Shared.Tests.Integration
{
    public abstract class WebApiTestFixture<TEntryPoint, TDbContext> : IntegrationTestFixture<TEntryPoint, TDbContext>
        where TEntryPoint : class
        where TDbContext : class, IMongoDbContext
    {
        private string _route;

        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        };

        protected void SetPath(string route)
        {
            if (string.IsNullOrWhiteSpace(route))
            {
                _route = string.Empty;
                return;
            }

            if (route.StartsWith("/"))
            {
                route = route.Substring(1, route.Length - 1);
            }

            if (route.EndsWith("/"))
            {
                route = route.Substring(0, route.Length - 1);
            }

            _route = $"{route}/";
        }

        protected static T Map<T>(object data) =>
            JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(data, SerializerOptions), SerializerOptions);

        protected Task<HttpResponseMessage> GetAsync(string endpoint)
            => Client.GetAsync(GetEndpoint(endpoint));

        protected async Task<T> GetAsync<T>(string endpoint)
            => await ReadAsync<T>(await GetAsync(endpoint));

        protected Task<HttpResponseMessage> PostAsync<T>(string endpoint, T command)
            => Client.PostAsync(GetEndpoint(endpoint), GetPayload(command));

        protected Task<HttpResponseMessage> PutAsync<T>(string endpoint, T command)
            => Client.PutAsync(GetEndpoint(endpoint), GetPayload(command));

        protected Task<HttpResponseMessage> DeleteAsync(string endpoint)
            => Client.DeleteAsync(GetEndpoint(endpoint));

        protected Task<HttpResponseMessage> SendAsync(string method, string endpoint)
            => SendAsync(GetMethod(method), endpoint);

        protected Task<HttpResponseMessage> SendAsync(HttpMethod method, string endpoint)
            => Client.SendAsync(new HttpRequestMessage(method, GetEndpoint(endpoint)));

        private static HttpMethod GetMethod(string method)
            => method.ToUpperInvariant() switch
            {
                "GET" => HttpMethod.Get,
                "POST" => HttpMethod.Post,
                "PUT" => HttpMethod.Put,
                "DELETE" => HttpMethod.Delete,
                _ => null
            };

        private string GetEndpoint(string endpoint) => $"{_route}{endpoint}";

        private static StringContent GetPayload(object value)
            => new(JsonSerializer.Serialize(value, SerializerOptions), Encoding.UTF8, "application/json");

        protected static async Task<T> ReadAsync<T>(HttpResponseMessage response)
            => JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), SerializerOptions);

        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }
    }

    public abstract class WebApiTestFixture<TEntryPoint> : IntegrationTestFixture<TEntryPoint> where TEntryPoint : class
    {
        private string _route;

        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        };

        protected void SetPath(string route)
        {
            if (string.IsNullOrWhiteSpace(route))
            {
                _route = string.Empty;
                return;
            }

            if (route.StartsWith("/"))
            {
                route = route.Substring(1, route.Length - 1);
            }

            if (route.EndsWith("/"))
            {
                route = route.Substring(0, route.Length - 1);
            }

            _route = $"{route}/";
        }

        protected static T Map<T>(object data) =>
            JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(data, SerializerOptions), SerializerOptions);

        protected Task<HttpResponseMessage> GetAsync(string endpoint)
            => Client.GetAsync(GetEndpoint(endpoint));

        protected async Task<T> GetAsync<T>(string endpoint)
            => await ReadAsync<T>(await GetAsync(endpoint));

        protected Task<HttpResponseMessage> PostAsync<T>(string endpoint, T command)
            => Client.PostAsync(GetEndpoint(endpoint), GetPayload(command));

        protected Task<HttpResponseMessage> PutAsync<T>(string endpoint, T command)
            => Client.PutAsync(GetEndpoint(endpoint), GetPayload(command));

        protected Task<HttpResponseMessage> DeleteAsync(string endpoint)
            => Client.DeleteAsync(GetEndpoint(endpoint));

        protected Task<HttpResponseMessage> SendAsync(string method, string endpoint)
            => SendAsync(GetMethod(method), endpoint);

        protected Task<HttpResponseMessage> SendAsync(HttpMethod method, string endpoint)
            => Client.SendAsync(new HttpRequestMessage(method, GetEndpoint(endpoint)));

        private static HttpMethod GetMethod(string method)
            => method.ToUpperInvariant() switch
            {
                "GET" => HttpMethod.Get,
                "POST" => HttpMethod.Post,
                "PUT" => HttpMethod.Put,
                "DELETE" => HttpMethod.Delete,
                _ => null
            };

        private string GetEndpoint(string endpoint) => $"{_route}{endpoint}";

        private static StringContent GetPayload(object value)
            => new(JsonSerializer.Serialize(value, SerializerOptions), Encoding.UTF8, "application/json");

        protected static async Task<T> ReadAsync<T>(HttpResponseMessage response)
            => JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), SerializerOptions);

        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }
    }
}