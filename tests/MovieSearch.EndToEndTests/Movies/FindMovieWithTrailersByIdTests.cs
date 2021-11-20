using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BuildingBlocks.Security.ApiKey;
using FluentAssertions;
using MovieSearch.Api;
using MovieSearch.Application.Movies.Dtos;
using MovieSearch.Application.Movies.Features.FindMovieWithTrailersById;
using MovieSearch.EndToEndTests.Mocks;
using Trill.Shared.Tests.Integration;
using Xunit;

namespace MovieSearch.EndToEndTests.Movies
{
    public class FindMovieWithTrailersByIdTests : WebApiTestFixture<Startup>
    {
        private Task<HttpResponseMessage> Act(int movieId, int trailersCount = 20)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"api/v1/movies/{movieId}/with-trailers?trailersCount={trailersCount}",
                    UriKind.RelativeOrAbsolute),
                Headers = { { ApiKeyConstants.HeaderName, "C5BFF7F0-B4DF-475E-A331-F737424F013C" } }
            };
            return Client.SendAsync(httpRequestMessage);
        }

        [Fact]
        public async Task find_movie_with_trailers_by_id_endpoint_should_return_http_status_code_ok()
        {
            // Act
            var response = await Act(MovieMocks.Data.Id, 20);

            // Assert
            response.IsSuccessStatusCode.Should().Be(true);
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task find_movie_with_trailers_by_id_endpoint_should_return_correct_data()
        {
            // Act
            var response = await Act(MovieMocks.Data.Id, 20);
            var result = await response.Content.ReadFromJsonAsync<FindMovieWithTrailersByIdQueryResult>();

            // Assert
            response.IsSuccessStatusCode.Should().Be(true);
            result.Should().NotBeNull();
            result.Should().BeOfType<FindMovieWithTrailersByIdQueryResult>();
            result?.MovieWithTrailers.Should().NotBeNull();
            result?.MovieWithTrailers.Movie.Should().NotBeNull();
            result?.MovieWithTrailers.Movie.Id.Should().Be(MovieMocks.Data.Id);
            result?.MovieWithTrailers.Trailers.Should().NotBeNull();
            result?.MovieWithTrailers.Trailers.Any().Should().BeTrue();
        }

        [Fact]
        public async Task find_movie_with_trailers_by_id_endpoint_should_return_unauthorized_without_an_api_key()
        {
            var response =
                await Client.GetAsync(
                    $"api/v1/movies/{MovieMocks.Data.Id.ToString()}/with-trailers?trailersCount={20}");

            response.IsSuccessStatusCode.Should().Be(false);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}