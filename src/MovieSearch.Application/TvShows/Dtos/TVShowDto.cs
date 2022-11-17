using System;
using System.Collections.Generic;
using MovieSearch.Application.Companies.Dtos;
using MovieSearch.Application.Generals.Dtos;
using MovieSearch.Application.Genres.Dtos;
using MovieSearch.Core.TV;

namespace MovieSearch.Application.TvShows.Dtos;

public class TVShowDto
{
    public int Id { get; init; }
    public string BackdropPath { get; init; }
    public IReadOnlyList<TVShowCreatorDto> CreatedBy { get; init; }
    public IReadOnlyList<int> EpisodeRunTime { get; init; }
    public DateTime FirstAirDate { get; init; }
    public IReadOnlyList<GenreDto> Genres { get; init; }
    public string Homepage { get; init; }
    public bool InProduction { get; init; }
    public IReadOnlyList<string> Languages { get; init; }
    public DateTime LastAirDate { get; init; }
    public string Name { get; init; }
    public IReadOnlyList<Network> Networks { get; init; }
    public int NumberOfEpisodes { get; init; }
    public int NumberOfSeasons { get; init; }
    public IReadOnlyList<string> OriginCountry { get; init; }
    public string OriginalLanguage { get; init; }
    public string OriginalName { get; init; }
    public string Overview { get; init; }
    public double Popularity { get; init; }
    public string PosterPath { get; init; }
    public IReadOnlyList<ProductionCompanyDto> ProductionCompanies { get; init; }
    public IReadOnlyList<CountryDto> ProductionCountries { get; init; }
    public IReadOnlyList<Season> Seasons { get; init; }
}