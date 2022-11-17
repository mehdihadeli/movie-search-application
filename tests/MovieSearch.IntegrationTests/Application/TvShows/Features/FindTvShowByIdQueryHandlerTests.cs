using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Test.Fixtures;
using DM.MovieApi.IntegrationTests;
using FluentAssertions;
using MovieSearch.Api;
using MovieSearch.Application.TvShows.Features.FindTvShowById;
using MovieSearch.IntegrationTests.Mocks;
using Xunit;

namespace MovieSearch.IntegrationTests.Application.TvShows.Features;

public class FindTvShowByIdQueryHandlerTests : IntegrationTestFixture<Program>
{
    public FindTvShowByIdQueryHandlerTests()
    {
        //setup the swaps for our tests
        RegisterTestServices(services => { });
    }

    [Fact]
    public async Task find_tv_show_by_id_query_should_return_a_valid_tv_show_dto()
    {
        // Arrange
        var query = new FindTvShowByIdQuery(TvShowMock.Data.Id);

        // Act
        var tvShow = (await QueryAsync(query, CancellationToken.None)).TvShow;

        // Assert
        tvShow.Should().NotBeNull();

        tvShow.FirstAirDate.Date.Should().Be(TvShowMock.Data.FirstAirDate);
        tvShow.Homepage.Should().Be(TvShowMock.Data.Homepage);
        tvShow.Name.Should().Be(TvShowMock.Data.Name);

        tvShow.CreatedBy.Should().NotBeNull().And.Subject.Count().Should().Be(TvShowMock.Data.CreatedBy.Count);
        tvShow.EpisodeRunTime.Count.Should().Be(TvShowMock.Data.EpisodeRunTime.Count);
        tvShow.Languages.Should().NotBeNull().And.Subject.Count().Should().Be(TvShowMock.Data.Languages.Count);
        tvShow.Genres.Should().NotBeNull().And.Subject.Count().Should().Be(TvShowMock.Data.Genres.Count);
        tvShow.Networks.Should().NotBeNull().And.Subject.Count().Should().Be(TvShowMock.Data.Networks.Count);
        tvShow.OriginCountry.Should().NotBeNull().And.Subject.Count().Should()
            .Be(TvShowMock.Data.OriginCountry.Count);
        tvShow.OriginalLanguage.Should().NotBeNull().And.Subject.Length.Should()
            .Be(TvShowMock.Data.OriginalLanguage.Length);
        tvShow.ProductionCompanies.Should().NotBeNull().And.Subject.Count().Should()
            .Be(TvShowMock.Data.ProductionCompanies.Count);

        TMDBTestUtil.AssertImagePath(tvShow.BackdropPath);
        TMDBTestUtil.AssertImagePath(tvShow.PosterPath);
    }
}