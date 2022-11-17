using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Test.Fixtures;
using DM.MovieApi.IntegrationTests;
using FluentAssertions;
using MovieSearch.Api;
using MovieSearch.Application.Movies.Features.FindMovieWithTrailersById;
using MovieSearch.IntegrationTests.Mocks;
using Xunit;

namespace MovieSearch.IntegrationTests.Application.Movies.Features;

public class FindMovieWithTrailersByIdQueryHandlerTests : IntegrationTestFixture<Program>
{
    public FindMovieWithTrailersByIdQueryHandlerTests()
    {
        //setup the swaps for our tests
        RegisterTestServices(services => { });
    }

    [Fact]
    public async Task find_movie_with_trailers_by_id_query_should_return_a_valid_movie_with_trailers_dto()
    {
        // Arrange
        var query = new FindMovieWithTrailersByIdQuery(MovieMocks.Data.Id);

        // Act
        var movieWithTrailers = (await QueryAsync(query, CancellationToken.None)).MovieWithTrailers;

        // Assert
        movieWithTrailers.Movie.Should().NotBeNull();
        movieWithTrailers.Trailers.Should().NotBeNull();
        movieWithTrailers.Trailers.Any().Should().BeTrue();
        movieWithTrailers.Trailers.Count().Should().Be(query.TrailersCount);
        movieWithTrailers.Trailers.All(x => string.IsNullOrEmpty(x.Key) == false).Should().BeTrue();

        movieWithTrailers.Movie.Id.Should().Be(MovieMocks.Data.Id);
        movieWithTrailers.Movie.ImdbId.Should().Be(MovieMocks.Data.ImdbId);
        movieWithTrailers.Movie.Title.Should().Be(MovieMocks.Data.Title);
        movieWithTrailers.Movie.OriginalTitle.Should().Be(MovieMocks.Data.OriginalTitle);
        movieWithTrailers.Movie.Tagline.Should().Be(MovieMocks.Data.Tagline);
        movieWithTrailers.Movie.OriginalLanguage.Should().Be(MovieMocks.Data.OriginalLanguage);
        movieWithTrailers.Movie.Homepage.Should().Be(MovieMocks.Data.Homepage);
        movieWithTrailers.Movie.Status.Should().Be(MovieMocks.Data.Status);
        movieWithTrailers.Movie.Budget.Should().Be(MovieMocks.Data.Budget);
        movieWithTrailers.Movie.Runtime.Should().Be(MovieMocks.Data.Runtime);
        movieWithTrailers.Movie.ReleaseDate.Should().Be(MovieMocks.Data.ReleaseDate);
        movieWithTrailers.Movie.Overview.StartsWith(MovieMocks.Data.Overview).Should().BeTrue();
        (movieWithTrailers.Movie.Popularity > 7).Should().BeTrue();
        (movieWithTrailers.Movie.VoteAverage > 5).Should().BeTrue();
        (movieWithTrailers.Movie.VoteCount > 1500).Should().BeTrue();
        TMDBTestUtil.AssertImagePath(movieWithTrailers.Movie.BackdropPath);
        TMDBTestUtil.AssertImagePath(movieWithTrailers.Movie.PosterPath);

        movieWithTrailers.Movie.SpokenLanguages.Count.Should().Be(MovieMocks.Data.SpokenLanguages.Count);
        movieWithTrailers.Movie.ProductionCompanies.Count.Should().Be(MovieMocks.Data.ProductionCompanies.Count);
        movieWithTrailers.Movie.ProductionCountries.Count.Should().Be(MovieMocks.Data.ProductionCountries.Count);
        movieWithTrailers.Movie.Genres.Count.Should().Be(MovieMocks.Data.Genres.Count);

        movieWithTrailers.Movie.MovieCollectionInfo.Should().NotBeNull();
        movieWithTrailers.Movie.MovieCollectionInfo.Id.Should().Be(10);
        movieWithTrailers.Movie.MovieCollectionInfo.Name.Should().Be("Star Wars Collection");
        TMDBTestUtil.AssertImagePath(movieWithTrailers.Movie.MovieCollectionInfo.BackdropPath);
        TMDBTestUtil.AssertImagePath(movieWithTrailers.Movie.MovieCollectionInfo.PosterPath);
    }
}