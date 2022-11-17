using Microsoft.Extensions.DependencyInjection;

namespace MovieSearch.Application;

//https://github.com/madeyoga/YoutubeSearchApi.Net
//https://github.com/Mayerch1/YoutubeSearch
public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}