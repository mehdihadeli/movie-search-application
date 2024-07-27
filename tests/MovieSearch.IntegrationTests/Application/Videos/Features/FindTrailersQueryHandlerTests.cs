using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Test.Fixtures;
using FluentAssertions;
using MovieSearch.Api;
using MovieSearch.Application.Videos.Dtos;
using MovieSearch.Application.Videos.Features.FindTrailers;
using MovieSearch.Core.Generals;
using MovieSearch.IntegrationTests.Mocks;
using Xunit;

namespace MovieSearch.IntegrationTests.Application.Videos.Features;

public class FindTrailersQueryHandlerTests : IntegrationTestFixture<Program>
{
    public FindTrailersQueryHandlerTests()
    {
        //setup the swaps for our tests
        RegisterTestServices(services => { });
    }

    [Fact]
    public async Task find_trailers_query_should_return_a_valid_video_dto_list()
    {
        // Arrange
        var query = new FindTrailersQuery
        {
            MovieName = MovieMocks.Data.Title,
            PublishedAfter = MovieMocks.Data.ReleaseDate,
            PageSize = 20
        };

        // Act
        var listResult = (await QueryAsync(query, CancellationToken.None)).VideoList;

        // Assert
        listResult.Should().NotBeNull();
        listResult.Items.Should().NotBeNull();
        listResult.Should().BeOfType<VideoListResultModel<VideoDto>>();
        listResult.Items.Any().Should().BeTrue();
        listResult.PageSize.Should().Be(listResult.Items.Count);
        listResult.Items.All(x => x.PublishedAt >= query.PublishedAfter).Should().BeTrue();
        listResult.PageSize.Should().Be(query.PageSize);
        listResult.NextPageToken.Should().NotBeNull();
        listResult.PageToken.Should().BeEmpty();
        listResult.PreviousPageToken.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task find_movie_trailers_query_with_next_page_should_return_a_valid_video_dto_list()
    {
        // Arrange
        var query = new FindTrailersQuery
        {
            MovieName = MovieMocks.Data.Title,
            PublishedAfter = MovieMocks.Data.ReleaseDate,
            PageSize = 20
        };

        // Act
        var listResult = (await QueryAsync(query, CancellationToken.None)).VideoList;

        var query2 = new FindTrailersQuery
        {
            MovieName = MovieMocks.Data.Title,
            PageSize = 20,
            PublishedAfter = MovieMocks.Data.ReleaseDate,
            PageToken = listResult.NextPageToken
        };

        var listResult2 = (await QueryAsync(query2, CancellationToken.None)).VideoList;

        listResult2.Should().NotBeNull();
        listResult2.Items.Should().NotBeNull();
        listResult2.Items.Any().Should().BeTrue();
        listResult2.PageSize.Should().Be(listResult2.Items.Count);
        listResult2.Items.All(x => x.PublishedAt >= query.PublishedAfter).Should().BeTrue();
        listResult2.PageSize.Should().Be(query2.PageSize);
        listResult2.PageToken.Should().NotBeNull();
        listResult2.PageToken.Should().Be(listResult.NextPageToken);
        listResult2.NextPageToken.Should().NotBeNull();
        listResult2.PreviousPageToken.Should().NotBeNull();
    }
}
