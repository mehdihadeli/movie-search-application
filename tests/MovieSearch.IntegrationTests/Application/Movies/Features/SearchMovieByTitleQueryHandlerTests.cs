using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Domain;
using BuildingBlocks.Test.Fixtures;
using DM.MovieApi.IntegrationTests;
using FluentAssertions;
using MovieSearch.Api;
using MovieSearch.Application.Movies.Dtos;
using MovieSearch.Application.Movies.Features.SearchMovie;
using MovieSearch.Application.Movies.Features.SearchMovieByTitle;
using MovieSearch.IntegrationTests.Mocks;
using Xunit;

namespace MovieSearch.IntegrationTests.Application.Movies.Features
{
    public class SearchMovieByTitleQueryHandlerTests: IntegrationTestFixture<Startup>
    {
        public SearchMovieByTitleQueryHandlerTests()
        {
            //setup the swaps for our tests
            RegisterTestServices(services => { });
        }

        [Fact]
        public async Task search_movie_by_title_query_should_return_a_valid_movie_info_list_dto()
        {
            // Arrange
            var query = new SearchMovieByTitleQuery()
            {
                Page = 1,
                SearchKeywords = MovieMocks.Data.Title
            };

            // Act
            var listResult = (await QueryAsync(query, CancellationToken.None)).MovieList;

            // Assert
            listResult.Should().NotBeNull();
            listResult.Should().BeOfType<ListResultModel<MovieInfoDto>>();
            listResult.Page.Should().Be(1);
            listResult.PageSize.Should().Be(listResult.Items.Count);
            listResult.Items.Any().Should().BeTrue();
            TMDBTestUtil.AssertMovieInformationDtoStructure(listResult.Items);
        }
    }
}