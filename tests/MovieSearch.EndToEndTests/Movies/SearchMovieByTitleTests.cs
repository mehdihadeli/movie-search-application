using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BuildingBlocks.Domain;
using BuildingBlocks.Security.ApiKey;
using BuildingBlocks.Utils;
using FluentAssertions;
using MovieSearch.Api;
using MovieSearch.Application.Movies.Dtos;
using MovieSearch.Application.Movies.Features.SearchMovieByTitle;
using MovieSearch.EndToEndTests.Mocks;
using Thesaurus.Api.Words.ViewModels;
using Trill.Shared.Tests.Integration;
using Xunit;

namespace MovieSearch.EndToEndTests.Movies
{
    public class SearchMovieByTitleTests : WebApiTestFixture<Startup>
    {
        private Task<HttpResponseMessage> Act(SearchMoviesByTitleRequest request)
        {
            var queryString = request.GetQueryString();
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"api/v1/movies/search-by-title?{queryString}",
                    UriKind.RelativeOrAbsolute),
                Headers = { { ApiKeyConstants.HeaderName, "C5BFF7F0-B4DF-475E-A331-F737424F013C" } }
            };
            return Client.SendAsync(httpRequestMessage);
        }

        [Fact]
        public async Task search_movie_by_title_endpoint_should_return_http_status_code_ok()
        {
            // Act
            var response =
                await Act(new SearchMoviesByTitleRequest { Page = 1, SearchKeywords = MovieMocks.Data.Title });

            // Assert
            response.IsSuccessStatusCode.Should().Be(true);
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        [Fact]
        public async Task search_movie_by_title_endpoint_should_return_correctData()
        {
            // Act
            var response =
                await Act(new SearchMoviesByTitleRequest { Page = 1, SearchKeywords = MovieMocks.Data.Title });
            var result = await response.Content.ReadFromJsonAsync<SearchMovieByTitleQueryResult>();

            // Assert
            response.IsSuccessStatusCode.Should().Be(true);
            result.Should().NotBeNull();
            result.Should().BeOfType<SearchMovieByTitleQueryResult>();
            result?.MovieList.Should().NotBeNull();
            result?.MovieList.Should().BeOfType<ListResultModel<MovieInfoDto>>();
            result?.MovieList.Items.Should().NotBeNull();
            result?.MovieList.Items.Any().Should().BeTrue();
            result?.MovieList.Page.Should().Be(1);
            result?.MovieList.PageSize.Should().Be(result?.MovieList.Items.Count);
        }

        [Fact]
        public async Task search_movie_by_title_endpoint_should_return_unauthorized_without_an_api_key()
        {
            var request = new SearchMoviesByTitleRequest { Page = 1, SearchKeywords = MovieMocks.Data.Title };
            var queryString = request.GetQueryString();

            var response =
                await Client.GetAsync( $"api/v1/movies/search-by-title?{queryString}");

            response.IsSuccessStatusCode.Should().Be(false);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}