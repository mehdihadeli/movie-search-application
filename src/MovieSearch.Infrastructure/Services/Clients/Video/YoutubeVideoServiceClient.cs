using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BuildingBlocks.Domain;
using BuildingBlocks.Resiliency.Configs;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Options;
using MovieSearch.Application.Services.Clients;
using MovieSearch.Core;
using MovieSearch.Core.Generals;
using Polly;
using Polly.Bulkhead;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

namespace MovieSearch.Infrastructure.Services.Clients.Video
{
    //https://github.com/youtube/api-samples/tree/master/dotnet
    public class YoutubeVideoServiceClient : IVideoServiceClient
    {
        private readonly IMapper _mapper;
        private readonly YoutubeVideoOptions _options;

        private readonly AsyncRetryPolicy _retryPolicy;
        private static AsyncTimeoutPolicy _timeoutPolicy;
        private static AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
        private static AsyncBulkheadPolicy _bulkheadPolicy;

        public YoutubeVideoServiceClient(IOptions<YoutubeVideoOptions> options, IMapper mapper, IOptions<PolicyConfig> policyOptions)
        {
            _mapper = mapper;
            _options = options.Value;

            _retryPolicy = Policy.Handle<Exception>().RetryAsync(policyOptions.Value.RetryCount);
            _timeoutPolicy = Policy.TimeoutAsync(policyOptions.Value.TimeOutDuration, TimeoutStrategy.Pessimistic);
            _circuitBreakerPolicy = Policy.Handle<Exception>().CircuitBreakerAsync(policyOptions.Value.RetryCount + 1,
                TimeSpan.FromSeconds(policyOptions.Value.BreakDuration));
            _bulkheadPolicy = Policy.BulkheadAsync(3, 6);

            _retryPolicy.WrapAsync(_circuitBreakerPolicy).WrapAsync(_timeoutPolicy);
        }

        //https://developers.google.com/youtube/v3/docs/search/list
        public async Task<VideoListResultModel<string>> GetVideos(string movieName, string pageToken = "")
        {
            YouTubeService youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _options.ApiKey,
                ApplicationName = GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List(_options.SearchPart);
            searchListRequest.Q = movieName;
            searchListRequest.Order =
                _options.Order; //our default config: Relevance - Resources are sorted based on their relevance to the search query.
            searchListRequest.MaxResults = _options.MaxResults;
            searchListRequest.PageToken = pageToken;
            searchListRequest.Type = _options.SearchType;
            searchListRequest.VideoEmbeddable = SearchResource.ListRequest.VideoEmbeddableEnum.True__;

            var searchListResponse = await _retryPolicy.ExecuteAsync(() => searchListRequest.ExecuteAsync());

            return new VideoListResultModel<string>(items: searchListResponse.Items.Select(x => x.Id.VideoId).ToList(),
                totalItems: searchListResponse.PageInfo.TotalResults ?? 0, pageToken: pageToken,
                nextPageToken: searchListResponse.NextPageToken,
                previousPageToken: searchListResponse.PrevPageToken,
                pageSize: searchListResponse.PageInfo.ResultsPerPage ?? 0);
        }
    }
}