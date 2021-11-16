using System;
using System.Collections.Generic;
using MovieSearch.Core.Collections;
using MovieSearch.Core.Companies;
using MovieSearch.Core.Generals;
using MovieSearch.Core.Genres;
using MovieSearch.Core.Keywords;

namespace MovieSearch.Core.Movies
{
    public class Movie
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public bool IsAdultThemed { get; init; }
        public string BackdropPath { get; init; }
        public CollectionInfo MovieCollectionInfo { get; init; }
        public int Budget { get; init; }
        public IReadOnlyList<Genre> Genres { get; init; }
        public string Homepage { get; init; }
        public string ImdbId { get; init; }
        /// <summary>
        /// ISO 3166-1 code.
        /// </summary>
        public string OriginalLanguage { get; init; }
        public string OriginalTitle { get; init; }
        public string Overview { get; init; }
        public double Popularity { get; init; }
        public string PosterPath { get; init; }
        public IReadOnlyList<ProductionCompany> ProductionCompanies { get; init; }
        public IReadOnlyList<Country> ProductionCountries { get; init; }
        public DateTime ReleaseDate { get; init; }
        public decimal Revenue { get; init; }
        public int Runtime { get; init; }
        public IReadOnlyList<Language> SpokenLanguages { get; init; }

        public string Status { get; init; }
        public string Tagline { get; init; }
        public bool IsVideo { get; init; }
        public double VoteAverage { get; init; }
        public int VoteCount { get; init; }
        public IReadOnlyList<Keyword> Keywords { get; init; }

        public Movie()
        {
            Genres = Array.Empty<Genre>();
            Keywords = Array.Empty<Keyword>();
            ProductionCompanies = Array.Empty<ProductionCompany>();
            ProductionCountries = Array.Empty<Country>();
            SpokenLanguages = Array.Empty<Language>();
        }

        public override string ToString()
            => $"{Title} ({ReleaseDate:yyyy-MM-dd}) [{Id}]";
    }
}
