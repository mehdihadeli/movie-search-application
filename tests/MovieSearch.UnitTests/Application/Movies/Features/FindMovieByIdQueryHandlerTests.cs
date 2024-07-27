using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MovieSearch.Application.Movies.Dtos;
using MovieSearch.Application.Movies.Exceptions;
using MovieSearch.Application.Movies.Features.FindById;
using MovieSearch.Application.Services.Clients;
using MovieSearch.UnitTests.Mocks;
using NSubstitute;
using Orders.UnitTests.Common;
using Xunit;

namespace MovieSearch.UnitTests.Application.Movies.Features;

public class FindMovieByIdQueryHandlerTests : UnitTestFixture
{
    private readonly FindMovieByIdQueryHandler _handler;
    private readonly IMovieDbServiceClient _movieDbServiceClient;

    public FindMovieByIdQueryHandlerTests()
    {
        // Arrange
        _movieDbServiceClient = Substitute.For<IMovieDbServiceClient>();
        _handler = new FindMovieByIdQueryHandler(_movieDbServiceClient, Mapper);
    }

    private Task<FindMovieByIdQueryResult> Act(FindMovieByIdQuery query, CancellationToken cancellationToken)
    {
        return _handler.Handle(query, cancellationToken);
    }

    [Fact]
    public async Task handle_with_invalid_movie_by_id_query_should_throw_movie_not_found_exception()
    {
        var query = new FindMovieByIdQuery { Id = 1 };

        //Act && Assert
        var act = async () =>
        {
            await Act(query, CancellationToken.None);
        };
        await act.Should().ThrowAsync<MovieNotFoundException>();
    }

    [Fact]
    public async Task handle_with_valid_movie_by_id_query_should_return_correct_movie_dto()
    {
        // Arrange
        var query = new FindMovieByIdQuery { Id = MovieMocks.Data.Id };

        _movieDbServiceClient
            .GetMovieByIdAsync(Arg.Is(query.Id), Arg.Any<CancellationToken>())
            .Returns(MovieMocks.Data);

        // Act
        var result = await Act(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Movie.Should().NotBeNull();
        result.Movie.Should().BeOfType<MovieDto>();
        result.Movie.Id.Should().Be(query.Id);
        result.Movie.Title.Should().Be(MovieMocks.Data.Title);
    }
}
