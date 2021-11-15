using System;
using System.Collections.Generic;
using BuildingBlocks.Domain;
using MovieSearch.Core.Aggregates.Companies;
using MovieSearch.Core.Aggregates.Genres;
using MovieSearch.Core.Aggregates.Reviews;

namespace MovieSearch.Core.Aggregates.Movies
{
    public class Movie : IAggregate<int>
    {
        public int Id { get; init; }
        public bool Adult { get; init; }
        public string BackdropPath { get; init; }
        public dynamic BelongsToCollection { get; init; }
        public long Budget { get; init; }
        public IReadOnlyList<Genre> Genres { get; private set; }
        public string Homepage { get; init; }
        public int ImdbId { get; init; }
        public string OriginalLanguage { get; init; }
        public string OriginalTitle { get; init; }
        public string Overview { get; init; }
        public double Popularity { get; init; }
        public string PosterPath { get; init; }
        public IReadOnlyList<Company> ProductionCompanies { get; private set; }
        public IReadOnlyList<MovieProductionCountry> ProductionCountries { get; private set; }
        public DateTime? ReleaseDate { get; init; }
        public long Revenue { get; init; }
        public int? Runtime { get; init; }
        public string Status { get; init; }
        public string Tagline { get; init; }
        public string Title { get; init; }
        public bool Video { get; init; }
        public double VoteAverage { get; init; }
        public int VoteCount { get; init; }

        public ListResultModel<Review> Reviews { get; private set; }

        public void ChangeReviews(ListResultModel<Review> reviews)
        {
            if (reviews is null)
                return;

            Reviews = reviews;
        }

        public void ChangeGenre(IReadOnlyList<Genre> genres)
        {
            if (genres is null)
                return;

            Genres = genres;
        }
    }
}