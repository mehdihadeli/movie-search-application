using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MovieSearch.Application.Movies.Exceptions;
using MovieSearch.Application.Movies.Features.FindMovieWithTrailersById;
using MovieSearch.Application.Services.Clients;
using MovieSearch.Core.Generals;
using MovieSearch.UnitTests.Mocks;
using NSubstitute;
using Orders.UnitTests.Common;
using Xunit;

namespace MovieSearch.UnitTests.Application.Movies.Features;

public class FindMovieWithTrailersByIdQueryHandlerTests : UnitTestFixture
{
    private readonly FindMovieWithTrailersByIdQueryHandler _handler;
    private readonly IMovieDbServiceClient _movieDbServiceClient;
    private readonly IVideoServiceClient _videoServiceClient;

    public FindMovieWithTrailersByIdQueryHandlerTests()
    {
        // Arrange
        _movieDbServiceClient = Substitute.For<IMovieDbServiceClient>();
        _videoServiceClient = Substitute.For<IVideoServiceClient>();
        _handler = new FindMovieWithTrailersByIdQueryHandler(_movieDbServiceClient, _videoServiceClient, Mapper);
    }

    private Task<FindMovieWithTrailersByIdQueryResult> Act(FindMovieWithTrailersByIdQuery query, CancellationToken
        cancellationToken)
    {
        return _handler.Handle(query, cancellationToken);
    }

    [Fact]
    public async Task handle_and_invalid_movie_with_trailers_by_id_query_should_throw_movie_not_found_exception()
    {
        var query = new FindMovieWithTrailersByIdQuery(1);
        //Act && Assert
        var act = async () => { await Act(query, CancellationToken.None); };
        await act.Should().ThrowAsync<MovieNotFoundException>();
    }

    [Fact]
    public async Task
        handle_and_valid_movie_with_trailers_by_id_query_should_return_correct_movie_with_trailers_dto()
    {
        // Arrange
        var query = new FindMovieWithTrailersByIdQuery(MovieMocks.Data.Id);
        var movieList = new VideoListResultModel<Video>(new List<Video>
        {
            new()
            {
                Id = "TestId",
                Key = "TestKey"
            }
        }, 1, "", "123", null, query.TrailersCount);

        _movieDbServiceClient.GetMovieByIdAsync(Arg.Is(query.MovieId), Arg.Any<CancellationToken>())
            .Returns(MovieMocks.Data);

        _videoServiceClient.GetTrailers(Arg.Is(MovieMocks.Data.Title)).Returns(movieList);

        // Act
        var result = await Act(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.MovieWithTrailers.Should().NotBeNull();
        result.MovieWithTrailers.Movie.Id.Should().Be(query.MovieId);
        result.MovieWithTrailers.Movie.Title.Should().Be(MovieMocks.Data.Title);
        result.MovieWithTrailers.Trailers.Should().NotBeNull();
        result.MovieWithTrailers.Trailers.Count().Should().Be(1);
        result.MovieWithTrailers.Trailers.First().Id.Should().Be("TestId");
        result.MovieWithTrailers.Trailers.First().Key.Should().Be("TestKey");
    }
}