using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Test.Fixtures;
using DM.MovieApi.IntegrationTests;
using FluentAssertions;
using MovieSearch.Api;
using MovieSearch.Application.Movies.Features.FindMovieByImdbId;
using MovieSearch.IntegrationTests.Mocks;
using Xunit;

namespace MovieSearch.IntegrationTests.Application.Movies.Features
{
    public class FindMovieByImdbIdQueryHandlerTests : IntegrationTestFixture<Startup>
    {
        public FindMovieByImdbIdQueryHandlerTests()
        {
            //setup the swaps for our tests
            RegisterTestServices(services => { });
        }

        [Fact]
        public async Task find_movie_by_imdbid_query_should_return_a_valid_movie_dto()
        {
            // Arrange
            var query = new FindMovieByImdbIdQuery { ImdbId = MovieMocks.Data.ImdbId };

            // Act
            var movie = (await QueryAsync(query, CancellationToken.None)).Movie;

            // Assert
            movie.Should().NotBeNull();
            movie.Id.Should().Be(MovieMocks.Data.Id);
            movie.ImdbId.Should().Be(MovieMocks.Data.ImdbId);
            movie.Title.Should().Be(MovieMocks.Data.Title);
            movie.OriginalTitle.Should().Be(MovieMocks.Data.OriginalTitle);
            movie.Tagline.Should().Be(MovieMocks.Data.Tagline);
            movie.OriginalLanguage.Should().Be(MovieMocks.Data.OriginalLanguage);
            movie.Homepage.Should().Be(MovieMocks.Data.Homepage);
            movie.Status.Should().Be(MovieMocks.Data.Status);
            movie.Budget.Should().Be(MovieMocks.Data.Budget);
            movie.Runtime.Should().Be(MovieMocks.Data.Runtime);
            movie.ReleaseDate.Should().Be(MovieMocks.Data.ReleaseDate);
            movie.Overview.StartsWith(MovieMocks.Data.Overview).Should().BeTrue();
            (movie.Popularity > 7).Should().BeTrue();
            (movie.VoteAverage > 5).Should().BeTrue();
            (movie.VoteCount > 1500).Should().BeTrue();
            TMDBTestUtil.AssertImagePath(movie.BackdropPath);
            TMDBTestUtil.AssertImagePath(movie.PosterPath);

            movie.SpokenLanguages.Count.Should().Be(MovieMocks.Data.SpokenLanguages.Count);
            movie.ProductionCompanies.Count.Should().Be(MovieMocks.Data.ProductionCompanies.Count);
            movie.ProductionCountries.Count.Should().Be(MovieMocks.Data.ProductionCountries.Count);
            movie.Genres.Count.Should().Be(MovieMocks.Data.Genres.Count);

            movie.MovieCollectionInfo.Should().NotBeNull();
            movie.MovieCollectionInfo.Id.Should().Be(10);
            movie.MovieCollectionInfo.Name.Should().Be("Star Wars Collection");
            TMDBTestUtil.AssertImagePath(movie.MovieCollectionInfo.BackdropPath);
            TMDBTestUtil.AssertImagePath(movie.MovieCollectionInfo.PosterPath);
        }
    }
}