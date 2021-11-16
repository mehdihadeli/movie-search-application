using System;
using System.Collections.Generic;
using MovieSearch.Core.Generals;
using MovieSearch.Core.Genres;

namespace MovieSearch.Core.People
{
    public class PersonInfo: MultiInfo
    {
        public string Name { get; init; }
        public bool IsAdultFilmStar { get; init; }
        public IReadOnlyList<PersonInfoRole> KnownFor { get; init; }
        public string ProfilePath { get; init; }
        public override MediaType MediaType { get; init; } = MediaType.Person;

        public PersonInfo()
        {
            KnownFor = Array.Empty<PersonInfoRole>();
        }
        public override string ToString()
            => $"{Name} ({Id})";
    }

    public class PersonInfoRole
    {

        /// <summary>
        /// The MovieId or TVShowId as defined by the value of <see cref="MediaType"/>.
        /// </summary>
        public int Id { get; init; }
        public MediaType MediaType { get; init; }

        /// <summary>
        /// Only populated when <see cref="MediaType"/> is TV.
        /// </summary>
        public string TVShowName { get; init; }

        /// <summary>
        /// Only populated when <see cref="MediaType"/> is TV.
        /// </summary>
        public string TVShowOriginalName { get; init; }

        /// <summary>
        /// Only populated when <see cref="MediaType"/> is Movie.
        /// </summary>
        public string MovieTitle { get; init; }

        /// <summary>
        /// Only populated when <see cref="MediaType"/> is Movie.
        /// </summary>
        public string MovieOriginalTitle { init; get; }

        public string BackdropPath { get; init; }

        public string PosterPath { get; init; }

        /// <summary>
        /// Only populated when <see cref="MediaType"/> is Movie.
        /// </summary>
        public DateTime MovieReleaseDate { get; init; }

        /// <summary>
        /// Only populated when <see cref="MediaType"/> is TV.
        /// </summary>
        public DateTime TVShowFirstAirDate { get; init; }
        public string Overview { get; init; }
        public bool IsAdultThemed { get; init; }
        public bool IsVideo { get; init; }
        public IReadOnlyList<int> GenreIds { get; init; }
        public IReadOnlyList<Genre> Genres { get; init; }
        public string OriginalLanguage { get; init; }
        public double Popularity { get; init; }
        public int VoteCount { get; init; }
        public double VoteAverage { get; init; }
        public IReadOnlyList<string> OriginCountry { get; init; }

        public PersonInfoRole()
        {
            GenreIds = Array.Empty<int>();
            Genres = Array.Empty<Genre>();
            OriginCountry = Array.Empty<string>();
        }

        public override string ToString()
        {
            return MediaType == MediaType.Movie
                ? $"Movie: {MovieTitle} ({Id} - {MovieReleaseDate:yyyy-MM-dd})"
                : $"TV: {TVShowName} ({Id} - {TVShowFirstAirDate:yyyy-MM-dd})";
        }
    }
}
