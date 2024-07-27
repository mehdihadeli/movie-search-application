using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Domain;
using BuildingBlocks.Test.Fixtures;
using DM.MovieApi.IntegrationTests;
using FluentAssertions;
using MovieSearch.Api;
using MovieSearch.Application.TvShows.Dtos;
using MovieSearch.Application.TvShows.Features.SearchTVShow;
using MovieSearch.IntegrationTests.Mocks;
using Xunit;

namespace MovieSearch.IntegrationTests.Application.TvShows.Features;

public class SearchTVShowQueryHandlerTests : IntegrationTestFixture<Program>
{
    public SearchTVShowQueryHandlerTests()
    {
        //setup the swaps for our tests
        RegisterTestServices(services => { });
    }

    [Fact]
    public async Task search_tv_show_query_should_return_a_valid_tv_show_info_list_dto()
    {
        // Arrange
        var query = new SearchTVShowQuery(TvShowMock.Data.Name, 1, includeAdult: true);

        // Act
        var listResult = (await QueryAsync(query, CancellationToken.None)).TVShowList;

        // Assert
        listResult.Should().NotBeNull();
        listResult.Should().BeOfType<ListResultModel<TVShowInfoDto>>();
        listResult.Page.Should().Be(1);
        listResult.PageSize.Should().Be(listResult.Items.Count);
        listResult.Items.Any().Should().BeTrue();
        TMDBTestUtil.AssertTvShowInformationDtoStructure(listResult.Items);
    }
}
