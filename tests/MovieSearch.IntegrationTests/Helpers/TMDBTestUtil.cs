using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieSearch.Core.Companies;
using MovieSearch.Core.Generals;
using MovieSearch.Core.Genres;
using MovieSearch.Core.Movies;
using MovieSearch.Core.People;
using MovieSearch.Core.TV;

namespace DM.MovieApi.IntegrationTests
{
    internal static class TMDBTestUtil
    {
        internal const int TestInitThrottle = 375;
        internal const int PagingThrottle = 225;

        /// <summary>
        /// Slows down the starting of tests to keep themoviedb.org api from denying the request
        /// due to too many requests. This should be placed in the [TestInitialize] method.
        /// </summary>
        public static void ThrottleTests()
        {
            System.Threading.Thread.Sleep(TestInitThrottle);
        }

        public static void AssertImagePath(string path)
        {
            Assert.IsTrue(path.StartsWith("/"), $"Actual: {path}");

            Assert.IsTrue(
                path.EndsWith(".jpg") || path.EndsWith(".png"),
                $"Actual: {path}");
        }

        private static void AssertPersonInfoStructure(IEnumerable<PersonInfo> people)
        {
            // ReSharper disable PossibleMultipleEnumeration
            Assert.IsTrue(people.Any());

            foreach (PersonInfo person in people)
            {
                Assert.IsTrue(person.Id > 0);
                Assert.IsFalse(string.IsNullOrWhiteSpace(person.Name));

                foreach (PersonInfoRole role in person.KnownFor)
                {
                    // not asserting movie/tv dates as some valid dates will be null.
                    if (role.MediaType == MediaType.Movie)
                    {
                        Assert.IsFalse(string.IsNullOrWhiteSpace(role.MovieTitle));
                        Assert.IsFalse(string.IsNullOrWhiteSpace(role.MovieOriginalTitle));

                        Assert.IsNull(role.TVShowName);
                        Assert.IsNull(role.TVShowOriginalName);
                        Assert.AreEqual(DateTime.MinValue, role.TVShowFirstAirDate);
                    }
                    else
                    {
                        Assert.IsFalse(string.IsNullOrWhiteSpace(role.TVShowName));
                        Assert.IsFalse(string.IsNullOrWhiteSpace(role.TVShowOriginalName));

                        Assert.IsNull(role.MovieTitle);
                        Assert.IsNull(role.MovieOriginalTitle);
                        Assert.AreEqual(DateTime.MinValue, role.MovieReleaseDate);
                    }

                    AssertGenres(role.GenreIds, role.Genres);
                }
            }
            // ReSharper restore PossibleMultipleEnumeration
        }

        public static void AssertMovieStructure(IEnumerable<Movie> movies)
        {
            var enumerable = movies as Movie[] ?? movies.ToArray();
            Assert.IsTrue(enumerable.Any());
            foreach (Movie movie in enumerable)
            {
                AssertMovieStructure(movie);
            }
        }

        public static void AssertMovieStructure(Movie movie)
        {
            Assert.IsTrue(movie.Id > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(movie.Title));

            foreach (Genre genre in movie.Genres)
            {
                Assert.IsTrue(genre.Id > 0);
                Assert.IsFalse(string.IsNullOrWhiteSpace(genre.Name));
            }

            foreach (ProductionCompany info in movie.ProductionCompanies)
            {
                Assert.IsTrue(info.Id > 0);
                Assert.IsFalse(string.IsNullOrWhiteSpace(info.Name));
            }

            foreach (Country country in movie.ProductionCountries)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(country.Iso3166Code));
                Assert.IsFalse(string.IsNullOrWhiteSpace(country.Name));
            }

            foreach (Language language in movie.SpokenLanguages)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(language.Iso639Code));
                Assert.IsFalse(string.IsNullOrWhiteSpace(language.Name));
            }
        }

        public static void AssertMovieInformationStructure(IEnumerable<MovieInfo> movies)
        {
            var movieInfos = movies.ToList();
            Assert.IsTrue(movieInfos.Any());

            foreach (MovieInfo movie in movieInfos)
            {
                AssertMovieInformationStructure(movie);
            }
        }

        public static void AssertMovieInformationStructure(MovieInfo movie)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(movie.Title));
            Assert.IsTrue(movie.Id > 0);

            AssertGenres(movie.GenreIds);
        }

        public static void AssertTvShowInformationStructure(IEnumerable<TVShowInfo> tvShows)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            Assert.IsTrue(tvShows.Any());

            // ReSharper disable once PossibleMultipleEnumeration
            foreach (TVShowInfo tvShow in tvShows)
            {
                AssertTvShowInformationStructure(tvShow);
            }
        }

        public static void AssertTvShowInformationStructure(TVShowInfo tvShow)
        {
            Assert.IsTrue(tvShow.Id > 0);
            Assert.IsFalse(string.IsNullOrEmpty(tvShow.Name));

            AssertGenres(tvShow.GenreIds);
        }

        private static void AssertGenres(IReadOnlyList<int> genreIds, IReadOnlyList<Genre> genres)
        {
            Assert.AreEqual(genreIds.Count, genres.Count);

            foreach (Genre genre in genres)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(genre.Name));
                Assert.IsTrue(genre.Id > 0);
            }
        }

        private static void AssertGenres(IReadOnlyList<int> genreIds)
        {
            Assert.IsNotNull(genreIds);
        }
    }
}