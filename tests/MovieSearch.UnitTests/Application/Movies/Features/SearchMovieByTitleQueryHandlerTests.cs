using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Domain;
using FluentAssertions;
using MovieSearch.Application.Movies.Features.SearchMovieByTitle;
using MovieSearch.Application.Services.Clients;
using MovieSearch.Core.Movies;
using MovieSearch.UnitTests.Mocks;
using NSubstitute;
using Orders.UnitTests.Common;
using Xunit;

namespace MovieSearch.UnitTests.Application.Movies.Features;

public class SearchMovieByTitleQueryHandlerTests : UnitTestFixture
{
    private readonly SearchMovieByTitleQueryHandler _handler;
    private readonly IMovieDbServiceClient _movieDbServiceClient;

    public SearchMovieByTitleQueryHandlerTests()
    {
        // Arrange
        _movieDbServiceClient = Substitute.For<IMovieDbServiceClient>();
        _handler = new SearchMovieByTitleQueryHandler(_movieDbServiceClient, Mapper);
    }

    private Task<SearchMovieByTitleQueryResult> Act(SearchMovieByTitleQuery query, CancellationToken cancellationToken)
    {
        return _handler.Handle(query, cancellationToken);
    }

    [Fact]
    public async Task handle_with_valid_search_movie_by_title_query_should_return_correct_movie_info_list_dto()
    {
        // Arrange
        var query = new SearchMovieByTitleQuery { Page = 1, SearchKeywords = MovieMocks.Data.Title };

        var movieInfoList = new ListResultModel<MovieInfo>(
            new List<MovieInfo>
            {
                new()
                {
                    Adult = true,
                    Id = MovieMocks.Data.Id,
                    Title = MovieMocks.Data.Title
                }
            },
            1,
            query.Page,
            20
        );

        _movieDbServiceClient
            .SearchMovieAsync(query.SearchKeywords, query.Page, false, 0, 0, Arg.Any<CancellationToken>())
            .Returns(movieInfoList);

        // Act
        var result = await Act(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.MovieList.Should().NotBeNull();
        result.MovieList.PageSize.Should().Be(movieInfoList.PageSize);
        result.MovieList.Page.Should().Be(query.Page);
        result.MovieList.Items.Should().NotBeNull();
        result.MovieList.Items.Any().Should().BeTrue();
        result.MovieList.Items.First().Id.Should().Be(movieInfoList.Items.First().Id);
        result.MovieList.Items.First().Title.Should().Be(movieInfoList.Items.First().Title);
    }
}
