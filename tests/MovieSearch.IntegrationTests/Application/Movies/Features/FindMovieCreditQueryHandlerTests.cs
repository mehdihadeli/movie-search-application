using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Test.Fixtures;
using FluentAssertions;
using MovieSearch.Api;
using MovieSearch.Application.Movies.Features.FindMovieCredits;
using MovieSearch.IntegrationTests.Mocks;
using Xunit;

namespace MovieSearch.IntegrationTests.Application.Movies.Features
{
    public class FindMovieCreditQueryHandlerTests: IntegrationTestFixture<Startup>
    {
        public FindMovieCreditQueryHandlerTests()
        {
            //setup the swaps for our tests
            RegisterTestServices(services => { });
        }

        [Fact]
        public async Task find_movie_credit_query_should_return_a_valid_movie_credits_dto()
        {
            // Arrange
            var query = new FindMovieCreditsQuery(MovieMocks.Data.Id);

            // Act
            var movieCredit = (await QueryAsync(query, CancellationToken.None)).MovieCredit;

            // Assert
            movieCredit.Should().NotBeNull();
            movieCredit.MovieId.Should().Be(MovieMocks.Data.Id);
            movieCredit.CastMembers.Should().NotBeNull();
            movieCredit.CastMembers.Any().Should().BeTrue();
            movieCredit.CrewMembers.Should().NotBeNull();
            movieCredit.CrewMembers.Any().Should().BeTrue();
        }
    }
}