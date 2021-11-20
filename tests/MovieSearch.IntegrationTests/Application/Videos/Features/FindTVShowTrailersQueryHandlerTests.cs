using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Test.Fixtures;
using FluentAssertions;
using MovieSearch.Api;
using MovieSearch.Application.Videos.Dtos;
using MovieSearch.Application.Videos.Features.FindTVShowTrailers;
using MovieSearch.Core.Generals;
using MovieSearch.IntegrationTests.Mocks;
using Xunit;

namespace MovieSearch.IntegrationTests.Application.Videos.Features
{
    public class FindTVShowTrailersQueryHandlerTests: IntegrationTestFixture<Startup>
    {
        public FindTVShowTrailersQueryHandlerTests()
        {
            //setup the swaps for our tests
            RegisterTestServices(services => { });
        }

        [Fact]
        public async Task find_tv_show_trailers_query_should_return_a_valid_video_dto_list()
        {
            // Arrange
            var query = new FindTVShowTrailersQuery
            {
                TVShowId = TvShowMock.Data.Id,
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
            listResult.PageSize.Should().Be(query.PageSize);
            listResult.NextPageToken.Should().NotBeNull();
            listResult.PageToken.Should().BeEmpty();
            listResult.PreviousPageToken.Should().BeNullOrEmpty();
        }
    }
}