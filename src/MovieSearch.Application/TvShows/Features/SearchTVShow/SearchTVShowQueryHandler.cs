using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using MovieSearch.Application.Services.Clients;
using MovieSearch.Application.TvShows.Dtos;

namespace MovieSearch.Application.TvShows.Features.SearchTVShow
{
    public class SearchTVShowQueryHandler : IRequestHandler<SearchTVShowQuery, SearchTVShowQueryResult>
    {
        private readonly IMovieDbServiceClient _movieDbServiceClient;
        private readonly IMapper _mapper;

        public SearchTVShowQueryHandler(IMovieDbServiceClient movieDbServiceClient, IMapper mapper)
        {
            _movieDbServiceClient = movieDbServiceClient;
            _mapper = mapper;
        }

        public async Task<SearchTVShowQueryResult> Handle(SearchTVShowQuery query, CancellationToken cancellationToken)
        {
            Guard.Against.Null(query, nameof(SearchTVShowQuery));

            var tvShows = await _movieDbServiceClient.SearchTvShowAsync(keyword: query.SearchKeywords,
                page: query.Page, includeAdult: query.IncludeAdult, firstAirDateYear: query.FirstAirDateYear,
                cancellationToken: cancellationToken);

            var result = tvShows.Map(x => _mapper.Map<TVShowInfoDto>(x));

            return new SearchTVShowQueryResult { TVShowList = result };
        }
    }
}