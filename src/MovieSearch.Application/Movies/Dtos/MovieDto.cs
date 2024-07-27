using System;
using System.Collections.Generic;
using MovieSearch.Application.Companies.Dtos;
using MovieSearch.Application.Generals.Dtos;
using MovieSearch.Application.Genres.Dtos;
using MovieSearch.Core.Collections;

namespace MovieSearch.Application.Movies.Dtos;

public class MovieDto
{
    public int Id { get; init; }
    public string Title { get; init; }
    public bool Adult { get; init; }
    public string BackdropPath { get; init; }
    public CollectionInfo MovieCollectionInfo { get; init; }
    public int Budget { get; init; }
    public IReadOnlyList<GenreDto> Genres { get; init; }
    public string Homepage { get; init; }
    public string ImdbId { get; init; }
    public string OriginalLanguage { get; init; }
    public string OriginalTitle { get; init; }
    public string Overview { get; init; }
    public double Popularity { get; init; }
    public string PosterPath { get; init; }
    public IReadOnlyList<ProductionCompanyDto> ProductionCompanies { get; init; }
    public IReadOnlyList<CountryDto> ProductionCountries { get; init; }
    public DateTime ReleaseDate { get; init; }
    public decimal Revenue { get; init; }
    public int Runtime { get; init; }
    public IReadOnlyList<LanguageDto> SpokenLanguages { get; init; }
    public string Status { get; init; }
    public string Tagline { get; init; }
    public bool IsVideo { get; init; }
    public double VoteAverage { get; init; }
    public int VoteCount { get; init; }
}
