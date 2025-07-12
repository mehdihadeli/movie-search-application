using AutoMapper;
using MovieSearch.Application.Companies;
using MovieSearch.Application.Generals;
using MovieSearch.Application.Genres;
using MovieSearch.Application.Movies;
using MovieSearch.Application.People;
using MovieSearch.Application.TvShows;
using MovieSearch.Application.Videos;
using MovieSearch.Infrastructure;

namespace Orders.UnitTests.Common;

public static class MapperFactory
{
    public static IMapper Create()
    {
        var configurationProvider = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<InfrastructureMappings>();
            cfg.AddProfile<CompanyMappings>();
            cfg.AddProfile<GeneralMappings>();
            cfg.AddProfile<GenreMappings>();
            cfg.AddProfile<MovieMappings>();
            cfg.AddProfile<PeopleMappings>();
            cfg.AddProfile<TvShowMappings>();
            cfg.AddProfile<VideoMappings>();
        }, null);

        return configurationProvider.CreateMapper();
    }
}
