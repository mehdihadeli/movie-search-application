using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BuildingBlocks.Resiliency.Configs;
using BuildingBlocks.Test.Fixtures;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MovieSearch.Api;
using MovieSearch.Core;
using MovieSearch.Infrastructure.Services.Clients.Video;
using Xunit;

namespace MovieSearch.IntegrationTests.Infrastructure.Services.Clients.Video
{
    public class YoutubeVideoServiceClientTests : IntegrationTestFixture<Startup>
    {
        private readonly YoutubeVideoServiceClient _sut;

        public YoutubeVideoServiceClientTests()
        {
            //setup the swaps for our tests
            RegisterTestServices(services => { });

            _sut = new YoutubeVideoServiceClient(ServiceProvider.GetRequiredService<IOptions<YoutubeVideoOptions>>(),
                ServiceProvider.GetRequiredService<IMapper>(),
                ServiceProvider.GetRequiredService<IOptions<PolicyConfig>>());
        }

        [Fact]
        public async Task get_videos_should_return_correct_data()
        {
            var result = await _sut.GetVideos("finding nemo");

            result.Should().NotBeNull();
            result.Items.Should().NotBeNull();
            result.Items.Any().Should().BeTrue();
            result.PageSize.Should().Be(result.Items.Count);
            result.NextPageToken.Should().NotBeNull();
        }

        [Fact]
        public async Task get_videos_by_next_page_should_return_correct_data()
        {
            var result = await _sut.GetVideos("finding nemo");
            var result2 = await _sut.GetVideos("finding nemo", 20, result.NextPageToken);

            result2.Should().NotBeNull();
            result2.Items.Should().NotBeNull();
            result2.Items.Any().Should().BeTrue();
            result2.PageSize.Should().Be(result2.Items.Count);
            result2.PageToken.Should().NotBeNull();
            result2.NextPageToken.Should().NotBeNull();
        }
    }
}