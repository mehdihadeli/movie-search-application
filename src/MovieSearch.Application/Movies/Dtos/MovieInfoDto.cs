using System;
using System.Collections.Generic;
using MovieSearch.Application.Generals.Dtos;
using MovieSearch.Core.Generals;

namespace MovieSearch.Application.Movies.Dtos
{
    public class MovieInfoDto : MultiInfoDto
    {
        public string Title { get; init; }
        public bool Adult { get; init; }
        public string BackdropPath { get; init; }
        public IReadOnlyList<int> GenreIds { get; init; }
        public string OriginalTitle { get; init; }
        public string OriginalLanguage { get; init; }
        public string Overview { get; init; }
        public override MediaType MediaType { get; init; } = MediaType.Movie;
        public DateTime ReleaseDate { get; init; }
        public string PosterPath { get; init; }
        public bool Video { get; init; }
        public double VoteAverage { get; init; }
        public int VoteCount { get; init; }
    }
}