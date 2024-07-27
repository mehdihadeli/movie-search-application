using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BuildingBlocks.Mongo;
using BuildingBlocks.Test.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace Trill.Shared.Tests.Integration;

public abstract class WebApiTestFixture<TEntryPoint, TDbContext> : IntegrationTestFixture<TEntryPoint, TDbContext>
    where TEntryPoint : class
    where TDbContext : class, IMongoDbContext
{
    private static readonly JsonSerializerOptions SerializerOptions =
        new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        };

    private string _route;

    protected void SetPath(string route)
    {
        if (string.IsNullOrWhiteSpace(route))
        {
            _route = string.Empty;
            return;
        }

        if (route.StartsWith("/"))
            route = route.Substring(1, route.Length - 1);

        if (route.EndsWith("/"))
            route = route.Substring(0, route.Length - 1);

        _route = $"{route}/";
    }

    protected static T Map<T>(object data)
    {
        return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(data, SerializerOptions), SerializerOptions);
    }

    protected Task<HttpResponseMessage> GetAsync(string endpoint)
    {
        return Client.GetAsync(GetEndpoint(endpoint));
    }

    protected async Task<T> GetAsync<T>(string endpoint)
    {
        return await ReadAsync<T>(await GetAsync(endpoint));
    }

    protected Task<HttpResponseMessage> PostAsync<T>(string endpoint, T command)
    {
        return Client.PostAsync(GetEndpoint(endpoint), GetPayload(command));
    }

    protected Task<HttpResponseMessage> PutAsync<T>(string endpoint, T command)
    {
        return Client.PutAsync(GetEndpoint(endpoint), GetPayload(command));
    }

    protected Task<HttpResponseMessage> DeleteAsync(string endpoint)
    {
        return Client.DeleteAsync(GetEndpoint(endpoint));
    }

    protected Task<HttpResponseMessage> SendAsync(string method, string endpoint)
    {
        return SendAsync(GetMethod(method), endpoint);
    }

    protected Task<HttpResponseMessage> SendAsync(HttpMethod method, string endpoint)
    {
        return Client.SendAsync(new HttpRequestMessage(method, GetEndpoint(endpoint)));
    }

    private static HttpMethod GetMethod(string method)
    {
        return method.ToUpperInvariant() switch
        {
            "GET" => HttpMethod.Get,
            "POST" => HttpMethod.Post,
            "PUT" => HttpMethod.Put,
            "DELETE" => HttpMethod.Delete,
            _ => null
        };
    }

    private string GetEndpoint(string endpoint)
    {
        return $"{_route}{endpoint}";
    }

    private static StringContent GetPayload(object value)
    {
        return new(JsonSerializer.Serialize(value, SerializerOptions), Encoding.UTF8, "application/json");
    }

    protected static async Task<T> ReadAsync<T>(HttpResponseMessage response)
    {
        return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), SerializerOptions);
    }

    protected virtual void ConfigureServices(IServiceCollection services) { }
}

public abstract class WebApiTestFixture<TEntryPoint> : IntegrationTestFixture<TEntryPoint>
    where TEntryPoint : class
{
    private static readonly JsonSerializerOptions SerializerOptions =
        new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        };

    private string _route;

    protected void SetPath(string route)
    {
        if (string.IsNullOrWhiteSpace(route))
        {
            _route = string.Empty;
            return;
        }

        if (route.StartsWith("/"))
            route = route.Substring(1, route.Length - 1);

        if (route.EndsWith("/"))
            route = route.Substring(0, route.Length - 1);

        _route = $"{route}/";
    }

    protected static T Map<T>(object data)
    {
        return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(data, SerializerOptions), SerializerOptions);
    }

    protected Task<HttpResponseMessage> GetAsync(string endpoint)
    {
        return Client.GetAsync(GetEndpoint(endpoint));
    }

    protected async Task<T> GetAsync<T>(string endpoint)
    {
        return await ReadAsync<T>(await GetAsync(endpoint));
    }

    protected Task<HttpResponseMessage> PostAsync<T>(string endpoint, T command)
    {
        return Client.PostAsync(GetEndpoint(endpoint), GetPayload(command));
    }

    protected Task<HttpResponseMessage> PutAsync<T>(string endpoint, T command)
    {
        return Client.PutAsync(GetEndpoint(endpoint), GetPayload(command));
    }

    protected Task<HttpResponseMessage> DeleteAsync(string endpoint)
    {
        return Client.DeleteAsync(GetEndpoint(endpoint));
    }

    protected Task<HttpResponseMessage> SendAsync(string method, string endpoint)
    {
        return SendAsync(GetMethod(method), endpoint);
    }

    protected Task<HttpResponseMessage> SendAsync(HttpMethod method, string endpoint)
    {
        return Client.SendAsync(new HttpRequestMessage(method, GetEndpoint(endpoint)));
    }

    private static HttpMethod GetMethod(string method)
    {
        return method.ToUpperInvariant() switch
        {
            "GET" => HttpMethod.Get,
            "POST" => HttpMethod.Post,
            "PUT" => HttpMethod.Put,
            "DELETE" => HttpMethod.Delete,
            _ => null
        };
    }

    private string GetEndpoint(string endpoint)
    {
        return $"{_route}{endpoint}";
    }

    private static StringContent GetPayload(object value)
    {
        return new(JsonSerializer.Serialize(value, SerializerOptions), Encoding.UTF8, "application/json");
    }

    protected static async Task<T> ReadAsync<T>(HttpResponseMessage response)
    {
        return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), SerializerOptions);
    }

    protected virtual void ConfigureServices(IServiceCollection services) { }
}
