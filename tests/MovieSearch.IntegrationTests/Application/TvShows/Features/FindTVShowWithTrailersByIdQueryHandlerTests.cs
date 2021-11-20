using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Test.Fixtures;
using DM.MovieApi.IntegrationTests;
using FluentAssertions;
using MovieSearch.Api;
using MovieSearch.Application.TvShows.Features.FindTVShowWithTrailersById;
using MovieSearch.IntegrationTests.Mocks;
using Xunit;

namespace MovieSearch.IntegrationTests.Application.TvShows.Features
{
    public class FindTVShowWithTrailersByIdQueryHandlerTests : IntegrationTestFixture<Startup>
    {
        public FindTVShowWithTrailersByIdQueryHandlerTests()
        {
            //setup the swaps for our tests
            RegisterTestServices(services => { });
        }

        [Fact]
        public async Task find_tv_show_with_trailers_by_id_query_should_return_a_valid_tv_show_with_trailers_dto()
        {
            // Arrange
            var query = new FindTVShowWithTrailersByIdQuery(TvShowMock.Data.Id, 20);

            // Act
            var tvShowWithTrailers = (await QueryAsync(query, CancellationToken.None)).TVShowWithTrailers;

            // Assert
            tvShowWithTrailers.Should().NotBeNull();

            tvShowWithTrailers.TVShow.Should().NotBeNull();
            tvShowWithTrailers.Trailers.Should().NotBeNull();
            tvShowWithTrailers.Trailers.Any().Should().BeTrue();
            tvShowWithTrailers.Trailers.Count().Should().Be(query.TrailersCount);
            tvShowWithTrailers.Trailers.All(x => string.IsNullOrEmpty(x.Key) == false).Should().BeTrue();

            tvShowWithTrailers.TVShow.FirstAirDate.Date.Should().Be(TvShowMock.Data.FirstAirDate);
            tvShowWithTrailers.TVShow.Homepage.Should().Be(TvShowMock.Data.Homepage);
            tvShowWithTrailers.TVShow.Name.Should().Be(TvShowMock.Data.Name);

            tvShowWithTrailers.TVShow.CreatedBy.Should().NotBeNull().And.Subject.Count().Should()
                .Be(TvShowMock.Data.CreatedBy.Count);
            tvShowWithTrailers.TVShow.EpisodeRunTime.Count.Should().Be(TvShowMock.Data.EpisodeRunTime.Count);
            tvShowWithTrailers.TVShow.Languages.Should().NotBeNull().And.Subject.Count().Should()
                .Be(TvShowMock.Data.Languages.Count);
            tvShowWithTrailers.TVShow.Genres.Should().NotBeNull().And.Subject.Count().Should()
                .Be(TvShowMock.Data.Genres.Count);
            tvShowWithTrailers.TVShow.Networks.Should().NotBeNull().And.Subject.Count().Should()
                .Be(TvShowMock.Data.Networks.Count);
            tvShowWithTrailers.TVShow.OriginCountry.Should().NotBeNull().And.Subject.Count().Should()
                .Be(TvShowMock.Data.OriginCountry.Count);
            tvShowWithTrailers.TVShow.OriginalLanguage.Should().NotBeNull().And.Subject.Length.Should()
                .Be(TvShowMock.Data.OriginalLanguage.Length);
            tvShowWithTrailers.TVShow.ProductionCompanies.Should().NotBeNull().And.Subject.Count().Should()
                .Be(TvShowMock.Data.ProductionCompanies.Count);

            TMDBTestUtil.AssertImagePath(tvShowWithTrailers.TVShow.BackdropPath);
            TMDBTestUtil.AssertImagePath(tvShowWithTrailers.TVShow.PosterPath);
        }
    }
}